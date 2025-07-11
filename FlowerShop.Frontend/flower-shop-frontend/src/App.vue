<script setup>
import { RouterLink, RouterView } from 'vue-router'
import { ref, onMounted } from 'vue'
import { occasionsApi } from './services/api'

const cartItems = ref([])
const cartCount = ref(0)
const occasions = ref([])
const user = ref(null)

const updateCartCount = () => {
  const cartKey = user.value ? `flowerShopCart_${user.value.id}` : 'flowerShopCart'
  const savedCart = localStorage.getItem(cartKey)
  if (savedCart) {
    cartItems.value = JSON.parse(savedCart)
    cartCount.value = cartItems.value.reduce((sum, item) => sum + item.quantity, 0)
  } else {
    cartItems.value = []
    cartCount.value = 0
  }
}

const checkUserLogin = () => {
  const savedUser = localStorage.getItem('user')
  if (savedUser) {
    user.value = JSON.parse(savedUser)
    // Update cart count for the logged-in user
    updateCartCount()
  }
}

const logout = () => {
  // Clear user data
  localStorage.removeItem('user')
  localStorage.removeItem('token')

  // Clear user-specific cart data
  localStorage.removeItem('flowerShopCart')
  localStorage.removeItem('flowerShopCustomer')

  user.value = null

  // Update cart count
  updateCartCount()

  // Always redirect to home page after logout
  window.location.href = '/'
}

const fetchOccasions = async () => {
  try {
    const response = await occasionsApi.getAll()
    occasions.value = response.data.occasions.slice(0, 8) // İlk 8 özel günü göster
  } catch (error) {
    console.error('Error fetching occasions:', error)
  }
}

const showDropdown = (event) => {
  const dropdown = event.currentTarget.querySelector('.dropdown-menu')
  if (dropdown) {
    dropdown.classList.add('show')
  }
}

const hideDropdown = (event) => {
  const dropdown = event.currentTarget.querySelector('.dropdown-menu')
  if (dropdown) {
    dropdown.classList.remove('show')
  }
}

