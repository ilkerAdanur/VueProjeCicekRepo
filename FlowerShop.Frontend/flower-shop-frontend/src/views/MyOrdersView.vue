<template>
  <div class="container mt-4">
    <div class="row">
      <div class="col-12">
        <div class="d-flex justify-content-between align-items-center mb-4">
          <h2>Siparişlerim</h2>
          <router-link to="/" class="btn btn-outline-primary">
            <i class="bi bi-house me-1"></i>
            Ana Sayfa
          </router-link>
        </div>

        <!-- Login Required Message -->
        <div class="card mb-4" v-if="!userEmail">
          <div class="card-body text-center">
            <i class="bi bi-person-lock display-1 text-muted"></i>
            <h5 class="card-title mt-3">Siparişlerinizi Görüntülemek İçin Giriş Yapın</h5>
            <p class="text-muted">Siparişlerinizi görüntülemek için hesabınıza giriş yapmanız gerekmektedir.</p>
            <router-link to="/login" class="btn btn-primary">
              <i class="bi bi-box-arrow-in-right me-1"></i>
              Giriş Yap
            </router-link>
          </div>
        </div>

        <!-- Loading -->
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
          <p class="mt-2">Siparişleriniz yükleniyor...</p>
        </div>

        <!-- Error Message -->
        <div v-if="error" class="alert alert-danger">
          {{ error }}
        </div>

        <!-- Orders List -->
        <div v-if="!loading && orders.length > 0">
          <div class="row">
            <div class="col-12 mb-3">
              <p class="text-muted">
                <strong>{{ userEmail }}</strong> adresine kayıtlı {{ orders.length }} sipariş bulundu.
              </p>
            </div>
          </div>

          <div class="row">
            <div class="col-lg-4 col-md-6 mb-4" v-for="order in orders" :key="order.id">
              <div class="card h-100">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <h6 class="mb-0">{{ order.orderNumber }}</h6>
                  <span :class="getStatusClass(order.status)">
                    {{ getStatusText(order.status) }}
                  </span>
                </div>
                <div class="card-body">
                  <p class="card-text">
                    <small class="text-muted">
                      <i class="bi bi-calendar me-1"></i>
                      {{ formatDate(order.orderDate) }}
                    </small>
                  </p>
                  
                  <div class="mb-2">
                    <strong>Toplam: {{ order.totalAmount }}₺</strong>
                  </div>
                  
                  <div class="mb-2">
                    <small class="text-muted">{{ order.itemCount }} ürün</small>
                  </div>

                  <div v-if="order.deliveryDate" class="mb-2">
                    <small class="text-muted">
                      <i class="bi bi-truck me-1"></i>
                      {{ formatDate(order.deliveryDate) }}
                      <span v-if="order.deliveryTime"> - {{ order.deliveryTime }}</span>
                    </small>
                  </div>

                  <div class="mb-3">
                    <small class="text-muted">
                      <i class="bi bi-geo-alt me-1"></i>
                      {{ order.deliveryAddress }}, {{ order.deliveryCity }}
                    </small>
                  </div>

                  <!-- Order Items Preview -->
                  <div class="mb-3">
                    <div class="row g-2">
                      <div 
                        class="col-4" 
                        v-for="(item, index) in order.items.slice(0, 3)" 
                        :key="item.id"
                      >
                        <div class="position-relative">
                          <img 
                            :src="item.flowerImage || '/placeholder-flower.jpg'" 
                            :alt="item.flowerName"
                            class="img-fluid rounded"
                            style="height: 60px; width: 100%; object-fit: cover;"
                          >
                          <span class="position-absolute top-0 end-0 badge bg-primary rounded-pill">
                            {{ item.quantity }}
                          </span>
                        </div>
                      </div>
                      <div v-if="order.items.length > 3" class="col-4 d-flex align-items-center justify-content-center">
                        <small class="text-muted">+{{ order.items.length - 3 }} daha</small>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="card-footer">
                  <button 
                    class="btn btn-outline-primary btn-sm w-100"
                    @click="viewOrderDetails(order)"
                  >
                    <i class="bi bi-eye me-1"></i>
                    Detayları Gör
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- No Orders -->
        <div v-if="!loading && orders.length === 0 && userEmail" class="text-center py-5">
          <i class="bi bi-inbox display-1 text-muted"></i>
          <h4 class="mt-3">Henüz siparişiniz bulunmuyor</h4>
          <p class="text-muted">İlk siparişinizi vermek için çiçeklerimize göz atın.</p>
          <router-link to="/flowers" class="btn btn-primary">
            <i class="bi bi-flower1 me-1"></i>
            Çiçekleri İncele
          </router-link>
        </div>
      </div>
    </div>

    <!-- Order Details Modal -->
    <div class="modal fade" id="orderDetailsModal" tabindex="-1">
      <div class="modal-dialog modal-lg">
        <div class="modal-content" v-if="selectedOrder">
          <div class="modal-header">
            <h5 class="modal-title">Sipariş Detayları - {{ selectedOrder.orderNumber }}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <div class="row mb-3">
              <div class="col-md-6">
                <strong>Sipariş Tarihi:</strong><br>
                {{ formatDate(selectedOrder.orderDate) }}
              </div>
              <div class="col-md-6">
                <strong>Durum:</strong><br>
                <span :class="getStatusClass(selectedOrder.status)">
                  {{ getStatusText(selectedOrder.status) }}
                </span>
              </div>
            </div>

            <div class="row mb-3" v-if="selectedOrder.deliveryDate">
              <div class="col-md-6">
                <strong>Teslimat Tarihi:</strong><br>
                {{ formatDate(selectedOrder.deliveryDate) }}
              </div>
              <div class="col-md-6" v-if="selectedOrder.deliveryTime">
                <strong>Teslimat Saati:</strong><br>
                {{ selectedOrder.deliveryTime }}
              </div>
            </div>

            <div class="mb-3">
              <strong>Teslimat Adresi:</strong><br>
              {{ selectedOrder.deliveryAddress }}<br>
              {{ selectedOrder.deliveryCity }}
            </div>

            <div class="mb-3">
              <strong>Sipariş İçeriği:</strong>
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
                    <tr v-for="item in selectedOrder.items" :key="item.id">
                      <td>
                        <div class="d-flex align-items-center">
                          <img 
                            :src="item.flowerImage || '/placeholder-flower.jpg'" 
                            :alt="item.flowerName"
                            class="me-2 rounded"
                            style="width: 40px; height: 40px; object-fit: cover;"
                          >
                          {{ item.flowerName }}
                        </div>
                      </td>
                      <td>{{ item.quantity }}</td>
                      <td>{{ item.unitPrice }}₺</td>
                      <td><strong>{{ item.total }}₺</strong></td>
                    </tr>
                  </tbody>
                  <tfoot>
                    <tr>
                      <th colspan="3">Genel Toplam</th>
                      <th>{{ selectedOrder.totalAmount }}₺</th>
                    </tr>
                  </tfoot>
                </table>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Kapat</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Modal } from 'bootstrap'

