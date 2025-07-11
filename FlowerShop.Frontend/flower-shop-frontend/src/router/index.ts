import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/flowers',
      name: 'flowers',
      component: () => import('../views/FlowersView.vue'),
    },
    {
      path: '/flowers/:id',
      name: 'flower-detail',
      component: () => import('../views/FlowerDetailView.vue'),
    },
    {
      path: '/categories',
      name: 'categories',
      component: () => import('../views/CategoriesView.vue'),
    },
    {
      path: '/occasions',
      name: 'occasions',
      component: () => import('../views/OccasionsView.vue'),
    },
    {
      path: '/occasions/:id',
      name: 'occasion-flowers',
      component: () => import('../views/OccasionFlowersView.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/order-tracking',
      name: 'order-tracking',
      component: () => import('../views/OrderTrackingView.vue'),
    },
    {
      path: '/my-orders',
      name: 'my-orders',
      component: () => import('../views/MyOrdersView.vue'),
    },
    {
      path: '/admin',
      name: 'admin',
      component: () => import('../views/AdminView.vue'),
    },
    {
      path: '/cart',
      name: 'cart',
      component: () => import('../views/CartView.vue'),
    },
    {
      path: '/checkout',
      name: 'checkout',
      component: () => import('../views/CheckoutView.vue'),
    },
  ],
})

export default router
