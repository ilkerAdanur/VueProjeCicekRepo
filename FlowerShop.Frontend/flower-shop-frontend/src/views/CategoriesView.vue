<script setup>
import { ref, onMounted } from 'vue'
import { categoriesApi } from '../services/api.js'

const categories = ref([])
const loading = ref(true)
const error = ref(null)

onMounted(async () => {
  try {
    const response = await categoriesApi.getAll()
    categories.value = response.data
  } catch (err) {
    console.error('Error loading categories:', err)
    error.value = 'Kategoriler yüklenirken bir hata oluştu.'
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <div class="container py-4">
    <div class="row">
      <div class="col-12">
        <h2 class="mb-4">Kategoriler</h2>
        
        <!-- Loading -->
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-success" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
        </div>

        <!-- Error Message -->
        <div v-else-if="error" class="alert alert-danger">
          {{ error }}
        </div>

        <!-- Categories Grid -->
        <div v-else class="row">
          <div v-for="category in categories" :key="category.id" class="col-lg-4 col-md-6 mb-4">
            <div class="card h-100 shadow-sm">
              <div class="card-body text-center">
                <i class="bi bi-flower2 text-success mb-3" style="font-size: 4rem;"></i>
                <h5 class="card-title">{{ category.name }}</h5>
                <p class="card-text">{{ category.description }}</p>
                <router-link :to="`/flowers?categoryId=${category.id}`" class="btn btn-success">
                  <i class="bi bi-arrow-right"></i> Çiçekleri Görüntüle
                </router-link>
              </div>
            </div>
          </div>
        </div>
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
