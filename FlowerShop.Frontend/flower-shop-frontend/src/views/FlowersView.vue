<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import { flowersApi, categoriesApi } from '../services/api.js'

const route = useRoute()
const flowers = ref([])
const categories = ref([])
const loading = ref(true)
const selectedCategory = ref(null)
const searchQuery = ref('')
const currentPage = ref(1)
const totalPages = ref(1)
const pageSize = 12

onMounted(async () => {
  await loadCategories()
  await loadFlowers()
  
  // Set category from query params
  if (route.query.categoryId) {
    selectedCategory.value = parseInt(route.query.categoryId)
  }
})

watch([selectedCategory, searchQuery, currentPage], () => {
  loadFlowers()
})

const loadCategories = async () => {
  try {
    const response = await categoriesApi.getAll()
    categories.value = response.data
  } catch (error) {
    console.error('Error loading categories:', error)
  }
}

const loadFlowers = async () => {
  loading.value = true
  try {
    const params = {
      page: currentPage.value,
      pageSize: pageSize
    }
    
    if (selectedCategory.value) {
      params.categoryId = selectedCategory.value
    }
    
    if (searchQuery.value) {
      params.search = searchQuery.value
    }
    
    const response = await flowersApi.getAll(params)
    flowers.value = response.data.flowers
    totalPages.value = response.data.totalPages
  } catch (error) {
    console.error('Error loading flowers:', error)
  } finally {
    loading.value = false
  }
}

const addToCart = (flower) => {
  const cart = JSON.parse(localStorage.getItem('flowerShopCart') || '[]')
  const existingItem = cart.find(item => item.id === flower.id)
  
  if (existingItem) {
    existingItem.quantity += 1
  } else {
    cart.push({ ...flower, quantity: 1 })
  }
  
  localStorage.setItem('flowerShopCart', JSON.stringify(cart))
  window.dispatchEvent(new Event('cartUpdated'))
}

const changePage = (page) => {
  currentPage.value = page
}

const resetFilters = () => {
  selectedCategory.value = null
  searchQuery.value = ''
  currentPage.value = 1
}
</script>

<template>
  <div class="container py-4">
    <div class="row">
      <!-- Sidebar Filters -->
      <div class="col-lg-3 mb-4">
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">Filtreler</h5>
          </div>
          <div class="card-body">
            <!-- Search -->
            <div class="mb-3">
              <label class="form-label">Arama</label>
              <input v-model="searchQuery" type="text" class="form-control" placeholder="Çiçek ara...">
            </div>
            
            <!-- Categories -->
            <div class="mb-3">
              <label class="form-label">Kategori</label>
              <select v-model="selectedCategory" class="form-select">
                <option :value="null">Tüm Kategoriler</option>
                <option v-for="category in categories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </option>
              </select>
            </div>
            
            <button @click="resetFilters" class="btn btn-outline-secondary w-100">
              Filtreleri Temizle
            </button>
          </div>
        </div>
      </div>
      
      <!-- Main Content -->
      <div class="col-lg-9">
        <div class="d-flex justify-content-between align-items-center mb-4">
          <h2>Çiçekler</h2>
          <span class="text-muted">{{ flowers.length }} ürün gösteriliyor</span>
        </div>
        
        <!-- Loading -->
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-success" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
        </div>
        
        <!-- Flowers Grid -->
        <div v-else-if="flowers.length > 0" class="row">
          <div v-for="flower in flowers" :key="flower.id" class="col-lg-4 col-md-6 mb-4">
            <div class="card h-100 shadow-sm">
              <img :src="flower.imageUrl || 'https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=300'" 
                   class="card-img-top" :alt="flower.name" style="height: 200px; object-fit: cover;">
              <div class="card-body d-flex flex-column">
                <h5 class="card-title">{{ flower.name }}</h5>
                <p class="card-text">{{ flower.description }}</p>
                <div class="mt-auto">
                  <div class="d-flex justify-content-between align-items-center">
                    <span class="h5 text-success mb-0">{{ flower.price }}₺</span>
                    <span class="badge bg-secondary">{{ flower.categoryName }}</span>
                  </div>
                  <div class="d-flex gap-2 mt-2">
                    <router-link :to="`/flowers/${flower.id}`" class="btn btn-outline-primary flex-fill">
                      Detay
                    </router-link>
                    <button @click="addToCart(flower)" class="btn btn-success">
                      <i class="bi bi-cart-plus"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- No Results -->
        <div v-else class="text-center py-5">
          <i class="bi bi-flower1 text-muted" style="font-size: 4rem;"></i>
          <h4 class="text-muted mt-3">Çiçek bulunamadı</h4>
          <p class="text-muted">Arama kriterlerinizi değiştirmeyi deneyin.</p>
        </div>
        
        <!-- Pagination -->
        <nav v-if="totalPages > 1" class="mt-4">
          <ul class="pagination justify-content-center">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <button class="page-link" @click="changePage(currentPage - 1)" :disabled="currentPage === 1">
                Önceki
              </button>
            </li>
            <li v-for="page in totalPages" :key="page" class="page-item" :class="{ active: currentPage === page }">
              <button class="page-link" @click="changePage(page)">{{ page }}</button>
            </li>
            <li class="page-item" :class="{ disabled: currentPage === totalPages }">
              <button class="page-link" @click="changePage(currentPage + 1)" :disabled="currentPage === totalPages">
                Sonraki
              </button>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  </div>
</template>

<style scoped>
.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-5px);
}
</style>
