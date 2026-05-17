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
    component: Cart
  },
  {
    path: '/pre-checkout/:id',
    name: 'PreCheckout',
    component: PreCheckout
  },
  {
    path: '/orders/my',
    name: 'MyOrders',
    component: OrdersList
  },
  {
    path: '/orders/:id',
    name: 'OrderDetails',
    component: OrderDetails
  },
  {
    path: '/admin/products',
    name: 'AdminProduct',
    component: AdminProduct
  },
  {
    path: '/admin/orders',
    name: 'AdminOrders',
    component: AdminOrders
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router