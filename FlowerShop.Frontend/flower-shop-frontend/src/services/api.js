import axios from 'axios'

const API_BASE_URL = 'http://localhost:5000/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Categories API
export const categoriesApi = {
  getAll: () => api.get('/categories'),
  getById: (id) => api.get(`/categories/${id}`)
}

// Flowers API
export const flowersApi = {
  getAll: (params = {}) => api.get('/flowers', { params }),
  getById: (id) => api.get(`/flowers/${id}`),
  getFeatured: () => api.get('/flowers/featured')
}

// Orders API
export const ordersApi = {
  create: (orderData) => api.post('/orders', orderData),
  getById: (id) => api.get(`/orders/${id}`),
  updateStatus: (id, status) => api.put(`/orders/${id}/status`, status)
}

export default api
