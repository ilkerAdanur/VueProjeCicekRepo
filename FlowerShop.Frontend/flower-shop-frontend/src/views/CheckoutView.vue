<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ordersApi } from '../services/api.js'

const router = useRouter()
const cartItems = ref([])
const loading = ref(false)
const orderPlaced = ref(false)
const orderNumber = ref(null)
const showQuickRegister = ref(false)
const quickEmail = ref('')

// Form data
const customerForm = ref({
  firstName: '',
  lastName: '',
  email: '',
  phone: '',
  address: '',
  city: '',
  postalCode: ''
})

// Load saved customer info
const loadCustomerInfo = () => {
  const savedCustomer = localStorage.getItem('flowerShopCustomer')
  if (savedCustomer) {
    const customer = JSON.parse(savedCustomer)
    customerForm.value = { ...customerForm.value, ...customer }
  }
}

// Save customer info
const saveCustomerInfo = () => {
  localStorage.setItem('flowerShopCustomer', JSON.stringify(customerForm.value))
}

// Quick register with email only
const quickRegister = () => {
  if (quickEmail.value) {
    customerForm.value.email = quickEmail.value
    showQuickRegister.value = false
    quickEmail.value = ''
  }
}

const deliveryForm = ref({
  deliveryAddress: '',
  deliveryCity: '',
  deliveryPostalCode: '',
  deliveryDate: '',
  deliveryTime: '',
  notes: ''
})

const sameAsCustomer = ref(true)

// Computed property for minimum delivery date
const minDeliveryDate = computed(() => {
  const today = new Date()
  return today.toISOString().split('T')[0]
})

onMounted(() => {
  loadCart()
  loadCustomerInfo()
  if (cartItems.value.length === 0) {
    router.push('/cart')
  }
  
  // Set default delivery date to today
  const today = new Date()
  deliveryForm.value.deliveryDate = today.toISOString().split('T')[0]
})

const loadCart = () => {
  const user = JSON.parse(localStorage.getItem('user') || 'null')
  const cartKey = user ? `flowerShopCart_${user.id}` : 'flowerShopCart'
  const savedCart = localStorage.getItem(cartKey)
  if (savedCart) {
    cartItems.value = JSON.parse(savedCart)
  }
}

const totalAmount = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + (item.price * item.quantity), 0)
})

const totalItems = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + item.quantity, 0)
})

const copyCustomerAddress = () => {
  if (sameAsCustomer.value) {
    deliveryForm.value.deliveryAddress = customerForm.value.address
    deliveryForm.value.deliveryCity = customerForm.value.city
    deliveryForm.value.deliveryPostalCode = customerForm.value.postalCode
  } else {
    deliveryForm.value.deliveryAddress = ''
    deliveryForm.value.deliveryCity = ''
    deliveryForm.value.deliveryPostalCode = ''
  }
}

