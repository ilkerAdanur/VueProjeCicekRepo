<script setup>
import { ref, onMounted } from 'vue'
import { flowersApi, categoriesApi } from '../services/api.js'

const featuredFlowers = ref([])
const categories = ref([])
const loading = ref(true)

onMounted(async () => {
  try {
    const [flowersResponse, categoriesResponse] = await Promise.all([
      flowersApi.getFeatured(),
      categoriesApi.getAll()
    ])

    featuredFlowers.value = flowersResponse.data
    categories.value = categoriesResponse.data
  } catch (error) {
    console.error('Error loading data:', error)
  } finally {
    loading.value = false
  }
})

const addToCart = (flower) => {
  const cart = JSON.parse(localStorage.getItem('flowerShopCart') || '[]')
  const existingItem = cart.find(item => item.id === flower.id)

  if (existingItem) {
    existingItem.quantity += 1
  } else {
    cart.push({ ...flower, quantity: 1 })
  }

  localStorage.setItem('flowerShopCart', JSON.stringify(cart))

  // Update cart count in parent component
  window.dispatchEvent(new Event('cartUpdated'))
}
</script>

<template>
  <div>
    <!-- Hero Section -->
    <section class="hero-section bg-success text-white py-5">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-lg-6">
            <h1 class="display-4 fw-bold">Çiçek Dükkanı</h1>
            <p class="lead">En taze ve güzel çiçeklerle hayatınıza renk katın. Sevdiklerinize özel anlar yaşatın.</p>
            <router-link to="/flowers" class="btn btn-light btn-lg">
              <i class="bi bi-flower1"></i> Çiçekleri Keşfet
            </router-link>
          </div>
          <div class="col-lg-6">
            <img src="https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=500"
                 alt="Çiçekler" class="img-fluid rounded shadow">
          </div>
        </div>
      </div>
    </section>

    <!-- Categories Section -->
    <section class="py-5">
      <div class="container">
        <h2 class="text-center mb-5">Kategoriler</h2>
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-success" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
        </div>
        <div v-else class="row">
          <div v-for="category in categories" :key="category.id" class="col-md-4 mb-4">
            <div class="card h-100 shadow-sm">
              <div class="card-body text-center">
                <i class="bi bi-flower2 text-success" style="font-size: 3rem;"></i>
                <h5 class="card-title mt-3">{{ category.name }}</h5>
                <p class="card-text">{{ category.description }}</p>
                <router-link :to="`/flowers?categoryId=${category.id}`" class="btn btn-outline-success">
                  Görüntüle
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Flowers Section -->
    <section class="py-5 bg-light">
      <div class="container">
        <h2 class="text-center mb-5">Öne Çıkan Çiçekler</h2>
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-success" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
        </div>
        <div v-else class="row">
          <div v-for="flower in featuredFlowers" :key="flower.id" class="col-lg-3 col-md-6 mb-4">
            <div class="card h-100 shadow-sm">
              <img :src="flower.imageUrl || 'https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=300'"
                   class="card-img-top" :alt="flower.name" style="height: 200px; object-fit: cover;">
              <div class="card-body d-flex flex-column">
                <h5 class="card-title">{{ flower.name }}</h5>
                <p class="card-text">{{ flower.description }}</p>
                <div class="mt-auto">
                  <div class="d-flex justify-content-between align-items-center">
                    <span class="h5 text-success mb-0">{{ flower.price }}₺</span>
                    <div>
                      <router-link :to="`/flowers/${flower.id}`" class="btn btn-sm btn-outline-primary me-2">
                        Detay
                      </router-link>
                      <button @click="addToCart(flower)" class="btn btn-sm btn-success">
                        <i class="bi bi-cart-plus"></i>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<style scoped>
.hero-section {
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
}

.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-5px);
}
</style>
