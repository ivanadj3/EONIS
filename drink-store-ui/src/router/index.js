import { createRouter, createWebHistory } from 'vue-router'
import Home from '../components/Home.vue'
import DrinkList from '../components/DrinkList.vue'

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
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router