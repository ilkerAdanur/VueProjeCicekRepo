<template>
  <div class="admin-view">
    <!-- Admin Header -->
    <div class="admin-header bg-dark text-white py-3">
      <div class="container-fluid">
        <div class="row align-items-center">
          <div class="col">
            <h4 class="mb-0">
              <i class="bi bi-gear me-2"></i>
              Admin Panel
            </h4>
          </div>
          <div class="col-auto">
            <span class="badge bg-primary">{{ user?.firstName }} {{ user?.lastName }}</span>
          </div>
        </div>
      </div>
    </div>

    <div class="container-fluid py-4">
      <div class="row">
        <!-- Sidebar -->
        <div class="col-lg-3 col-md-4">
          <div class="card shadow-sm border-0">
            <div class="card-body p-0">
              <div class="list-group list-group-flush">
                <button 
                  @click="activeTab = 'dashboard'"
                  :class="`list-group-item list-group-item-action ${activeTab === 'dashboard' ? 'active' : ''}`"
                >
                  <i class="bi bi-speedometer2 me-2"></i>
                  Dashboard
                </button>
                <button 
                  @click="activeTab = 'flowers'"
                  :class="`list-group-item list-group-item-action ${activeTab === 'flowers' ? 'active' : ''}`"
                >
                  <i class="bi bi-flower1 me-2"></i>
                  Çiçek Yönetimi
                </button>
                <button 
                  @click="activeTab = 'categories'"
                  :class="`list-group-item list-group-item-action ${activeTab === 'categories' ? 'active' : ''}`"
                >
                  <i class="bi bi-tags me-2"></i>
                  Kategori Yönetimi
                </button>
                <button 
                  @click="activeTab = 'orders'"
                  :class="`list-group-item list-group-item-action ${activeTab === 'orders' ? 'active' : ''}`"
                >
                  <i class="bi bi-receipt me-2"></i>
                  Sipariş Yönetimi
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Main Content -->
        <div class="col-lg-9 col-md-8">
          <!-- Dashboard -->
          <div v-if="activeTab === 'dashboard'" class="dashboard-content">
            <div class="row g-4">
              <div class="col-lg-3 col-md-6">
                <div class="card bg-primary text-white">
                  <div class="card-body">
                    <div class="d-flex justify-content-between">
                      <div>
                        <h6 class="card-title">Toplam Çiçek</h6>
                        <h3 class="mb-0">{{ stats.totalFlowers }}</h3>
                      </div>
                      <i class="bi bi-flower1 fs-1 opacity-50"></i>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-lg-3 col-md-6">
                <div class="card bg-success text-white">
                  <div class="card-body">
                    <div class="d-flex justify-content-between">
                      <div>
                        <h6 class="card-title">Toplam Sipariş</h6>
                        <h3 class="mb-0">{{ stats.totalOrders }}</h3>
                      </div>
                      <i class="bi bi-receipt fs-1 opacity-50"></i>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-lg-3 col-md-6">
                <div class="card bg-warning text-white">
                  <div class="card-body">
                    <div class="d-flex justify-content-between">
                      <div>
                        <h6 class="card-title">Bekleyen Sipariş</h6>
                        <h3 class="mb-0">{{ stats.pendingOrders }}</h3>
                      </div>
                      <i class="bi bi-clock fs-1 opacity-50"></i>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-lg-3 col-md-6">
                <div class="card bg-info text-white">
                  <div class="card-body">
                    <div class="d-flex justify-content-between">
                      <div>
                        <h6 class="card-title">Kategoriler</h6>
                        <h3 class="mb-0">{{ stats.totalCategories }}</h3>
                      </div>
                      <i class="bi bi-tags fs-1 opacity-50"></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Flowers Management -->
          <AdminFlowers v-else-if="activeTab === 'flowers'" />

          <!-- Categories Management -->
          <AdminCategories v-else-if="activeTab === 'categories'" />

          <!-- Orders Management -->
          <AdminOrders v-else-if="activeTab === 'orders'" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AdminFlowers from '../components/admin/AdminFlowers.vue'
import AdminCategories from '../components/admin/AdminCategories.vue'
import AdminOrders from '../components/admin/AdminOrders.vue'
import { dashboardApi } from '../services/api'

export default {
  name: 'AdminView',
  components: {
    AdminFlowers,
    AdminCategories,
    AdminOrders
  },
  data() {
    return {
      activeTab: 'dashboard',
      user: null,
      stats: {
        totalFlowers: 0,
        totalOrders: 0,
        pendingOrders: 0,
        totalCategories: 0
      }
    }
  },
  async mounted() {
    // Check if user is admin
    const savedUser = localStorage.getItem('user')
    if (savedUser) {
      this.user = JSON.parse(savedUser)
      if (this.user.role !== 'Admin') {
        this.$router.push('/')
        return
      }
    } else {
      this.$router.push('/login')
      return
    }

    await this.loadStats()
  },
  methods: {
    async loadStats() {
      try {
        const response = await dashboardApi.getStats()
        this.stats = response.data
      } catch (error) {
        console.error('Error loading stats:', error)
      }
    }
  }
}
</script>

<style scoped>
.admin-header {
  background: linear-gradient(135deg, #343a40 0%, #495057 100%) !important;
}

.list-group-item.active {
  background-color: #667eea;
  border-color: #667eea;
}

.list-group-item:hover {
  background-color: #f8f9fa;
}

.card {
  transition: all 0.3s ease;
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
}
</style>
