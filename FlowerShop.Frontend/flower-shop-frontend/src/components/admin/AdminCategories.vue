<template>
  <div class="admin-categories">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h5 class="mb-0">Kategori Yönetimi</h5>
      <button class="btn btn-primary" @click="showAddModal = true">
        <i class="bi bi-plus-circle me-2"></i>
        Yeni Kategori Ekle
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Yükleniyor...</span>
      </div>
    </div>

    <!-- Categories Table -->
    <div v-else class="card">
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Ad</th>
                <th>Açıklama</th>
                <th>Çiçek Sayısı</th>
                <th>Durum</th>
                <th>Oluşturulma</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="category in categories" :key="category.id">
                <td><strong>{{ category.name }}</strong></td>
                <td>{{ category.description }}</td>
                <td>
                  <span class="badge bg-info">{{ category.flowerCount || 0 }}</span>
                </td>
                <td>
                  <span :class="`badge ${category.isActive ? 'bg-success' : 'bg-secondary'}`">
                    {{ category.isActive ? 'Aktif' : 'Pasif' }}
                  </span>
                </td>
                <td>{{ formatDate(category.createdAt) }}</td>
                <td>
                  <div class="btn-group btn-group-sm">
                    <button class="btn btn-outline-primary" @click="editCategory(category)">
                      <i class="bi bi-pencil"></i>
                    </button>
                    <button 
                      class="btn btn-outline-danger" 
                      @click="deleteCategory(category)"
                      :disabled="!category.isActive || (category.flowerCount && category.flowerCount > 0)"
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
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ editingCategory ? 'Kategori Düzenle' : 'Yeni Kategori Ekle' }}</h5>
            <button type="button" class="btn-close" @click="closeModal"></button>
          </div>
          <form @submit.prevent="saveCategory">
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Kategori Adı *</label>
                <input v-model="categoryForm.name" type="text" class="form-control" required>
              </div>
              <div class="mb-3">
                <label class="form-label">Açıklama *</label>
                <textarea v-model="categoryForm.description" class="form-control" rows="3" required></textarea>
              </div>
              <div class="mb-3">
                <div class="form-check">
                  <input v-model="categoryForm.isActive" class="form-check-input" type="checkbox" id="categoryActive">
                  <label class="form-check-label" for="categoryActive">
                    Aktif
                  </label>
                </div>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" @click="closeModal">İptal</button>
              <button type="submit" class="btn btn-primary" :disabled="saving">
                <span v-if="saving" class="spinner-border spinner-border-sm me-2"></span>
                {{ editingCategory ? 'Güncelle' : 'Ekle' }}
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
import { categoriesApi } from '../../services/api'

export default {
  name: 'AdminCategories',
  data() {
    return {
      categories: [],
      loading: false,
      saving: false,
      showAddModal: false,
      editingCategory: null,
      categoryForm: {
        name: '',
        description: '',
        isActive: true
      }
    }
  },
  async mounted() {
    await this.loadCategories()
  },
  methods: {
    async loadCategories() {
      try {
        this.loading = true
        const response = await categoriesApi.getAll()
        this.categories = response.data.categories
      } catch (error) {
        console.error('Error loading categories:', error)
      } finally {
        this.loading = false
      }
    },
    
    editCategory(category) {
      this.editingCategory = category
      this.categoryForm = {
        name: category.name,
        description: category.description,
        isActive: category.isActive
      }
      this.showAddModal = true
    },
    
    async saveCategory() {
      try {
        this.saving = true
        
        if (this.editingCategory) {
          // Update existing category
          await categoriesApi.update(this.editingCategory.id, this.categoryForm)
        } else {
          // Create new category
          await categoriesApi.create(this.categoryForm)
        }
        
        await this.loadCategories()
        this.closeModal()
        
      } catch (error) {
        console.error('Error saving category:', error)
        alert('Kategori kaydedilirken bir hata oluştu.')
      } finally {
        this.saving = false
      }
    },
    
    async deleteCategory(category) {
      if (confirm(`"${category.name}" kategorisini silmek istediğinizden emin misiniz?`)) {
        try {
          await categoriesApi.delete(category.id)
          await this.loadCategories()
        } catch (error) {
          console.error('Error deleting category:', error)
          alert('Kategori silinirken bir hata oluştu.')
        }
      }
    },
    
    closeModal() {
      this.showAddModal = false
      this.editingCategory = null
      this.categoryForm = {
        name: '',
        description: '',
        isActive: true
      }
    },
    
    formatDate(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleDateString('tr-TR')
    }
  }
}
</script>

<style scoped>
.modal.show {
  background-color: rgba(0,0,0,0.5);
}
</style>