export default {
  name: 'MyOrdersView',
  data() {
    return {
      userEmail: '',
      orders: [],
      selectedOrder: null,
      loading: false,
      error: null
    }
  },
  mounted() {
    // Check if user is logged in
    const user = JSON.parse(localStorage.getItem('flowerShopUser') || 'null')
    if (user && user.email) {
      this.userEmail = user.email
      this.loadOrders()
    }
  },
  methods: {
    async loadOrders() {
      if (!this.userEmail.trim()) {
        this.error = 'Giriş yapmanız gereklidir.'
        return
      }

      this.loading = true
      this.error = null

      try {
        const response = await fetch(`http://localhost:5133/api/Orders/customer/${encodeURIComponent(this.userEmail)}`)

        if (response.ok) {
          this.orders = await response.json()
        } else {
          this.error = 'Siparişler yüklenirken bir hata oluştu.'
        }
      } catch (error) {
        console.error('Error loading orders:', error)
        this.error = 'Sunucuya bağlanırken bir hata oluştu.'
      } finally {
        this.loading = false
      }
    },
    viewOrderDetails(order) {
      this.selectedOrder = order
      const modal = new Modal(document.getElementById('orderDetailsModal'))
      modal.show()
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
    getStatusClass(status) {
      const classMap = {
        0: 'badge bg-warning',
        1: 'badge bg-info',
        2: 'badge bg-primary', 
        3: 'badge bg-secondary',
        4: 'badge bg-success',
        5: 'badge bg-danger'
      }
      return classMap[status] || 'badge bg-secondary'
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
.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-2px);
}

.badge {
  font-size: 0.75em;
}
</style>
