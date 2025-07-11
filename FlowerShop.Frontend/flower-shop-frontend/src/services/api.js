import axios from 'axios'

const API_BASE_URL = 'http://localhost:5133/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Categories API
export const categoriesApi = {
  getAll: () => api.get('/categories'),
  getById: (id) => api.get(`/categories/${id}`),
  create: (categoryData) => api.post('/categories', categoryData),
  update: (id, categoryData) => api.put(`/categories/${id}`, categoryData),
  delete: (id) => api.delete(`/categories/${id}`)
}

// Flowers API
export const flowersApi = {
  getAll: (params = {}) => api.get('/flowers', { params }),
  getById: (id) => api.get(`/flowers/${id}`),
  getFeatured: () => api.get('/flowers/featured'),
  create: (flowerData) => api.post('/flowers', flowerData),
  update: (id, flowerData) => api.put(`/flowers/${id}`, flowerData),
  delete: (id) => api.delete(`/flowers/${id}`)
}

// Occasions API
export const occasionsApi = {
  getAll: () => api.get('/occasions'),
  getById: (id) => api.get(`/occasions/${id}`),
  getFlowers: (id) => api.get(`/occasions/${id}/flowers`)
}

// Auth API
export const authApi = {
  login: (credentials) => api.post('/auth/login', credentials),
  register: (userData) => api.post('/auth/register', userData)
}

// Orders API
export const ordersApi = {
  create: (orderData) => api.post('/orders', orderData),
  getById: (id) => api.get(`/orders/${id}`),
  getAll: () => api.get('/orders'),
  updateStatus: (id, status) => api.put(`/orders/${id}/status`, status),
  search: (orderNumber, email) => api.get(`/orders/search?orderNumber=${orderNumber}&email=${email}`)
}

// Dashboard API
export const dashboardApi = {
  getStats: () => api.get('/dashboard/stats')
}

export default api
