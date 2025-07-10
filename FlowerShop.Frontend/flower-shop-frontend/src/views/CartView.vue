<script setup>
import { ref, computed, onMounted } from 'vue'

const cartItems = ref([])

onMounted(() => {
  loadCart()
})

const loadCart = () => {
  const savedCart = localStorage.getItem('flowerShopCart')
  if (savedCart) {
    cartItems.value = JSON.parse(savedCart)
  }
}

const updateQuantity = (itemId, newQuantity) => {
  if (newQuantity <= 0) {
    removeItem(itemId)
    return
  }
  
  const item = cartItems.value.find(item => item.id === itemId)
  if (item) {
    item.quantity = newQuantity
    saveCart()
  }
}

const removeItem = (itemId) => {
  cartItems.value = cartItems.value.filter(item => item.id !== itemId)
  saveCart()
}

const clearCart = () => {
  cartItems.value = []
  saveCart()
}

const saveCart = () => {
  localStorage.setItem('flowerShopCart', JSON.stringify(cartItems.value))
  window.dispatchEvent(new Event('cartUpdated'))
}

const totalAmount = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + (item.price * item.quantity), 0)
})

const totalItems = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + item.quantity, 0)
})
</script>

<template>
  <div class="container py-4">
    <div class="row">
      <div class="col-12">
        <h2 class="mb-4">Alışveriş Sepeti</h2>
        
        <!-- Empty Cart -->
        <div v-if="cartItems.length === 0" class="text-center py-5">
          <i class="bi bi-cart-x text-muted" style="font-size: 4rem;"></i>
          <h4 class="text-muted mt-3">Sepetiniz boş</h4>
          <p class="text-muted">Çiçek alışverişine başlamak için ürünleri keşfedin.</p>
          <router-link to="/flowers" class="btn btn-success">
            <i class="bi bi-flower1"></i> Çiçekleri Keşfet
          </router-link>
        </div>
        
        <!-- Cart Items -->
        <div v-else>
          <div class="row">
            <div class="col-lg-8">
              <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <h5 class="mb-0">Sepetinizdeki Ürünler ({{ totalItems }} adet)</h5>
                  <button @click="clearCart" class="btn btn-outline-danger btn-sm">
                    <i class="bi bi-trash"></i> Sepeti Temizle
                  </button>
                </div>
                <div class="card-body p-0">
                  <div v-for="item in cartItems" :key="item.id" class="border-bottom p-3">
                    <div class="row align-items-center">
                      <div class="col-md-2">
                        <img :src="item.imageUrl || 'https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=100'" 
                             :alt="item.name" class="img-fluid rounded" style="height: 80px; object-fit: cover;">
                      </div>
                      <div class="col-md-4">
                        <h6 class="mb-1">{{ item.name }}</h6>
                        <small class="text-muted">{{ item.description }}</small>
                      </div>
                      <div class="col-md-2">
                        <span class="fw-bold text-success">{{ item.price }}₺</span>
                      </div>
                      <div class="col-md-3">
                        <div class="input-group input-group-sm">
                          <button @click="updateQuantity(item.id, item.quantity - 1)" 
                                  class="btn btn-outline-secondary" type="button">
                            <i class="bi bi-dash"></i>
                          </button>
                          <input v-model.number="item.quantity" 
                                 @change="updateQuantity(item.id, item.quantity)"
                                 type="number" class="form-control text-center" min="1">
                          <button @click="updateQuantity(item.id, item.quantity + 1)" 
                                  class="btn btn-outline-secondary" type="button">
                            <i class="bi bi-plus"></i>
                          </button>
                        </div>
                      </div>
                      <div class="col-md-1">
                        <button @click="removeItem(item.id)" class="btn btn-outline-danger btn-sm">
                          <i class="bi bi-x"></i>
                        </button>
                      </div>
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
                  <div class="d-flex justify-content-between mb-2">
                    <span>Ürün Toplamı:</span>
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
                  <router-link to="/checkout" class="btn btn-success w-100">
                    <i class="bi bi-credit-card"></i> Siparişi Tamamla
                  </router-link>
                </div>
              </div>
              
              <!-- Continue Shopping -->
              <div class="mt-3">
                <router-link to="/flowers" class="btn btn-outline-success w-100">
                  <i class="bi bi-arrow-left"></i> Alışverişe Devam Et
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
.input-group input[type="number"] {
  -moz-appearance: textfield;
}

.input-group input[type="number"]::-webkit-outer-spin-button,
.input-group input[type="number"]::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}
</style>
