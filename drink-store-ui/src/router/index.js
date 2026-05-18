import { createRouter, createWebHistory } from 'vue-router'
import Home from '../components/Home.vue'
import DrinkList from '../components/DrinkList.vue'
import DrinkDetails from '../components/DrinkDetails.vue'
import Login from '../components/Login.vue'
import SignUp from '../components/SignUp.vue'
import Cart from '../components/Cart.vue'
import PreCheckout from '../components/PreCheckout.vue'
import OrdersList from '../components/OrdersList.vue'
import OrderDetails from '../components/OrderDetails.vue'
import AdminProduct from '../components/AdminProduct.vue'
import AdminOrders from '../components/AdminOrders.vue'
import AdminUsers from '../components/AdminUsers.vue'
import { user } from '../store/user'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/drink-list',
    name: 'DrinkList',
    component: DrinkList
  },
  {
    path: '/drinks/:id',
    name: 'DrinkDetails',
    component: DrinkDetails
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/sign-up',
    name: 'SignUp',
    component: SignUp
  },
  {
    path: '/cart',
    name: 'Cart',
    component: Cart,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/pre-checkout/:id',
    name: 'PreCheckout',
    component: PreCheckout,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/orders/my',
    name: 'MyOrders',
    component: OrdersList,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/orders/:id',
    name: 'OrderDetails',
    component: OrderDetails,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/admin/products',
    name: 'AdminProduct',
    component: AdminProduct,
    meta: {
      requiresAuth: true,
      requiresAdmin: true
    }
  },
  { 
    path: '/admin/orders',
    name: 'AdminOrders',
    component: AdminOrders,
    meta: {
      requiresAuth: true,
      requiresAdmin: true
    }
  },
  {
    path: '/admin/users',
    name: 'AdminUsers',
    component: AdminUsers,
    meta: {
      requiresAuth: true,
      requiresAdmin: true
    }
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from) => {
  const token = user.value.jwt
  const role = user.value.role

  if (to.meta.requiresAuth && !token) {
    return "/login";
  }

  if (to.meta.requiresAdmin && role !== 'Admin') {
    return "/";
  }

  return true;
});

export default router