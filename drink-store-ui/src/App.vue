<template>
  <n-layout class="app">
    <!-- Navbar -->
    <n-layout-header bordered class="navbar">
      <div class="nav-left">
        <span class="logo">🥤 DrinkStore</span>
        <n-space size="large" class="nav-links">
          <router-link to="/">Home</router-link>
          <a href="#">Shop</a>
          <a href="#">Categories</a>
          <a href="#">About</a>
          <a href="#">Contact</a>
        </n-space>
      </div>
      <div class="nav-right">
        <n-button text>🔍</n-button>
        <n-badge :value="2">
          <n-button text>🛒</n-button>
        </n-badge>
        <n-button type="primary" size="small">Sign In</n-button>
      </div>
    </n-layout-header>

    <!-- Hero -->
    <section class="hero">
      <div class="hero-text">
        <n-tag type="success">Premium Drinks Delivered</n-tag>
        <h1>Refresh Your Day</h1>
        <h2>The Natural Way</h2>
        <p>
          Discover refreshing juices, smoothies, and iced coffees crafted for every mood.
          Delivered fresh to your door.
        </p>

        <n-space>
          <n-button type="primary" size="large">Shop Now →</n-button>
          <n-button size="large" secondary>Explore</n-button>
        </n-space>

        <div class="features">
          <div>🚚 Fast Delivery<br /><small>30 min delivery</small></div>
          <div>🌿 100% Natural<br /><small>No added sugar</small></div>
          <div>🔒 Secure Payment<br /><small>Safe & secure</small></div>
        </div>
      </div>

      <div class="hero-images">
        <div class="bottle orange">Orange Juice</div>
        <div class="bottle coffee">Iced Coffee</div>
        <div class="bottle berry">Berry Smoothie</div>
      </div>
    </section>

    <!-- Products -->
    <section class="products">
      <h2>Our Most Loved Drinks</h2>
      <n-grid :cols="3" :x-gap="16">
        <n-grid-item v-for="item in products" :key="item.name">
          <n-card hoverable>
            <h3>{{ item.name }}</h3>
            <p>{{ item.desc }}</p>
            <div class="price-row">
              <strong>${{ item.price }}</strong>
              <n-button type="primary" size="small">Buy Now</n-button>
            </div>
          </n-card>
        </n-grid-item>
      </n-grid>
    </section>

    <!-- Newsletter -->
    <section class="newsletter">
      <h3>Get 20% Off Your First Order</h3>
      <n-space>
        <n-input placeholder="Enter your email" />
        <n-button type="primary">Sign Up</n-button>
      </n-space>
    </section>

    <!-- Footer -->
    <n-layout-footer bordered class="footer">
      <n-space justify="space-between">
        <span>🚚 Fast Delivery</span>
        <span>🌿 100% Natural</span>
        <span>🔒 Secure Payment</span>
        <span>📞 24/7 Support</span>
      </n-space>
    </n-layout-footer>
  </n-layout>

  <router-view />
</template>

<script setup>
const products = [
  { name: "Orange Juice", desc: "Fresh & natural vitamin boost.", price: 4 },
  { name: "Iced Coffee", desc: "Cold brew for energy lovers.", price: 5 },
  { name: "Berry Smoothie", desc: "Sweet, healthy, refreshing.", price: 6 }
];
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

.hero {
  display: flex;
  justify-content: space-between;
  padding: 60px 40px;
  background: linear-gradient(120deg, #f5fff7, #ffffff);
}

.hero-text h1 {
  font-size: 42px;
  margin: 10px 0;
}

.hero-text h2 {
  color: #4caf50;
  margin-bottom: 10px;
}

.features {
  display: flex;
  gap: 20px;
  margin-top: 20px;
  font-size: 14px;
}

.hero-images {
  display: flex;
  gap: 10px;
  align-items: center;
}

.bottle {
  width: 100px;
  height: 160px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 12px;
}

.orange { background: orange; }
.coffee { background: #6b4f3b; }
.berry { background: #c2185b; }

.products {
  padding: 40px;
}

.price-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 10px;
}

.newsletter {
  padding: 40px;
  text-align: center;
  background: #f7fff9;
}

.footer {
  padding: 20px;
}
</style>
