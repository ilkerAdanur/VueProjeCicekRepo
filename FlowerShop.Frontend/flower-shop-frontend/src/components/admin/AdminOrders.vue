<template>
  <div class="admin-orders">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h5 class="mb-0">Sipariş Yönetimi</h5>
      <div class="d-flex gap-2">
        <select v-model="statusFilter" class="form-select" @change="loadOrders">
          <option value="">Tüm Durumlar</option>
          <option value="0">Beklemede</option>
          <option value="1">Onaylandı</option>
          <option value="2">Hazırlanıyor</option>
          <option value="3">Kargoda</option>
          <option value="4">Teslim Edildi</option>
          <option value="5">İptal Edildi</option>
        </select>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Yükleniyor...</span>
      </div>
    </div>

    <!-- Orders Table -->
    <div v-else class="card">
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Sipariş No</th>
                <th>Müşteri</th>
                <th>Tarih</th>
                <th>Tutar</th>
                <th>Durum</th>
                <th>Teslimat</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="order in orders" :key="order.id">
                <td>
                  <strong>{{ order.orderNumber }}</strong>
                </td>
                <td>
                  {{ order.customer.firstName }} {{ order.customer.lastName }}
                  <br>
                  <small class="text-muted">{{ order.customer.email }}</small>
                </td>
                <td>{{ formatDate(order.orderDate) }}</td>
                <td><strong>{{ order.totalAmount }}₺</strong></td>
                <td>
                  <span :class="`badge ${getStatusBadgeClass(order.status)}`">
                    {{ getStatusText(order.status) }}
                  </span>
                </td>
                <td>
                  {{ order.deliveryDate ? formatDate(order.deliveryDate) : 'Belirtilmemiş' }}
                  <span v-if="order.deliveryTime" class="badge bg-info ms-1">{{ order.deliveryTime }}</span>
                  <br>
                  <small class="text-muted">{{ order.deliveryCity }}</small>
                </td>
                <td>
                  <div class="btn-group btn-group-sm">
                    <button class="btn btn-outline-info" @click="viewOrder(order)">
                      <i class="bi bi-eye"></i>
                    </button>
                    <div class="dropdown">
                      <button class="btn btn-outline-primary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                        <i class="bi bi-gear"></i>
                      </button>
                      <ul class="dropdown-menu">
                        <li v-if="order.status === 0">
                          <a class="dropdown-item" href="#" @click.prevent="updateOrderStatus(order.id, 1)">
                            <i class="bi bi-check-circle me-2"></i>Onayla
                          </a>
                        </li>
                        <li v-if="order.status === 1">
                          <a class="dropdown-item" href="#" @click.prevent="updateOrderStatus(order.id, 2)">
                            <i class="bi bi-clock me-2"></i>Hazırlanıyor
                          </a>
                        </li>
                        <li v-if="order.status === 2">
                          <a class="dropdown-item" href="#" @click.prevent="updateOrderStatus(order.id, 3)">
                            <i class="bi bi-truck me-2"></i>Kargoya Ver
                          </a>
                        </li>
                        <li v-if="order.status === 3">
                          <a class="dropdown-item" href="#" @click.prevent="updateOrderStatus(order.id, 4)">
                            <i class="bi bi-check2-all me-2"></i>Teslim Edildi
                          </a>
                        </li>
                        <li v-if="order.status < 4">
                          <hr class="dropdown-divider">
                          <a class="dropdown-item text-danger" href="#" @click.prevent="updateOrderStatus(order.id, 5)">
                            <i class="bi bi-x-circle me-2"></i>İptal Et
                          </a>
                        </li>
                      </ul>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Order Detail Modal -->
    <div class="modal fade" :class="{ show: showDetailModal }" :style="{ display: showDetailModal ? 'block' : 'none' }" tabindex="-1">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Sipariş Detayları</h5>
            <button type="button" class="btn-close" @click="showDetailModal = false"></button>
          </div>
          <div class="modal-body" v-if="selectedOrder">
            <div class="row mb-3">
              <div class="col-md-6">
                <h6 class="fw-bold">Sipariş Bilgileri</h6>
                <p class="mb-1"><strong>No:</strong> {{ selectedOrder.orderNumber }}</p>
                <p class="mb-1"><strong>Tarih:</strong> {{ formatDate(selectedOrder.orderDate) }}</p>
                <p class="mb-1"><strong>Toplam:</strong> {{ selectedOrder.totalAmount }}₺</p>
                <p class="mb-1"><strong>Durum:</strong> 
                  <span :class="`badge ${getStatusBadgeClass(selectedOrder.status)}`">
                    {{ getStatusText(selectedOrder.status) }}
                  </span>
                </p>
              </div>
              <div class="col-md-6">
                <h6 class="fw-bold">Müşteri Bilgileri</h6>
                <p class="mb-1"><strong>Ad:</strong> {{ selectedOrder.customer.firstName }} {{ selectedOrder.customer.lastName }}</p>
                <p class="mb-1"><strong>Email:</strong> {{ selectedOrder.customer.email }}</p>
                <p class="mb-1"><strong>Telefon:</strong> {{ selectedOrder.customer.phone }}</p>
              </div>
            </div>
            
            <div class="row mb-3">
              <div class="col-12">
                <h6 class="fw-bold">Teslimat Bilgileri</h6>
                <p class="mb-1"><strong>Adres:</strong> {{ selectedOrder.deliveryAddress }}</p>
                <p class="mb-1"><strong>Şehir:</strong> {{ selectedOrder.deliveryCity }} {{ selectedOrder.deliveryPostalCode }}</p>
                <p class="mb-1"><strong>Tarih:</strong> {{ selectedOrder.deliveryDate ? formatDate(selectedOrder.deliveryDate) : 'Belirtilmemiş' }}</p>
                <p v-if="selectedOrder.deliveryTime" class="mb-1"><strong>Saat:</strong> {{ selectedOrder.deliveryTime }}</p>
                <p v-if="selectedOrder.notes" class="mb-1"><strong>Notlar:</strong> {{ selectedOrder.notes }}</p>
              </div>
            </div>

            <div class="row">
              <div class="col-12">
                <h6 class="fw-bold">Sipariş İçeriği</h6>
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
                        <td>{{ item.flowerName }}</td>
                        <td>{{ item.quantity }}</td>
                        <td>{{ item.unitPrice }}₺</td>
                        <td><strong>{{ (item.quantity * item.unitPrice).toFixed(2) }}₺</strong></td>
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
    <div v-if="showDetailModal" class="modal-backdrop fade show"></div>
  </div>
</template>

<script>
import { ordersApi } from '../../services/api'

export default {
  name: 'AdminOrders',
  data() {
    return {
      orders: [],
      loading: false,
      statusFilter: '',
      showDetailModal: false,
      selectedOrder: null
    }
  },
  async mounted() {
    await this.loadOrders()
  },
  methods: {
    async loadOrders() {
      try {
        this.loading = true
        const response = await ordersApi.getAll()
        this.orders = response.data
      } catch (error) {
        console.error('Error loading orders:', error)
        this.orders = []
      } finally {
        this.loading = false
      }
    },
    
    viewOrder(order) {
      this.selectedOrder = order
      this.showDetailModal = true
    },
    
    async updateOrderStatus(orderId, newStatus) {
      try {
        await ordersApi.updateStatus(orderId, newStatus)
        await this.loadOrders()
      } catch (error) {
        console.error('Error updating order status:', error)
        alert('Sipariş durumu güncellenirken bir hata oluştu.')
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
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    }
  }
}
</script>

<style scoped>
.modal.show {
  background-color: rgba(0,0,0,0.5);
}
</style>
