<template>
  <div class="hero">

    <div class="content">

      <h1 class="title">
        Piće na klik
      </h1>

      <p class="subtitle">
        Vaša omiljena prodavnica pića
      </p>

      <n-button
        type="primary"
        size="large"
        round
        @click="router.push('/drink-list')"
      >
        Proizvodi
      </n-button>

    </div>

  </div>
</template>

<script setup>
import { useRouter } from "vue-router";

const router = useRouter();
</script>

<style scoped>
.hero {
  position: relative;

  min-height: 100vh;

  display: flex;
  align-items: center;

  padding-left: 100px;

  overflow: hidden;

  background-image:
    linear-gradient(
      to right,
      rgba(255,255,255,1) 0%,
      rgba(255,255,255,0.92) 20%,
      rgba(255,255,255,0.7) 35%,
      rgba(255,255,255,0.2) 55%,
      rgba(255,255,255,0) 75%
    ),
    url("/public/wallpaper.jpg");

  background-size: cover;
  background-position: center;
}

.content {
  position: relative;

  z-index: 2;

  max-width: 520px;
}

.title {
  margin: 0;

  font-size: 76px;
  line-height: 1;

  font-weight: 900;

  color: #111827;

  letter-spacing: -3px;
}

.subtitle {
  margin-top: 28px;
  margin-bottom: 36px;

  font-size: 20px;
  line-height: 1.8;

  color: #4b5563;
}

</style>