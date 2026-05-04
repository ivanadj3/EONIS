import { createRouter, createWebHistory } from 'vue-router'
import Home from '../components/Home.vue'
import DrinkList from '../components/DrinkList.vue'
import DrinkDetails from '../components/DrinkDetails.vue'
import Login from '../components/Login.vue'
import SignUp from '../components/SignUp.vue'

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
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router