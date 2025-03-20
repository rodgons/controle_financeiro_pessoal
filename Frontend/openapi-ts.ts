import { createClient } from '@hey-api/openapi-ts'

createClient({
  input: 'http://localhost:5119/openapi/v1.json',
  output: 'src/client',
  plugins: ['@hey-api/client-fetch'],
})
