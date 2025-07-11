<template>
  <div class="occasions-view">
    <!-- Hero Section -->
    <div class="hero-section bg-primary text-white py-5">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-lg-8">
            <h1 class="display-4 fw-bold mb-3">Özel Günleriniz İçin</h1>
            <p class="lead mb-4">
              Her özel anınız için özenle hazırlanmış çiçek buketleri. 
              Sevdiklerinize duygularınızı en güzel şekilde ifade edin.
            </p>
          </div>
          <div class="col-lg-4 text-center">
            <i class="bi bi-heart-fill display-1"></i>
          </div>
        </div>
      </div>
    </div>

    <!-- Occasions Grid -->
    <div class="container py-5">
      <div class="row">
        <div class="col-12">
          <h2 class="text-center mb-5">Hangi Özel Gün İçin Çiçek Arıyorsunuz?</h2>
        </div>
      </div>

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

      <!-- Occasions Grid -->
      <div v-else class="row g-4">
        <div 
          v-for="occasion in occasions" 
          :key="occasion.id"
          class="col-lg-3 col-md-4 col-sm-6"
        >
          <div 
            class="occasion-card card h-100 shadow-sm border-0"
            @click="goToOccasionFlowers(occasion.id)"
            style="cursor: pointer; transition: transform 0.2s;"
            @mouseenter="$event.target.style.transform = 'translateY(-5px)'"
            @mouseleave="$event.target.style.transform = 'translateY(0)'"
          >
            <div class="card-body text-center p-4">
              <div class="occasion-icon mb-3">
                <i :class="`${occasion.icon} display-4 text-primary`"></i>
              </div>
              <h5 class="card-title fw-bold mb-2">{{ occasion.name }}</h5>
              <p class="card-text text-muted small mb-3">{{ occasion.description }}</p>
              <div class="flower-count">
                <span class="badge bg-light text-dark">
                  <i class="bi bi-flower1 me-1"></i>
                  {{ occasion.flowerCount }} Buket
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Popular Occasions Section -->
    <div class="bg-light py-5">
      <div class="container">
        <div class="row">
          <div class="col-12 text-center mb-4">
            <h3>En Popüler Özel Günler</h3>
            <p class="text-muted">En çok tercih edilen özel günler için hazırladığımız buketler</p>
          </div>
        </div>
        <div class="row g-3 justify-content-center">
          <div class="col-auto">
            <button 
              class="btn btn-outline-primary btn-lg"
              @click="goToOccasionFlowers(4)"
            >
              <i class="bi bi-heart-fill me-2"></i>
              Sevgililer Günü
            </button>
          </div>
          <div class="col-auto">
            <button 
              class="btn btn-outline-primary btn-lg"
              @click="goToOccasionFlowers(5)"
            >
              <i class="bi bi-flower1 me-2"></i>
              Anneler Günü
            </button>
          </div>
          <div class="col-auto">
            <button 
              class="btn btn-outline-primary btn-lg"
              @click="goToOccasionFlowers(1)"
            >
              <i class="bi bi-gift me-2"></i>
              Doğum Günü
            </button>
          </div>
          <div class="col-auto">
            <button 
              class="btn btn-outline-primary btn-lg"
              @click="goToOccasionFlowers(2)"
            >
              <i class="bi bi-heart me-2"></i>
              Düğün
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { occasionsApi } from '../services/api'

export default {
  name: 'OccasionsView',
  data() {
    return {
      occasions: [],
      loading: true,
      error: null
    }
  },
  async mounted() {
    await this.fetchOccasions()
  },
  methods: {
    async fetchOccasions() {
      try {
        this.loading = true
        this.error = null
        const response = await occasionsApi.getAll()
        this.occasions = response.data.occasions
      } catch (error) {
        console.error('Error fetching occasions:', error)
        this.error = 'Özel günler yüklenirken bir hata oluştu.'
      } finally {
        this.loading = false
      }
    },
    goToOccasionFlowers(occasionId) {
      this.$router.push({ name: 'occasion-flowers', params: { id: occasionId } })
    }
  }
}
</script>

<style scoped>
.hero-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.occasion-card {
  transition: all 0.3s ease;
}

.occasion-card:hover {
  box-shadow: 0 10px 30px rgba(0,0,0,0.1) !important;
}

.occasion-icon {
  height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-outline-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0,0,0,0.1);
}
</style>
