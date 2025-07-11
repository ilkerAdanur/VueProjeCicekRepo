<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { flowersApi } from '../services/api.js'

const route = useRoute()
const router = useRouter()
const flower = ref(null)
const loading = ref(true)
const quantity = ref(1)

onMounted(async () => {
  try {
    const response = await flowersApi.getById(route.params.id)
    flower.value = response.data
  } catch (error) {
    console.error('Error loading flower:', error)
    router.push('/flowers')
  } finally {
    loading.value = false
  }
})

const addToCart = () => {
  const user = JSON.parse(localStorage.getItem('user') || 'null')
  const cartKey = user ? `flowerShopCart_${user.id}` : 'flowerShopCart'
  const cart = JSON.parse(localStorage.getItem(cartKey) || '[]')
  const existingItem = cart.find(item => item.id === flower.value.id)

  if (existingItem) {
    existingItem.quantity += quantity.value
  } else {
    cart.push({ ...flower.value, quantity: quantity.value })
  }

  localStorage.setItem(cartKey, JSON.stringify(cart))
  window.dispatchEvent(new Event('cartUpdated'))

  // Show success message or redirect to cart
  router.push('/cart')
}

const goBack = () => {
  router.go(-1)
}
</script>

<template>
  <div class="flower-detail-view">
    <!-- Compact Header -->
    <div class="compact-header bg-primary text-white py-3">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-auto">
            <button
              class="btn btn-outline-light btn-sm"
              @click="goBack"
            >
              <i class="bi bi-arrow-left me-1"></i>
              Geri
            </button>
          </div>
          <div class="col">
            <div v-if="flower" class="d-flex align-items-center">
              <i class="bi bi-flower1 me-3 fs-4"></i>
              <div>
                <h4 class="mb-0">{{ flower.name }}</h4>
                <small class="opacity-75">{{ flower.categoryName }}</small>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Content -->
    <div class="container py-4">
      <!-- Loading -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Yükleniyor...</span>
        </div>
      </div>

      <!-- Flower Detail -->
      <div v-else-if="flower" class="row">
      
      <!-- Flower Image -->
      <div class="col-lg-6 mb-4">
        <div class="card">
          <img :src="flower.imageUrl || 'https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=600'" 
               :alt="flower.name" class="card-img-top" style="height: 400px; object-fit: cover;">
        </div>
      </div>
      
      <!-- Flower Info -->
      <div class="col-lg-6">
        <div class="card h-100">
          <div class="card-body">
            <h1 class="card-title">{{ flower.name }}</h1>
            <p class="text-muted mb-3">{{ flower.categoryName }}</p>
            <p class="card-text">{{ flower.description }}</p>
            
            <!-- Price -->
            <div class="mb-4">
              <span class="h3 text-success">{{ flower.price }}₺</span>
            </div>
            
            <!-- Stock Info -->
            <div class="mb-4">
              <span v-if="flower.stock > 0" class="badge bg-success">
                <i class="bi bi-check-circle"></i> Stokta ({{ flower.stock }} adet)
              </span>
              <span v-else class="badge bg-danger">
                <i class="bi bi-x-circle"></i> Stokta Yok
              </span>
            </div>
            
            <!-- Quantity and Add to Cart -->
            <div v-if="flower.stock > 0" class="mb-4">
              <label class="form-label">Adet:</label>
              <div class="input-group mb-3" style="max-width: 150px;">
                <button @click="quantity = Math.max(1, quantity - 1)" 
                        class="btn btn-outline-secondary" type="button">
                  <i class="bi bi-dash"></i>
                </button>
                <input v-model.number="quantity" type="number" class="form-control text-center" 
                       min="1" :max="flower.stock">
                <button @click="quantity = Math.min(flower.stock, quantity + 1)" 
                        class="btn btn-outline-secondary" type="button">
                  <i class="bi bi-plus"></i>
                </button>
              </div>
              
              <button @click="addToCart" class="btn btn-success btn-lg w-100">
                <i class="bi bi-cart-plus"></i> Sepete Ekle
              </button>
            </div>
            
            <!-- Product Details -->
            <div class="mt-4">
              <h6>Ürün Detayları</h6>
              <ul class="list-unstyled">
                <li><strong>Kategori:</strong> {{ flower.categoryName }}</li>
                <li><strong>Stok Durumu:</strong> {{ flower.stock > 0 ? 'Mevcut' : 'Tükendi' }}</li>
                <li><strong>Eklenme Tarihi:</strong> {{ new Date(flower.createdAt).toLocaleDateString('tr-TR') }}</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      </div>

      <!-- Error State -->
      <div v-else class="text-center py-5">
        <i class="bi bi-exclamation-triangle text-warning" style="font-size: 4rem;"></i>
        <h4 class="text-muted mt-3">Ürün bulunamadı</h4>
        <p class="text-muted">Aradığınız ürün mevcut değil veya kaldırılmış olabilir.</p>
        <router-link to="/flowers" class="btn btn-primary">
          <i class="bi bi-flower1"></i> Çiçekleri Keşfet
        </router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
.compact-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.input-group input[type="number"] {
  -moz-appearance: textfield;
}

.input-group input[type="number"]::-webkit-outer-spin-button,
.input-group input[type="number"]::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}
</style>
