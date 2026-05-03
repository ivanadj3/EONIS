import { createRouter, createWebHistory } from 'vue-router'
import Home from '../components/Home.vue'
import DrinkList from '../components/DrinkList.vue'
import DrinkDetails from '../components/DrinkDetails.vue'

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
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router