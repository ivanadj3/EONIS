<template>
  <n-layout class="app">
    <!-- Navbar -->
    <n-layout-header bordered class="navbar">
      <div class="nav-left">
        <span class="logo">🥤 DrinkStore</span>
        <n-space size="large" class="nav-links">
          <router-link to="/">Pocetna strana</router-link>
          <router-link to="/drink-list">Lista pica</router-link>
          <a href="#">Categories</a>
          <a href="#">About</a>
          <a href="#">Contact</a>
        </n-space>
      </div>
      <div class="nav-right">
        <n-button text>🔍</n-button>
        <router-link to="/cart">
          <n-badge :value="cartCount">
            <n-button text>🛒</n-button>
          </n-badge>
        </router-link>
        <router-link v-if="Object.keys(user).length === 0" to="/login">Sign In</router-link>
        <n-button v-if="Object.keys(user).length !== 0" text @click="handleLogout">Logout</n-button>
      </div>
    </n-layout-header>

  </n-layout>

  <n-message-provider>
    <router-view />
  </n-message-provider>
</template>

<script setup>
import { computed } from 'vue';
import { cartItems } from './store/cart';
import { deleteUser, user } from './store/user';

  const products = [
    { name: "Orange Juice", desc: "Fresh & natural vitamin boost.", price: 4 },
    { name: "Iced Coffee", desc: "Cold brew for energy lovers.", price: 5 },
    { name: "Berry Smoothie", desc: "Sweet, healthy, refreshing.", price: 6 }
  ];

  const cartCount = computed(() =>
    cartItems.value.reduce(
      (sum, item) => sum + item.quantity,
      0
    )
  );

  const handleLogout = () => {
    deleteUser();
  }
</script>

<style scoped>
  .app {
    font-family: system-ui;
  }

  .navbar {
    display: flex;
    justify-content: space-between;
    padding: 12px 24px;
    align-items: center;
  }

  .nav-left {
    display: flex;
    align-items: center;
    gap: 20px;
  }

  .logo {
    font-weight: bold;
    font-size: 18px;
  }

  .nav-links a {
    margin-right: 12px;
    text-decoration: none;
    color: #333;
  }

  .nav-right {
    display: flex;
    align-items: center;
    gap: 10px;
  }

  .footer {
    padding: 20px;
  }
</style>
