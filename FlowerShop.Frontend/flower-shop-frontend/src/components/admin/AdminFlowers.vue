<template>
  <div class="admin-flowers">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h5 class="mb-0">Çiçek Yönetimi</h5>
      <button class="btn btn-primary" @click="showAddModal = true">
        <i class="bi bi-plus-circle me-2"></i>
        Yeni Çiçek Ekle
      </button>
    </div>

    <!-- Filters -->
    <div class="card mb-4">
      <div class="card-body">
        <div class="row g-3">
          <div class="col-md-4">
            <label class="form-label">Kategori Filtresi</label>
            <select v-model="filters.categoryId" class="form-select" @change="loadFlowers">
              <option value="">Tüm Kategoriler</option>
              <option v-for="category in categories" :key="category.id" :value="category.id">
                {{ category.name }}
              </option>
            </select>
          </div>
          <div class="col-md-4">
            <label class="form-label">Özel Gün Filtresi</label>
            <select v-model="filters.occasionId" class="form-select" @change="loadFlowers">
              <option value="">Tüm Özel Günler</option>
              <option v-for="occasion in occasions" :key="occasion.id" :value="occasion.id">
                {{ occasion.name }}
              </option>
            </select>
          </div>
          <div class="col-md-4">
            <label class="form-label">Arama</label>
            <input 
              v-model="filters.search" 
              type="text" 
              class="form-control" 
              placeholder="Çiçek adı ara..."
              @input="debounceSearch"
            >
          </div>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Yükleniyor...</span>
      </div>
    </div>

    <!-- Flowers Table -->
    <div v-else class="card">
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Resim</th>
                <th>Ad</th>
                <th>Kategori</th>
                <th>Özel Gün</th>
                <th>Fiyat</th>
                <th>Stok</th>
                <th>Durum</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="flower in flowers" :key="flower.id">
                <td>
                  <img 
                    :src="flower.imageUrl" 
                    :alt="flower.name"
                    style="width: 50px; height: 50px; object-fit: cover; border-radius: 4px;"
                    @error="$event.target.src = '/images/placeholder-flower.jpg'"
                  >
                </td>
                <td>
                  <strong>{{ flower.name }}</strong>
                  <br>
                  <small class="text-muted">{{ flower.description }}</small>
                </td>
                <td>{{ flower.categoryName }}</td>
                <td>{{ flower.occasionName || '-' }}</td>
                <td>{{ flower.price }}₺</td>
                <td>
                  <span :class="`badge ${flower.stock > 10 ? 'bg-success' : flower.stock > 0 ? 'bg-warning' : 'bg-danger'}`">
                    {{ flower.stock }}
                  </span>
                </td>
                <td>
                  <span :class="`badge ${flower.isActive ? 'bg-success' : 'bg-secondary'}`">
                    {{ flower.isActive ? 'Aktif' : 'Pasif' }}
                  </span>
                </td>
                <td>
                  <div class="btn-group btn-group-sm">
                    <button class="btn btn-outline-primary" @click="editFlower(flower)">
                      <i class="bi bi-pencil"></i>
                    </button>
                    <button 
                      class="btn btn-outline-danger" 
                      @click="deleteFlower(flower)"
                      :disabled="!flower.isActive"
                    >
                      <i class="bi bi-trash"></i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <div class="modal fade" :class="{ show: showAddModal }" :style="{ display: showAddModal ? 'block' : 'none' }" tabindex="-1">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ editingFlower ? 'Çiçek Düzenle' : 'Yeni Çiçek Ekle' }}</h5>
            <button type="button" class="btn-close" @click="closeModal"></button>
          </div>
          <form @submit.prevent="saveFlower">
            <div class="modal-body">
              <div class="row g-3">
                <div class="col-md-6">
                  <label class="form-label">Çiçek Adı *</label>
                  <input v-model="flowerForm.name" type="text" class="form-control" required>
                </div>
                <div class="col-md-6">
                  <label class="form-label">Fiyat *</label>
                  <div class="input-group">
                    <input v-model="flowerForm.price" type="number" step="0.01" class="form-control" required>
                    <span class="input-group-text">₺</span>
                  </div>
                </div>
                <div class="col-12">
                  <label class="form-label">Açıklama *</label>
                  <textarea v-model="flowerForm.description" class="form-control" rows="3" required></textarea>
                </div>
                <div class="col-md-6">
                  <label class="form-label">Kategori *</label>
                  <select v-model="flowerForm.categoryId" class="form-select" required>
                    <option value="">Kategori Seçin</option>
                    <option v-for="category in categories" :key="category.id" :value="category.id">
                      {{ category.name }}
                    </option>
                  </select>
                </div>
                <div class="col-md-6">
                  <label class="form-label">Özel Gün</label>
                  <select v-model="flowerForm.occasionId" class="form-select">
                    <option value="">Özel Gün Seçin</option>
                    <option v-for="occasion in occasions" :key="occasion.id" :value="occasion.id">
                      {{ occasion.name }}
                    </option>
                  </select>
                </div>
                <div class="col-md-6">
                  <label class="form-label">Stok *</label>
                  <input v-model="flowerForm.stock" type="number" class="form-control" required>
                </div>
                <div class="col-md-6">
                  <label class="form-label">Resim URL *</label>
                  <input v-model="flowerForm.imageUrl" type="url" class="form-control" required>
                </div>
                <div class="col-12">
                  <div class="form-check">
                    <input v-model="flowerForm.isActive" class="form-check-input" type="checkbox" id="isActive">
                    <label class="form-check-label" for="isActive">
                      Aktif
                    </label>
                  </div>
                </div>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" @click="closeModal">İptal</button>
              <button type="submit" class="btn btn-primary" :disabled="saving">
                <span v-if="saving" class="spinner-border spinner-border-sm me-2"></span>
                {{ editingFlower ? 'Güncelle' : 'Ekle' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
    <div v-if="showAddModal" class="modal-backdrop fade show"></div>
  </div>
</template>

<script>
import { flowersApi, categoriesApi, occasionsApi } from '../../services/api'

export default {
  name: 'AdminFlowers',
  data() {
    return {
      flowers: [],
      categories: [],
      occasions: [],
      loading: false,
      saving: false,
      showAddModal: false,
      editingFlower: null,
      searchTimeout: null,
      filters: {
        categoryId: '',
        occasionId: '',
        search: ''
      },
      flowerForm: {
        name: '',
        description: '',
        price: '',
        stock: '',
        categoryId: '',
        occasionId: '',
        imageUrl: '',
        isActive: true
      }
    }
  },
  async mounted() {
    await this.loadData()
  },
  methods: {
    async loadData() {
      await Promise.all([
        this.loadFlowers(),
        this.loadCategories(),
        this.loadOccasions()
      ])
    },
    
    async loadFlowers() {
      try {
        this.loading = true
        const params = new URLSearchParams()
        if (this.filters.categoryId) params.append('categoryId', this.filters.categoryId)
        if (this.filters.occasionId) params.append('occasionId', this.filters.occasionId)
        if (this.filters.search) params.append('search', this.filters.search)
        
        const response = await flowersApi.getAll(params.toString())
        this.flowers = response.data.flowers
      } catch (error) {
        console.error('Error loading flowers:', error)
      } finally {
        this.loading = false
      }
    },
    
    async loadCategories() {
      try {
        const response = await categoriesApi.getAll()
        this.categories = response.data.categories
      } catch (error) {
        console.error('Error loading categories:', error)
      }
    },
    
    async loadOccasions() {
      try {
        const response = await occasionsApi.getAll()
        this.occasions = response.data.occasions
      } catch (error) {
        console.error('Error loading occasions:', error)
      }
    },
    
    debounceSearch() {
      clearTimeout(this.searchTimeout)
      this.searchTimeout = setTimeout(() => {
        this.loadFlowers()
      }, 500)
    },
    
    editFlower(flower) {
      this.editingFlower = flower
      this.flowerForm = {
        name: flower.name,
        description: flower.description,
        price: flower.price,
        stock: flower.stock,
        categoryId: flower.categoryId,
        occasionId: flower.occasionId || '',
        imageUrl: flower.imageUrl,
        isActive: flower.isActive
      }
      this.showAddModal = true
    },
    
    async saveFlower() {
      try {
        this.saving = true
        
        const flowerData = {
          ...this.flowerForm,
          occasionId: this.flowerForm.occasionId || null
        }
        
        if (this.editingFlower) {
          // Update existing flower
          await flowersApi.update(this.editingFlower.id, flowerData)
        } else {
          // Create new flower
          await flowersApi.create(flowerData)
        }
        
        await this.loadFlowers()
        this.closeModal()
        
      } catch (error) {
        console.error('Error saving flower:', error)
        alert('Çiçek kaydedilirken bir hata oluştu.')
      } finally {
        this.saving = false
      }
    },
    
    async deleteFlower(flower) {
      if (confirm(`"${flower.name}" adlı çiçeği silmek istediğinizden emin misiniz?`)) {
        try {
          await flowersApi.delete(flower.id)
          await this.loadFlowers()
        } catch (error) {
          console.error('Error deleting flower:', error)
          alert('Çiçek silinirken bir hata oluştu.')
        }
      }
    },
    
    closeModal() {
      this.showAddModal = false
      this.editingFlower = null
      this.flowerForm = {
        name: '',
        description: '',
        price: '',
        stock: '',
        categoryId: '',
        occasionId: '',
        imageUrl: '',
        isActive: true
      }
    }
  }
}
</script>

<style scoped>
.modal.show {
  background-color: rgba(0,0,0,0.5);
}
</style>
