<template>
  <div class="order-tracking-view">
    <div class="container py-5">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="text-center mb-5">
            <i class="bi bi-search display-4 text-primary mb-3"></i>
            <h2 class="fw-bold">Sipariş Takibi</h2>
            <p class="text-muted">Sipariş numaranız ve e-posta adresinizle siparişinizi sorgulayabilirsiniz</p>
          </div>

          <!-- Search Form -->
          <div class="card shadow-sm border-0 mb-4">
            <div class="card-body p-4">
              <form @submit.prevent="searchOrder">
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label class="form-label fw-bold">Sipariş Numarası</label>
                    <div class="input-group">
                      <span class="input-group-text">
                        <i class="bi bi-receipt"></i>
                      </span>
                      <input 
                        v-model="searchForm.orderNumber" 
                        type="text" 
                        class="form-control" 
                        placeholder="Örn: ORD-20240101-001"
                        required
                      >
                    </div>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label fw-bold">E-posta Adresi</label>
                    <div class="input-group">
                      <span class="input-group-text">
                        <i class="bi bi-envelope"></i>
                      </span>
                      <input 
                        v-model="searchForm.email" 
                        type="email" 
                        class="form-control" 
                        placeholder="ornek@email.com"
                        required
                      >
                    </div>
                  </div>
                </div>
                <div class="text-center">
                  <button 
                    type="submit" 
                    class="btn btn-primary btn-lg"
                    :disabled="loading"
                  >
                    <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                    <i v-else class="bi bi-search me-2"></i>
                    Sipariş Sorgula
                  </button>
                </div>
              </form>
            </div>
          </div>

          <!-- Error Message -->
          <div v-if="error" class="alert alert-danger">
            <i class="bi bi-exclamation-triangle me-2"></i>
            {{ error }}
          </div>

          <!-- Order Details -->
          <div v-if="order" class="card shadow-sm border-0">
            <div class="card-header bg-primary text-white">
              <div class="row align-items-center">
                <div class="col">
                  <h5 class="mb-0">
                    <i class="bi bi-receipt me-2"></i>
                    Sipariş Detayları
                  </h5>
                </div>
                <div class="col-auto">
                  <span :class="`badge ${getStatusBadgeClass(order.status)}`">
                    {{ getStatusText(order.status) }}
                  </span>
                </div>
              </div>
            </div>
            <div class="card-body">
              <!-- Order Info -->
              <div class="row mb-4">
                <div class="col-md-6">
                  <h6 class="fw-bold text-muted">SİPARİŞ BİLGİLERİ</h6>
                  <p class="mb-1"><strong>Sipariş No:</strong> {{ order.orderNumber }}</p>
                  <p class="mb-1"><strong>Tarih:</strong> {{ formatDate(order.orderDate) }}</p>
                  <p class="mb-1"><strong>Toplam:</strong> {{ order.totalAmount }}₺</p>
                </div>
                <div class="col-md-6">
                  <h6 class="fw-bold text-muted">MÜŞTERİ BİLGİLERİ</h6>
                  <p class="mb-1"><strong>Ad Soyad:</strong> {{ order.customer.firstName }} {{ order.customer.lastName }}</p>
                  <p class="mb-1"><strong>E-posta:</strong> {{ order.customer.email }}</p>
                  <p class="mb-1"><strong>Telefon:</strong> {{ order.customer.phone }}</p>
                </div>
              </div>

              <!-- Delivery Info -->
              <div class="row mb-4">
                <div class="col-12">
                  <h6 class="fw-bold text-muted">TESLİMAT BİLGİLERİ</h6>
                  <p class="mb-1"><strong>Adres:</strong> {{ order.deliveryAddress }}, {{ order.deliveryCity }} {{ order.deliveryPostalCode }}</p>
                  <p class="mb-1"><strong>Teslimat Tarihi:</strong> {{ order.deliveryDate ? formatDate(order.deliveryDate) : 'Belirtilmemiş' }}</p>
                  <p v-if="order.deliveryTime" class="mb-1"><strong>Teslimat Saati:</strong> {{ order.deliveryTime }}</p>
                  <p v-if="order.notes" class="mb-1"><strong>Notlar:</strong> {{ order.notes }}</p>
                </div>
              </div>

              <!-- Order Items -->
              <div class="row">
                <div class="col-12">
                  <h6 class="fw-bold text-muted">SİPARİŞ İÇERİĞİ</h6>
                  <div class="table-responsive">
                    <table class="table table-sm">
                      <thead>
                        <tr>
                          <th>Ürün</th>
                          <th>Adet</th>
                          <th>Birim Fiyat</th>
                          <th>Toplam</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="item in order.items" :key="item.id">
                          <td>
                            <div class="d-flex align-items-center">
                              <img 
                                :src="item.flowerImage" 
                                :alt="item.flowerName"
                                class="me-2"
                                style="width: 40px; height: 40px; object-fit: cover; border-radius: 4px;"
                                @error="$event.target.src = '/images/placeholder-flower.jpg'"
                              >
                              {{ item.flowerName }}
                            </div>
                          </td>
                          <td>{{ item.quantity }}</td>
                          <td>{{ item.unitPrice }}₺</td>
                          <td><strong>{{ item.total }}₺</strong></td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ordersApi } from '../services/api'

export default {
  name: 'OrderTrackingView',
  data() {
    return {
      searchForm: {
        orderNumber: '',
        email: ''
      },
      order: null,
      loading: false,
      error: null
    }
  },
  methods: {
    async searchOrder() {
      try {
        this.loading = true
        this.error = null
        this.order = null
        
        const response = await ordersApi.search(this.searchForm.orderNumber, this.searchForm.email)
        this.order = response.data
        
      } catch (error) {
        console.error('Order search error:', error)
        this.error = error.response?.data?.message || 'Sipariş sorgulanırken bir hata oluştu.'
      } finally {
        this.loading = false
      }
    },
    
    getStatusText(status) {
      const statusMap = {
        0: 'Beklemede',
        1: 'Onaylandı',
        2: 'Hazırlanıyor',
        3: 'Kargoda',
        4: 'Teslim Edildi',
        5: 'İptal Edildi'
      }
      return statusMap[status] || 'Bilinmiyor'
    },
    
    getStatusBadgeClass(status) {
      const classMap = {
        0: 'bg-warning',
        1: 'bg-info',
        2: 'bg-primary',
        3: 'bg-secondary',
        4: 'bg-success',
        5: 'bg-danger'
      }
      return classMap[status] || 'bg-secondary'
    },
    
    formatDate(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleDateString('tr-TR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    }
  }
}
</script>

<style scoped>
.input-group-text {
  background-color: #f8f9fa;
  border-right: none;
}

.form-control {
  border-left: none;
}

.form-control:focus {
  border-color: #667eea;
  box-shadow: 0 0 0 0.2rem rgba(102, 126, 234, 0.25);
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
}

.btn-primary:hover {
  background: linear-gradient(135deg, #5a6fd8 0%, #6a4190 100%);
  transform: translateY(-1px);
}

.card-header.bg-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%) !important;
}
</style>
