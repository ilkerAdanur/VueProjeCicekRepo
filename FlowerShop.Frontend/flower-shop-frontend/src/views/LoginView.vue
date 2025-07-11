<template>
  <div class="login-view">
    <div class="container-fluid vh-100">
      <div class="row h-100">
        <!-- Left Side - Image/Branding -->
        <div class="col-lg-6 d-none d-lg-flex align-items-center justify-content-center bg-primary">
          <div class="text-center text-white">
            <i class="bi bi-flower1 display-1 mb-4"></i>
            <h2 class="fw-bold mb-3">Çiçek Dükkanı</h2>
            <p class="lead">Her özel anınız için en güzel çiçekler</p>
            <div class="mt-5">
              <div class="row text-center">
                <div class="col-4">
                  <i class="bi bi-heart-fill fs-1 mb-2"></i>
                  <p class="small">Sevgi Dolu</p>
                </div>
                <div class="col-4">
                  <i class="bi bi-truck fs-1 mb-2"></i>
                  <p class="small">Hızlı Teslimat</p>
                </div>
                <div class="col-4">
                  <i class="bi bi-shield-check fs-1 mb-2"></i>
                  <p class="small">Güvenilir</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Right Side - Login Form -->
        <div class="col-lg-6 d-flex align-items-center justify-content-center">
          <div class="login-form-container w-100" style="max-width: 400px;">
            <div class="text-center mb-5">
              <i class="bi bi-flower1 text-primary fs-1 mb-3"></i>
              <h3 class="fw-bold">Hoş Geldiniz</h3>
              <p class="text-muted">Hesabınıza giriş yapın</p>
            </div>

            <!-- Login Form -->
            <form @submit.prevent="handleLogin" v-if="!showRegister">
              <div class="mb-3">
                <label class="form-label">Kullanıcı Adı</label>
                <div class="input-group">
                  <span class="input-group-text">
                    <i class="bi bi-person"></i>
                  </span>
                  <input 
                    v-model="loginForm.username" 
                    type="text" 
                    class="form-control" 
                    placeholder="Kullanıcı adınızı girin"
                    required
                  >
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">Şifre</label>
                <div class="input-group">
                  <span class="input-group-text">
                    <i class="bi bi-lock"></i>
                  </span>
                  <input 
                    v-model="loginForm.password" 
                    :type="showPassword ? 'text' : 'password'" 
                    class="form-control" 
                    placeholder="Şifrenizi girin"
                    required
                  >
                  <button 
                    type="button" 
                    class="btn btn-outline-secondary"
                    @click="showPassword = !showPassword"
                  >
                    <i :class="showPassword ? 'bi bi-eye-slash' : 'bi bi-eye'"></i>
                  </button>
                </div>
              </div>

              <div class="mb-4">
                <div class="form-check">
                  <input class="form-check-input" type="checkbox" id="rememberMe">
                  <label class="form-check-label" for="rememberMe">
                    Beni hatırla
                  </label>
                </div>
              </div>

              <button 
                type="submit" 
                class="btn btn-primary w-100 mb-3"
                :disabled="loading"
              >
                <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                <i v-else class="bi bi-box-arrow-in-right me-2"></i>
                Giriş Yap
              </button>

              <div class="text-center">
                <button 
                  type="button" 
                  class="btn btn-link text-decoration-none"
                  @click="showRegister = true"
                >
                  Hesabınız yok mu? Kayıt olun
                </button>
              </div>
            </form>

            <!-- Register Form -->
            <form @submit.prevent="handleRegister" v-else>
              <div class="row">
                <div class="col-6 mb-3">
                  <label class="form-label">Ad</label>
                  <input 
                    v-model="registerForm.firstName" 
                    type="text" 
                    class="form-control" 
                    placeholder="Adınız"
                    required
                  >
                </div>
                <div class="col-6 mb-3">
                  <label class="form-label">Soyad</label>
                  <input 
                    v-model="registerForm.lastName" 
                    type="text" 
                    class="form-control" 
                    placeholder="Soyadınız"
                    required
                  >
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">Kullanıcı Adı</label>
                <input 
                  v-model="registerForm.username" 
                  type="text" 
                  class="form-control" 
                  placeholder="Kullanıcı adınız"
                  required
                >
              </div>

              <div class="mb-3">
                <label class="form-label">E-posta</label>
                <input 
                  v-model="registerForm.email" 
                  type="email" 
                  class="form-control" 
                  placeholder="E-posta adresiniz"
                  required
                >
              </div>

              <div class="mb-3">
                <label class="form-label">Şifre</label>
                <input 
                  v-model="registerForm.password" 
                  type="password" 
                  class="form-control" 
                  placeholder="Şifreniz"
                  required
                >
              </div>

              <button 
                type="submit" 
                class="btn btn-success w-100 mb-3"
                :disabled="loading"
              >
                <span v-if="loading" class="spinner-border spinner-border-sm me-2"></span>
                <i v-else class="bi bi-person-plus me-2"></i>
                Kayıt Ol
              </button>

              <div class="text-center">
                <button 
                  type="button" 
                  class="btn btn-link text-decoration-none"
                  @click="showRegister = false"
                >
                  Zaten hesabınız var mı? Giriş yapın
                </button>
              </div>
            </form>

            <!-- Error/Success Messages -->
            <div v-if="message" :class="`alert ${messageType === 'error' ? 'alert-danger' : 'alert-success'} mt-3`">
              <i :class="`bi ${messageType === 'error' ? 'bi-exclamation-triangle' : 'bi-check-circle'} me-2`"></i>
              {{ message }}
            </div>

            <!-- Demo Credentials -->
            <div class="mt-4 p-3 bg-light rounded">
              <h6 class="fw-bold mb-2">Demo Hesapları:</h6>
              <small class="text-muted">
                <strong>Admin:</strong> admin / admin123<br>
                <strong>Müşteri:</strong> demo / demo123
              </small>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { authApi } from '../services/api'

