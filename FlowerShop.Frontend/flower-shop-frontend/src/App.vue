<script setup>
import { RouterLink, RouterView } from 'vue-router'
import { ref, onMounted } from 'vue'

const cartItems = ref([])
const cartCount = ref(0)

const updateCartCount = () => {
  const savedCart = localStorage.getItem('flowerShopCart')
  if (savedCart) {
    cartItems.value = JSON.parse(savedCart)
    cartCount.value = cartItems.value.reduce((sum, item) => sum + item.quantity, 0)
  } else {
    cartItems.value = []
    cartCount.value = 0
  }
}

onMounted(() => {
  updateCartCount()

  // Listen for cart updates
  window.addEventListener('cartUpdated', updateCartCount)
})
</script>

<template>
  <div id="app">
    <!-- Navigation Header -->
    <nav class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
      <div class="container">
        <RouterLink to="/" class="navbar-brand">
          <i class="bi bi-flower1"></i>
          <strong>Çiçek Dükkanı</strong>
        </RouterLink>

        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <RouterLink to="/" class="nav-link">Ana Sayfa</RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink to="/flowers" class="nav-link">Çiçekler</RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink to="/categories" class="nav-link">Kategoriler</RouterLink>
            </li>
          </ul>

          <div class="d-flex">
            <RouterLink to="/cart" class="btn btn-outline-primary position-relative me-2">
              <i class="bi bi-cart"></i>
              Sepet
              <span v-if="cartCount > 0" class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                {{ cartCount }}
              </span>
            </RouterLink>
          </div>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <main>
      <RouterView />
    </main>

    <!-- Footer -->
    <footer class="bg-dark text-light py-4 mt-5">
      <div class="container">
        <div class="row">
          <div class="col-md-6">
            <h5><i class="bi bi-flower1"></i> Çiçek Dükkanı</h5>
            <p>En taze çiçeklerle hayatınıza renk katın.</p>
          </div>
          <div class="col-md-6">
            <h6>İletişim</h6>
            <p>
              <i class="bi bi-telephone"></i> +90 555 123 4567<br>
              <i class="bi bi-envelope"></i> info@cicekdukkani.com
            </p>
          </div>
        </div>
        <hr>
        <div class="text-center">
          <small>&copy; 2024 Çiçek Dükkanı. Tüm hakları saklıdır.</small>
        </div>
      </div>
    </footer>
  </div>
</template>

<style>
#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

main {
  flex: 1;
}

.navbar-brand {
  font-size: 1.5rem;
  color: #28a745 !important;
}

.navbar-brand i {
  margin-right: 0.5rem;
}

.nav-link.router-link-active {
  color: #28a745 !important;
  font-weight: bold;
}

footer {
  margin-top: auto;
}
</style>
