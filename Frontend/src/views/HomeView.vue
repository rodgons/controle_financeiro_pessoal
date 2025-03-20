<template>
  <div class="p-4 pb-12 max-w-6xl mx-auto">
    <h1 class="text-2xl font-semibold mb-6">Gerenciador Financeiro</h1>

    <!-- Loading and Error states -->
    <div v-if="isLoading" class="bg-blue-50 p-4 rounded-lg shadow-md mb-6">
      <p class="text-blue-700">Carregando dados...</p>
    </div>

    <div v-if="error" class="bg-red-50 p-4 rounded-lg shadow-md mb-6">
      <p class="text-red-700">{{ error }}</p>
      <button
        @click="loadData"
        class="mt-2 px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-opacity-50"
      >
        Tentar Novamente
      </button>
    </div>

    <!-- Formulário para adicionar novo item -->
    <div class="bg-white p-6 rounded-lg shadow-md mb-6">
      <h2 class="text-xl font-medium mb-4">Adicionar Novo Item</h2>
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div>
          <label for="new-item-tipo" class="block text-sm font-medium text-gray-700 mb-1"
            >Tipo</label
          >
          <select
            id="new-item-tipo"
            v-model="newItem.type"
            class="w-full p-2 border rounded focus:ring focus:ring-blue-200 focus:border-blue-500"
          >
            <option value="Receita">Receita</option>
            <option value="Despesa">Despesa</option>
          </select>
        </div>
        <div>
          <label for="new-item-data" class="block text-sm font-medium text-gray-700 mb-1"
            >Data</label
          >
          <input
            id="new-item-data"
            type="date"
            v-model="newItem.date"
            class="w-full p-2 border rounded focus:ring focus:ring-blue-200 focus:border-blue-500"
          />
        </div>
        <div>
          <label for="new-item-descricao" class="block text-sm font-medium text-gray-700 mb-1"
            >Descrição</label
          >
          <input
            id="new-item-descricao"
            type="text"
            v-model="newItem.description"
            class="w-full p-2 border rounded focus:ring focus:ring-blue-200 focus:border-blue-500"
            placeholder="Descrição do item"
          />
        </div>
        <div>
          <label for="new-item-valor" class="block text-sm font-medium text-gray-700 mb-1"
            >Valor</label
          >
          <input
            id="new-item-valor"
            type="number"
            v-model="newItem.value"
            step="0.01"
            class="w-full p-2 border rounded focus:ring focus:ring-blue-200 focus:border-blue-500"
            placeholder="0,00"
          />
        </div>
      </div>
      <button
        @click="adicionarItem"
        :disabled="isAddingItem"
        class="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        {{ isAddingItem ? 'Adicionando...' : 'Adicionar Item' }}
      </button>
      <div v-if="addError" class="mt-4 p-4 bg-red-50 text-red-700 rounded-lg">
        {{ addError }}
      </div>
    </div>

    <!-- Tabela de itens -->
    <div class="bg-white rounded-lg shadow-md overflow-hidden">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th
              v-for="column in columns"
              :key="column.id"
              class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer"
              @click="() => sortBy(column.id)"
            >
              {{ column.header }}
              <span v-if="sorting.id === column.id" class="ml-1">
                {{ sorting.desc ? '↓' : '↑' }}
              </span>
            </th>
            <th
              class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
            >
              Ações
            </th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="item in sortedItems" :key="item.id" class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap">
              <span
                :class="{
                  'px-2 py-1 text-xs font-medium rounded-full': true,
                  'bg-green-100 text-green-800': item.type === 'Receita',
                  'bg-red-100 text-red-800': item.type === 'Despesa',
                }"
              >
                {{ item.type }}
              </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">{{ formatarData(item.date) }}</td>
            <td class="px-6 py-4">{{ item.description }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-right">{{ formatarValor(item.value) }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm">
              <button
                @click="() => item.id && removeItem(item.id)"
                class="text-red-600 hover:text-red-900 focus:outline-none cursor-pointer"
              >
                Excluir
              </button>
            </td>
          </tr>
          <tr v-if="items.length === 0">
            <td colspan="5" class="px-6 py-4 text-center text-gray-500">
              Nenhum item encontrado. Adicione um novo item acima.
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useFinances } from '@/composables/useFinances'
import type { Expense } from '@/client/types.gen'

interface SortingState {
  id: keyof Expense
  desc: boolean
}

const { items, addItem, removeItem, fetchExpenses, isLoading, error } = useFinances()

const isAddingItem = ref(false)
const addError = ref<string | null>(null)

// Estado para o novo item
const newItem = ref({
  type: 'Receita',
  date: '',
  description: '',
  value: '',
})

// Estado para ordenação
const sorting = ref<SortingState>({
  id: 'date',
  desc: true,
})

// Definição das colunas
const columns = [
  { id: 'type' as const, header: 'Tipo' },
  { id: 'date' as const, header: 'Data' },
  { id: 'description' as const, header: 'Descrição' },
  { id: 'value' as const, header: 'Valor' },
]

// Função para adicionar um novo item
const adicionarItem = async () => {
  if (!newItem.value.date || !newItem.value.description || !newItem.value.value) {
    alert('Preencha todos os campos')
    return
  }

  await addItem({
    type: newItem.value.type,
    date: newItem.value.date,
    description: newItem.value.description,
    value: parseFloat(newItem.value.value),
  })

  // Limpa o formulário
  newItem.value = {
    type: 'Receita',
    date: '',
    description: '',
    value: '',
  }
}

// Função para ordenar os dados
const sortBy = (columnId: keyof Expense) => {
  if (sorting.value.id === columnId) {
    sorting.value.desc = !sorting.value.desc
  } else {
    sorting.value = { id: columnId, desc: false }
  }
}

// Dados ordenados
const sortedItems = computed(() => {
  const { id, desc } = sorting.value
  const direction = desc ? -1 : 1

  return [...items.value].sort((a, b) => {
    const aValue = a[id]
    const bValue = b[id]

    if (id === 'value') {
      return (((aValue as number) ?? 0) - ((bValue as number) ?? 0)) * direction
    }
    return String(aValue ?? '').localeCompare(String(bValue ?? '')) * direction
  })
})

// Formatação da data
const formatarData = (dataStr: string | undefined) => {
  if (!dataStr) return ''
  const data = new Date(dataStr)
  return data.toLocaleDateString('pt-BR')
}

// Formatação do valor
const formatarValor = (valor: number | undefined) => {
  return (valor ?? 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

// Load data when component mounts
const loadData = async () => {
  await fetchExpenses()
}

onMounted(() => {
  loadData()
})
</script>