const submitOrder = async () => {
  loading.value = true
  
  try {
    const orderData = {
      customer: customerForm.value,
      items: cartItems.value.map(item => ({
        flowerId: item.id,
        quantity: item.quantity
      })),
      deliveryAddress: sameAsCustomer.value ? customerForm.value.address : deliveryForm.value.deliveryAddress,
      deliveryCity: sameAsCustomer.value ? customerForm.value.city : deliveryForm.value.deliveryCity,
      deliveryPostalCode: sameAsCustomer.value ? customerForm.value.postalCode : deliveryForm.value.deliveryPostalCode,
      deliveryDate: deliveryForm.value.deliveryDate ? new Date(deliveryForm.value.deliveryDate).toISOString() : null,
      deliveryTime: deliveryForm.value.deliveryTime,
      notes: deliveryForm.value.notes
    }
    
    const response = await ordersApi.create(orderData)
    orderNumber.value = response.data.orderNumber
    orderPlaced.value = true

    // Save customer info for future orders
    saveCustomerInfo()

    // Clear cart
    const user = JSON.parse(localStorage.getItem('user') || 'null')
    const cartKey = user ? `flowerShopCart_${user.id}` : 'flowerShopCart'
    localStorage.removeItem(cartKey)
    window.dispatchEvent(new Event('cartUpdated'))
    
  } catch (error) {
    console.error('Error placing order:', error)
    alert('Sipariş verilirken bir hata oluştu. Lütfen tekrar deneyin.')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="container py-4">
    <!-- Order Success -->
    <div v-if="orderPlaced" class="text-center py-5">
      <i class="bi bi-check-circle-fill text-success" style="font-size: 4rem;"></i>
      <h2 class="text-success mt-3">Siparişiniz Alındı!</h2>
      <p class="lead">Sipariş numaranız: <strong>{{ orderNumber }}</strong></p>
      <p>Siparişiniz en kısa sürede hazırlanacak ve size ulaştırılacaktır.</p>
      <div class="mt-4">
        <router-link to="/" class="btn btn-success me-2">Ana Sayfaya Dön</router-link>
        <router-link to="/flowers" class="btn btn-outline-success">Alışverişe Devam Et</router-link>
      </div>
    </div>
    
    <!-- Checkout Form -->
    <div v-else>
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="mb-0">Sipariş Tamamla</h2>
        <button
          v-if="!customerForm.email"
          type="button"
          class="btn btn-outline-primary"
          @click="showQuickRegister = true"
        >
          <i class="bi bi-lightning me-2"></i>Hızlı Kayıt
        </button>
      </div>

      <form @submit.prevent="submitOrder">
        <div class="row">
          <!-- Customer Information -->
          <div class="col-lg-8">
            <div class="card mb-4">
              <div class="card-header">
                <h5 class="mb-0">Müşteri Bilgileri</h5>
              </div>
              <div class="card-body">
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Ad *</label>
                    <input v-model="customerForm.firstName" type="text" class="form-control" required>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Soyad *</label>
                    <input v-model="customerForm.lastName" type="text" class="form-control" required>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">E-posta *</label>
                    <input v-model="customerForm.email" type="email" class="form-control" required>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Telefon</label>
                    <input v-model="customerForm.phone" type="tel" class="form-control">
                  </div>
                  <div class="col-12 mb-3">
                    <label class="form-label">Adres *</label>
                    <textarea v-model="customerForm.address" class="form-control" rows="3" required></textarea>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Şehir *</label>
                    <input v-model="customerForm.city" type="text" class="form-control" required>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Posta Kodu</label>
                    <input v-model="customerForm.postalCode" type="text" class="form-control">
                  </div>
                </div>
              </div>
            </div>
            
            <!-- Delivery Information -->
            <div class="card mb-4">
              <div class="card-header">
                <h5 class="mb-0">Teslimat Bilgileri</h5>
              </div>
              <div class="card-body">
                <div class="form-check mb-3">
                  <input v-model="sameAsCustomer" @change="copyCustomerAddress" 
                         class="form-check-input" type="checkbox" id="sameAsCustomer">
                  <label class="form-check-label" for="sameAsCustomer">
                    Teslimat adresi müşteri adresi ile aynı
                  </label>
                </div>
                
                <div v-if="!sameAsCustomer">
                  <div class="row">
                    <div class="col-12 mb-3">
                      <label class="form-label">Teslimat Adresi *</label>
                      <textarea v-model="deliveryForm.deliveryAddress" class="form-control" rows="3" required></textarea>
                    </div>
                    <div class="col-md-6 mb-3">
                      <label class="form-label">Şehir *</label>
                      <input v-model="deliveryForm.deliveryCity" type="text" class="form-control" required>
                    </div>
                    <div class="col-md-6 mb-3">
                      <label class="form-label">Posta Kodu</label>
                      <input v-model="deliveryForm.deliveryPostalCode" type="text" class="form-control">
                    </div>
                  </div>
                </div>
                
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Teslimat Tarihi</label>
                    <input v-model="deliveryForm.deliveryDate" type="date" class="form-control" :min="minDeliveryDate">
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Teslimat Saati</label>
                    <select v-model="deliveryForm.deliveryTime" class="form-select">
                      <option value="">Saat seçiniz</option>
                      <option value="09:00-12:00">09:00 - 12:00</option>
                      <option value="12:00-15:00">12:00 - 15:00</option>
                      <option value="15:00-18:00">15:00 - 18:00</option>
                      <option value="18:00-21:00">18:00 - 21:00</option>
                    </select>
                  </div>
                  <div class="col-12 mb-3">
                    <label class="form-label">Notlar</label>
                    <textarea v-model="deliveryForm.notes" class="form-control" rows="3" 
                              placeholder="Özel talimatlarınız varsa buraya yazabilirsiniz..."></textarea>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <!-- Order Summary -->
          <div class="col-lg-4">
            <div class="card">
              <div class="card-header">
                <h5 class="mb-0">Sipariş Özeti</h5>
              </div>
              <div class="card-body">
                <!-- Cart Items -->
                <div class="mb-3">
                  <div v-for="item in cartItems" :key="item.id" class="d-flex justify-content-between align-items-center mb-2">
                    <div>
                      <small class="fw-bold">{{ item.name }}</small><br>
                      <small class="text-muted">{{ item.quantity }} x {{ item.price }}₺</small>
                    </div>
                    <span class="fw-bold">{{ (item.price * item.quantity).toFixed(2) }}₺</span>
                  </div>
                </div>
                
                <hr>
                
                <!-- Totals -->
                <div class="d-flex justify-content-between mb-2">
                  <span>Ürün Toplamı ({{ totalItems }} adet):</span>
                  <span>{{ totalAmount.toFixed(2) }}₺</span>
                </div>
                <div class="d-flex justify-content-between mb-2">
                  <span>Kargo:</span>
                  <span class="text-success">Ücretsiz</span>
                </div>
                <hr>
                <div class="d-flex justify-content-between mb-3">
                  <strong>Toplam:</strong>
                  <strong class="text-success">{{ totalAmount.toFixed(2) }}₺</strong>
                </div>
                
                <!-- Submit Button -->
                <button type="submit" :disabled="loading" class="btn btn-success w-100">
                  <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                  <i v-else class="bi bi-credit-card me-2"></i>
                  {{ loading ? 'Sipariş Veriliyor...' : 'Siparişi Tamamla' }}
                </button>
                
                <router-link to="/cart" class="btn btn-outline-secondary w-100 mt-2">
                  <i class="bi bi-arrow-left"></i> Sepete Dön
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </form>
    </div>

    <!-- Quick Register Modal -->
    <div class="modal fade" :class="{ show: showQuickRegister }" :style="{ display: showQuickRegister ? 'block' : 'none' }" tabindex="-1">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="bi bi-lightning me-2"></i>Hızlı Kayıt
            </h5>
            <button type="button" class="btn-close" @click="showQuickRegister = false"></button>
          </div>
          <div class="modal-body">
            <p class="text-muted mb-3">
              Sadece e-posta adresinizi girin, diğer bilgileri sipariş sırasında tamamlayabilirsiniz.
            </p>
            <form @submit.prevent="quickRegister">
              <div class="mb-3">
                <label class="form-label">E-posta Adresi</label>
                <input
                  v-model="quickEmail"
                  type="email"
                  class="form-control"
                  placeholder="ornek@email.com"
                  required
                >
              </div>
              <div class="d-flex gap-2">
                <button type="submit" class="btn btn-primary flex-fill">
                  <i class="bi bi-check me-2"></i>Devam Et
                </button>
                <button type="button" class="btn btn-outline-secondary" @click="showQuickRegister = false">
                  İptal
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <div v-if="showQuickRegister" class="modal-backdrop fade show"></div>
  </div>
</template>

<style scoped>
.form-control:focus {
  border-color: #28a745;
  box-shadow: 0 0 0 0.2rem rgba(40, 167, 69, 0.25);
}

.modal.show {
  background-color: rgba(0,0,0,0.5);
}
</style>
