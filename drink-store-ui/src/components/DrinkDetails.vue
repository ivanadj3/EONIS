<template>
  <div class="page">
    <n-spin :show="loading">
      
      <n-card v-if="product" class="card">
        
        <div class="layout">
          
          <!-- IMAGE -->
          <img :src="product.image" class="img" />

          <!-- INFO -->
          <div class="info">

            <h2>{{ product.title }}</h2>

            <p class="price">{{ product.price }} din.</p>

            <p class="desc">
              {{ product.description || "No description available." }}
            </p>

            <!-- QUANTITY -->
            <div class="qty">
              <span>Quantity:</span>

              <n-input-number
                v-model:value="quantity"
                :min="1"
                :max="99"
              />
            </div>

            <!-- ADD TO CART -->
            <n-button type="primary" size="large" @click="addToCart">
              Add to Cart
            </n-button>

          </div>
        </div>

      </n-card>

    </n-spin>

    <!-- ERROR -->
    <n-alert v-if="error" type="error" title="Failed to load product">
      {{ error.message }}
    </n-alert>

  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { fetchProductByIdApi } from "../api/products.api";

const route = useRoute();

const product = ref(null);
const loading = ref(false);
const error = ref(null);

const quantity = ref(1);

// fake cart (replace later with Pinia if needed)
const cart = ref([]);

const fetchProduct = async () => {
  loading.value = true;
  error.value = null;

  try {
    const id = route.params.id;

    const res = await fetchProductByIdApi(id);
    product.value = res;

  } catch (e) {
    error.value = e;
  } finally {
    loading.value = false;
  }
};

const addToCart = () => {
  if (!product.value) return;

  cart.value.push({
    product: product.value,
    quantity: quantity.value,
  });

  console.log("Cart:", cart.value);
};

onMounted(fetchProduct);
</script>

<style scoped>
.page {
  padding: 24px;
  min-height: 100vh;
  color: white;
}

.card {
  max-width: 1000px;
  margin: auto;
  border-radius: 16px;
  background: rgba(255,255,255,0.05);
  backdrop-filter: blur(10px);
}

.layout {
  display: flex;
  gap: 24px;
}

.img {
  width: 400px;
  height: 300px;
  object-fit: cover;
  border-radius: 12px;
}

.info {
  flex: 1;
}

.price {
  font-size: 24px;
  color: #034901;
  margin: 10px 0;
}

.desc {
  opacity: 0.8;
  margin-bottom: 20px;
}

.qty {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 20px;
}
</style>