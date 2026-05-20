<template>
  <n-layout class="app">
    <!-- Navbar -->
     <div v-if="isLoggedIn() && isAdmin()" class="navbar-yellow">ADMIN</div>
    <n-layout-header bordered class="navbar">
      <div class="nav-left">
        <span class="logo">🥤 Piće na klik</span>
        <n-space size="large" class="nav-links">
          <router-link to="/" class="nav-link">Početna strana</router-link>
          <router-link to="/drink-list" class="nav-link">Lista pića</router-link>
          <router-link v-if="isLoggedIn() && !isAdmin()" to="/orders/my" class="nav-link">Moje porudžbine</router-link>
          <router-link v-if="isLoggedIn() && isAdmin()" to="/admin/products" class="nav-link">Proizvodi</router-link>
          <router-link v-if="isLoggedIn() && isAdmin()" to="/admin/orders" class="nav-link">Porudžbine</router-link>
          <router-link v-if="isLoggedIn() && isAdmin()" to="/admin/users" class="nav-link">Korisnici</router-link>
        </n-space>
      </div>
      <div class="nav-right">
        <router-link v-if="isLoggedIn() && !isAdmin()" to="/cart">
          <n-badge :value="cartCount">
            <n-button text>🛒</n-button>
          </n-badge>
        </router-link>
        <router-link v-if="!isLoggedIn()" to="/login" class="nav-link">Login</router-link>
        <router-link v-if="!isLoggedIn()" to="/sign-up" class="nav-link">Registracija</router-link>
        <n-button v-if="isLoggedIn()" text @click="handleLogout" class="nav-link">Logout</n-button>
      </div>
    </n-layout-header>

  </n-layout>

  <n-config-provider>
    <n-dialog-provider>
      <n-message-provider>
        <router-view />
      </n-message-provider>
    </n-dialog-provider>
  </n-config-provider>
</template>

<script setup>
import { computed } from 'vue';
import { cartItems } from './store/cart';
import { deleteUser, isAdmin, isLoggedIn } from './store/user';

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

  .navbar-yellow {
    background-color: darkgoldenrod;
    text-align: center;
    font-size: 30px;
    color: white;
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

  .nav-link {
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
