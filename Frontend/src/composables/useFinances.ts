import { ref, computed, watch } from 'vue'
import { getAllExpenses, createExpense, updateExpense, deleteExpense } from '@/client/sdk.gen'
import type { CreateExpenseCommand, Expense, UpdateExpenseCommand } from '@/client/types.gen'

const STORAGE_KEY = 'financial-items'

interface FilterState {
  tipo: string
  dataInicio: string
  dataFim: string
  busca: string
}

export function useFinances() {
  const items = ref<Expense[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  // Load expenses from API
  const fetchExpenses = async () => {
    isLoading.value = true
    error.value = null

    try {
      const response = await getAllExpenses()
      items.value = response.data?.items ?? []
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while fetching expenses'
      items.value = loadStoredData()
    } finally {
      isLoading.value = false
    }
  }

  // Load initial data from localStorage as fallback
  const loadStoredData = (): Expense[] => {
    const stored = localStorage.getItem(STORAGE_KEY)
    return stored ? JSON.parse(stored) : []
  }

  // Watch for changes and save to localStorage as backup
  watch(
    items,
    (newItems) => {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(newItems))
    },
    { deep: true },
  )

  // Filter state
  const filters = ref<FilterState>({
    tipo: 'Todos',
    dataInicio: '',
    dataFim: '',
    busca: '',
  })

  // Filtered and computed items
  const filteredItems = computed(() => {
    return items.value.filter((item) => {
      if (filters.value.tipo !== 'Todos' && item.type !== filters.value.tipo) return false
      if (filters.value.dataInicio && item.date && item.date < filters.value.dataInicio)
        return false
      if (filters.value.dataFim && item.date && item.date > filters.value.dataFim) return false
      if (filters.value.busca && item.description) {
        const searchTerm = filters.value.busca.toLowerCase()
        return item.description.toLowerCase().includes(searchTerm)
      }
      return true
    })
  })

  // CRUD operations
  const addItem = async (item: Omit<Expense, 'id'>) => {
    try {
      const command: CreateExpenseCommand = {
        type: item.type,
        date: item.date,
        description: item.description,
        value: item.value,
      }

      const response = await createExpense({
        body: command,
      })

      if (response.data) {
        const newItem = response.data as Expense
        items.value.push(newItem)
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while adding expense'
      const newItem: Expense = {
        ...item,
        id: crypto.randomUUID(),
      }
      items.value.push(newItem)
    }
  }

  const removeItem = async (id: string) => {
    try {
      await deleteExpense({
        path: { id },
      })

      const index = items.value.findIndex((item) => item.id === id)
      if (index !== -1) {
        items.value.splice(index, 1)
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while deleting expense'
      const index = items.value.findIndex((item) => item.id === id)
      if (index !== -1) {
        items.value.splice(index, 1)
      }
    }
  }

  const updateItem = async (id: string, updates: Partial<Expense>) => {
    try {
      const command: UpdateExpenseCommand = {
        id,
        date: updates.date,
        description: updates.description,
        value: updates.value,
      }

      const response = await updateExpense({
        path: { id },
        body: command,
      })

      if (response.data) {
        const updatedItem = response.data
        const index = items.value.findIndex((item) => item.id === id)
        if (index !== -1) {
          items.value[index] = updatedItem
        }
      }
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'An error occurred while updating expense'
      const item = items.value.find((item) => item.id === id)
      if (item) {
        Object.assign(item, updates)
      }
    }
  }

  return {
    items: filteredItems,
    filters,
    isLoading,
    error,
    fetchExpenses,
    addItem,
    removeItem,
    updateItem,
  }
}