onMounted(() => {
  updateCartCount()
  fetchOccasions()
  checkUserLogin()

  // Listen for cart updates
  window.addEventListener('cartUpdated', updateCartCount)

  // Listen for user login
  window.addEventListener('userLoggedIn', checkUserLogin)
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
            <li class="nav-item dropdown" @mouseenter="showDropdown" @mouseleave="hideDropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                role="button"
                data-bs-toggle="dropdown"
                aria-expanded="false"
                @click.prevent
              >
                Özel Günler
              </a>
              <ul class="dropdown-menu">
                <li>
                  <RouterLink to="/occasions" class="dropdown-item">
                    <i class="bi bi-grid me-2"></i>
                    Tüm Özel Günler
                  </RouterLink>
                </li>
                <li><hr class="dropdown-divider"></li>
                <li v-for="occasion in occasions" :key="occasion.id">
                  <RouterLink
                    :to="`/occasions/${occasion.id}`"
                    class="dropdown-item"
                  >
                    <i :class="`${occasion.icon} me-2`"></i>
                    {{ occasion.name }}
                    <span class="badge bg-light text-dark ms-2">{{ occasion.flowerCount }}</span>
                  </RouterLink>
                </li>
                <li v-if="occasions.length === 0">
                  <span class="dropdown-item-text text-muted">
                    <i class="bi bi-hourglass-split me-2"></i>
                    Yükleniyor...
                  </span>
                </li>
              </ul>
            </li>
          </ul>

          <div class="d-flex align-items-center">
            <!-- Theme Toggle -->
            <button
              @click="toggleTheme"
              class="btn btn-outline-secondary me-2"
              :title="darkMode ? 'Açık Tema' : 'Koyu Tema'"
            >
              <i :class="darkMode ? 'bi bi-sun' : 'bi bi-moon'"></i>
            </button>

            <RouterLink to="/order-tracking" class="btn btn-outline-secondary me-2">
              <i class="bi bi-search"></i>
              Sipariş Takibi
            </RouterLink>
            <RouterLink v-if="user" to="/my-orders" class="btn btn-outline-info me-2">
              <i class="bi bi-list-ul"></i>
              Siparişlerim
            </RouterLink>
            <RouterLink to="/cart" class="btn btn-outline-primary position-relative me-2">
              <i class="bi bi-cart"></i>
              Sepet
              <span v-if="cartCount > 0" class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                {{ cartCount }}
              </span>
            </RouterLink>

            <!-- User Menu -->
            <div v-if="user" class="dropdown">
              <a
                class="btn btn-outline-secondary dropdown-toggle d-flex align-items-center"
                href="#"
                role="button"
                data-bs-toggle="dropdown"
                aria-expanded="false"
              >
                <i class="bi bi-person-circle me-2"></i>
                {{ user.firstName }}
              </a>
              <ul class="dropdown-menu dropdown-menu-end">
                <li>
                  <span class="dropdown-item-text">
                    <strong>{{ user.firstName }} {{ user.lastName }}</strong><br>
                    <small class="text-muted">{{ user.role }}</small>
                  </span>
                </li>
                <li><hr class="dropdown-divider"></li>
                <li v-if="user.role === 'Admin'">
                  <RouterLink to="/admin" class="dropdown-item">
                    <i class="bi bi-gear me-2"></i>
                    Admin Panel
                  </RouterLink>
                </li>
                <li>
                  <a href="#" class="dropdown-item" @click.prevent="logout">
                    <i class="bi bi-box-arrow-right me-2"></i>
                    Çıkış Yap
                  </a>
                </li>
              </ul>
            </div>

            <!-- Login Button -->
            <RouterLink v-else to="/login" class="btn btn-primary">
              <i class="bi bi-box-arrow-in-right me-1"></i>
              Giriş Yap
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

<script>
import { Dropdown } from 'bootstrap'

export default {
  name: 'App',
  data() {
    return {
      user: null,
      cartCount: 0,
      darkMode: false
    }
  },
  mounted() {
    this.loadUser()
    this.updateCartCount()
    this.loadTheme()

    // Initialize Bootstrap dropdowns
    this.$nextTick(() => {
      const dropdownElements = document.querySelectorAll('[data-bs-toggle="dropdown"]')
      dropdownElements.forEach(element => {
        new Dropdown(element)
      })
    })

    // Listen for cart updates
    window.addEventListener('cartUpdated', this.updateCartCount)

    // Listen for user login/logout
    window.addEventListener('userUpdated', this.loadUser)
  },
  beforeUnmount() {
    window.removeEventListener('cartUpdated', this.updateCartCount)
    window.removeEventListener('userUpdated', this.loadUser)
  },
  methods: {
    loadUser() {
      const userData = localStorage.getItem('flowerShopUser')
      this.user = userData ? JSON.parse(userData) : null

      // Re-initialize dropdowns after user state change
      this.$nextTick(() => {
        const dropdownElements = document.querySelectorAll('[data-bs-toggle="dropdown"]')
        dropdownElements.forEach(element => {
          new Dropdown(element)
        })
      })
    },
    updateCartCount() {
      const cart = JSON.parse(localStorage.getItem('flowerShopCart') || '[]')
      this.cartCount = cart.reduce((total, item) => total + item.quantity, 0)
    },
    logout() {
      localStorage.removeItem('flowerShopUser')
      this.user = null
      this.$router.push('/')
      window.dispatchEvent(new Event('userUpdated'))
    },
    toggleTheme() {
      this.darkMode = !this.darkMode
      document.body.classList.toggle('dark-theme', this.darkMode)
      localStorage.setItem('darkMode', this.darkMode.toString())
    },
    loadTheme() {
      const savedTheme = localStorage.getItem('darkMode')
      if (savedTheme !== null) {
        this.darkMode = savedTheme === 'true'
        document.body.classList.toggle('dark-theme', this.darkMode)
      }
    }
  }
}
</script>

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

/* Dark Theme */
.dark-theme {
  background-color: #121212 !important;
  color: #ffffff !important;
}

.dark-theme .navbar {
  background-color: #1f1f1f !important;
}

.dark-theme .navbar-brand,
.dark-theme .nav-link {
  color: #ffffff !important;
}

.dark-theme .btn-outline-secondary {
  border-color: #6c757d;
  color: #6c757d;
}

.dark-theme .btn-outline-secondary:hover {
  background-color: #6c757d;
  color: #ffffff;
}

.dark-theme .btn-outline-primary {
  border-color: #0d6efd;
  color: #0d6efd;
}

.dark-theme .btn-outline-primary:hover {
  background-color: #0d6efd;
  color: #ffffff;
}

.dark-theme .btn-outline-info {
  border-color: #0dcaf0;
  color: #0dcaf0;
}

.dark-theme .btn-outline-info:hover {
  background-color: #0dcaf0;
  color: #000000;
}

.dark-theme .card {
  background-color: #1f1f1f !important;
  border-color: #333333 !important;
  color: #ffffff !important;
}

.dark-theme .dropdown-menu {
  background-color: #1f1f1f !important;
  border-color: #333333 !important;
}

.dark-theme .dropdown-item {
  color: #ffffff !important;
}

.dark-theme .dropdown-item:hover {
  background-color: #333333 !important;
}

.dark-theme .dropdown-item-text {
  color: #ffffff !important;
}

.dark-theme .dropdown-divider {
  border-color: #333333 !important;
}

.dark-theme footer {
  background-color: #000000 !important;
}
</style>