export default {
  name: 'LoginView',
  data() {
    return {
      showRegister: false,
      showPassword: false,
      loading: false,
      message: '',
      messageType: 'error',
      loginForm: {
        username: '',
        password: ''
      },
      registerForm: {
        username: '',
        email: '',
        password: '',
        firstName: '',
        lastName: ''
      }
    }
  },
  methods: {
    async handleLogin() {
      try {
        this.loading = true
        this.message = ''
        
        const response = await authApi.login(this.loginForm)
        
        // Store user data
        localStorage.setItem('user', JSON.stringify(response.data))
        localStorage.setItem('token', response.data.token)

        // Emit event to update header
        window.dispatchEvent(new Event('userLoggedIn'))

        this.message = 'Giriş başarılı! Yönlendiriliyorsunuz...'
        this.messageType = 'success'

        // Redirect based on role
        setTimeout(() => {
          if (response.data.role === 'Admin') {
            this.$router.push('/admin')
          } else {
            this.$router.push('/')
          }
        }, 1500)
        
      } catch (error) {
        console.error('Login error:', error)
        this.message = error.response?.data?.message || 'Giriş işlemi başarısız.'
        this.messageType = 'error'
      } finally {
        this.loading = false
      }
    },
    
    async handleRegister() {
      try {
        this.loading = true
        this.message = ''
        
        const response = await authApi.register(this.registerForm)
        
        // Store user data
        localStorage.setItem('user', JSON.stringify(response.data))
        localStorage.setItem('token', response.data.token)

        // Emit event to update header
        window.dispatchEvent(new Event('userLoggedIn'))

        this.message = 'Kayıt başarılı! Yönlendiriliyorsunuz...'
        this.messageType = 'success'

        // Redirect to home
        setTimeout(() => {
          this.$router.push('/')
        }, 1500)
        
      } catch (error) {
        console.error('Register error:', error)
        this.message = error.response?.data?.message || 'Kayıt işlemi başarısız.'
        this.messageType = 'error'
      } finally {
        this.loading = false
      }
    }
  }
}
</script>

<style scoped>
.login-view {
  min-height: 100vh;
}

.bg-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%) !important;
}

.login-form-container {
  padding: 2rem;
}

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

.btn-success {
  background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
  border: none;
}

.btn-success:hover {
  background: linear-gradient(135deg, #218838 0%, #1ea085 100%);
  transform: translateY(-1px);
}
</style>
