<template>
  <div class="occasion-flowers-view">
    <!-- Compact Header -->
    <div class="compact-header bg-primary text-white py-3">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-auto">
            <button 
              class="btn btn-outline-light btn-sm"
              @click="$router.go(-1)"
            >
              <i class="bi bi-arrow-left me-1"></i>
              Geri
            </button>
          </div>
          <div class="col">
            <div v-if="occasion" class="d-flex align-items-center">
              <i :class="`${occasion.icon} me-3 fs-4`"></i>
              <div>
                <h4 class="mb-0">{{ occasion.name }}</h4>
                <small class="opacity-75">{{ occasion.description }}</small>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Content -->
    <div class="container py-4">
      <!-- Loading State -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Yükleniyor...</span>
        </div>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="alert alert-danger text-center">
        <i class="bi bi-exclamation-triangle me-2"></i>
        {{ error }}
      </div>

      <!-- Flowers Grid -->
      <div v-else>
        <div class="row mb-4">
          <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
              <h5 class="mb-0">
                {{ flowers.length }} Buket Bulundu
              </h5>
              <div class="btn-group btn-group-sm" role="group">
                <input type="radio" class="btn-check" name="sortOptions" id="sortName" autocomplete="off" v-model="sortBy" value="name">
                <label class="btn btn-outline-secondary" for="sortName">İsim</label>
                
                <input type="radio" class="btn-check" name="sortOptions" id="sortPrice" autocomplete="off" v-model="sortBy" value="price">
                <label class="btn btn-outline-secondary" for="sortPrice">Fiyat</label>
              </div>
            </div>
          </div>
        </div>

        <div class="row g-4">
          <div 
            v-for="flower in sortedFlowers" 
            :key="flower.id"
            class="col-lg-4 col-md-6"
          >
            <div class="card h-100 shadow-sm border-0 flower-card">
              <div class="position-relative">
                <img 
                  :src="flower.imageUrl" 
                  :alt="flower.name"
                  class="card-img-top"
                  style="height: 250px; object-fit: cover;"
                  @error="$event.target.src = '/images/placeholder-flower.jpg'"
                >
                <div class="position-absolute top-0 end-0 m-2">
                  <span class="badge bg-success">Stokta {{ flower.stock }}</span>
                </div>
              </div>
              <div class="card-body d-flex flex-column">
                <h5 class="card-title fw-bold">{{ flower.name }}</h5>
                <p class="card-text text-muted flex-grow-1">{{ flower.description }}</p>
                <div class="d-flex justify-content-between align-items-center mt-auto">
                  <div>
                    <span class="h5 text-primary fw-bold">{{ flower.price }}₺</span>
                    <br>
                    <small class="text-muted">{{ flower.categoryName }}</small>
                  </div>
                  <div class="btn-group-vertical btn-group-sm">
                    <button 
                      class="btn btn-outline-primary"
                      @click="viewFlowerDetail(flower.id)"
                    >
                      <i class="bi bi-eye me-1"></i>
                      Detay
                    </button>
                    <button 
                      class="btn btn-primary"
                      @click="addToCart(flower)"
                      :disabled="flower.stock === 0"
                    >
                      <i class="bi bi-cart-plus me-1"></i>
                      Sepete Ekle
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Empty State -->
        <div v-if="flowers.length === 0" class="text-center py-5">
          <i class="bi bi-flower1 display-1 text-muted mb-3"></i>
          <h4>Bu özel gün için henüz buket bulunmuyor</h4>
          <p class="text-muted">Diğer özel günlere göz atabilirsiniz.</p>
          <button 
            class="btn btn-primary"
            @click="$router.push({ name: 'occasions' })"
          >
            <i class="bi bi-arrow-left me-2"></i>
            Özel Günlere Dön
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { occasionsApi } from '../services/api'

export default {
  name: 'OccasionFlowersView',
  data() {
    return {
      occasion: null,
      flowers: [],
      loading: true,
      error: null,
      sortBy: 'name'
    }
  },
  computed: {
    sortedFlowers() {
      const sorted = [...this.flowers]
      if (this.sortBy === 'name') {
        return sorted.sort((a, b) => a.name.localeCompare(b.name, 'tr'))
      } else if (this.sortBy === 'price') {
        return sorted.sort((a, b) => a.price - b.price)
      }
      return sorted
    }
  },
  async mounted() {
    await this.fetchOccasionFlowers()
  },
  watch: {
    '$route.params.id': {
      async handler(newId, oldId) {
        if (newId && newId !== oldId) {
          await this.fetchOccasionFlowers()
        }
      },
      immediate: false
    }
  },
  methods: {
    async fetchOccasionFlowers() {
      try {
        this.loading = true
        this.error = null
        const occasionId = this.$route.params.id

        const response = await occasionsApi.getFlowers(occasionId)
        this.occasion = response.data.occasion
        this.flowers = response.data.flowers
      } catch (error) {
        console.error('Error fetching occasion flowers:', error)
        this.error = 'Çiçekler yüklenirken bir hata oluştu.'
      } finally {
        this.loading = false
      }
    },
    viewFlowerDetail(flowerId) {
      this.$router.push({ name: 'flower-detail', params: { id: flowerId } })
    },
    addToCart(flower) {
      const cart = JSON.parse(localStorage.getItem('flowerShopCart') || '[]')
      const existingItem = cart.find(item => item.id === flower.id)

      if (existingItem) {
        existingItem.quantity += 1
      } else {
        cart.push({
          id: flower.id,
          name: flower.name,
          price: flower.price,
          image: flower.image,
          quantity: 1
        })
      }

      localStorage.setItem('flowerShopCart', JSON.stringify(cart))

      // Show success message
      alert(`${flower.name} sepete eklendi!`)

      // Update cart count in header if needed
      window.dispatchEvent(new Event('cartUpdated'))
    }
  }
}
</script>

<style scoped>
.compact-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.flower-card {
  transition: all 0.3s ease;
}

.flower-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 30px rgba(0,0,0,0.15) !important;
}

.btn-group-vertical .btn {
  border-radius: 0;
}

.btn-group-vertical .btn:first-child {
  border-top-left-radius: 0.375rem;
  border-top-right-radius: 0.375rem;
}

.btn-group-vertical .btn:last-child {
  border-bottom-left-radius: 0.375rem;
  border-bottom-right-radius: 0.375rem;
}
</style>
