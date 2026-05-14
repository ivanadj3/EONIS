<template>
  <div class="page">
    <n-spin :show="loading">

      <n-card class="card" v-if="order">

        <div class="header">
          <div>
            <h1>Porudzbina #{{ order.id }}</h1>

            <div class="date">
              {{ formatDate( order.createdAt )}}
            </div>
          </div>

          <n-tag
            :type="order.isPaid ? 'success' : 'warning'"
            size="large"
          >
            {{ order.isPaid ? 'Placena' : 'Nije placena' }}
          </n-tag>
        </div>

        <div class="section">
          <div class="section-title">
            Podaci za dostavu
          </div>

          <div class="shipping-grid">

            <div class="shipping-item">
              <span>Name</span>
              <strong>{{ order.user }}</strong>
            </div>

            <div class="shipping-item">
              <span>Phone</span>
              <strong>{{ order.phone }}</strong>
            </div>

            <div class="shipping-item">
              <span>Address</span>
              <strong>{{ order.address }}</strong>
            </div>

          </div>
        </div>

        <hr />

        <div class="section">

          <div class="section-title">
            Proizvodi
          </div>

          <div
            v-for="item in order.items"
            :key="item.productId"
            class="item"
          >

            <div class="info">
              <div class="title">
                {{ item.productName }}
              </div>

              <div class="meta">
                Kolicina: {{ item.quantity }}
              </div>
            </div>

            <div class="price-wrapper">
              <div class="subtotal">
                {{ item.totalPrice.toFixed(2) }} din.
              </div>
            </div>

          </div>

        </div>

        <hr />

        <div class="section">

          <div class="section-title">
            Podaci o placanju
          </div>

          <div class="summary">

            <div class="summary-row total">
              <span>Ukupno</span>
              <span>{{ order.totalAmount.toFixed(2) }} din.</span>
            </div>

          </div>

        </div>

        <div class="actions">

          <n-button
            secondary
            @click="$router.push('/orders/my')"
          >
            Lista porudzbina
          </n-button>

        </div>

      </n-card>

    </n-spin>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute } from "vue-router";
import { useMessage } from "naive-ui";
import { getOrderByIdApi } from "../api/order.api";

const route = useRoute();
const message = useMessage();

const order = ref(null);

const loading = ref(false);
const paymentLoading = ref(false);

const orderId = route.params.id;

const fetchOrder = async () => {
  loading.value = true;

  try {
    const res = await getOrderByIdApi(orderId);

    order.value = res.data;
  } catch (e) {
    message.error(
      e.response?.data?.message ||
      "Failed to load order"
    );
  } finally {
    loading.value = false;
  }
};

const subtotal = computed(() => {
  if (!order.value) return 0;

  return order.value.items.reduce((sum, item) => {
    return sum + item.price * item.quantity;
  }, 0);
});

const formatDate = (date) => {
    return new Date(date).toLocaleString("sr-RS")
};

onMounted(() => {
  fetchOrder();
});
</script>

<style scoped>
.page {
  min-height: 100vh;
  padding: 32px;

}

.card {
  max-width: 1000px;
  margin: 0 auto;

  border-radius: 24px;

  backdrop-filter: blur(12px);
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;

  margin-bottom: 32px;
}

.header h1 {
  margin: 0;
  font-size: 34px;
}

.date {
  margin-top: 8px;
  opacity: 0.7;
}

.section {
  margin-top: 32px;
}

.section-title {
  font-size: 22px;
  font-weight: bold;

  margin-bottom: 18px;
}

.shipping-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

.shipping-item {
  padding: 18px;

  border-radius: 16px;

  background: rgba(255,255,255,0.04);

  display: flex;
  flex-direction: column;
  gap: 8px;
}

.shipping-item span {
  opacity: 0.7;
  font-size: 14px;
}

.item {
  display: flex;
  align-items: center;

  gap: 18px;

  padding: 16px;

  margin-bottom: 14px;

  border-radius: 16px;

  background: rgba(255,255,255,0.04);
}

.image {
  width: 90px;
  height: 90px;

  object-fit: cover;

  border-radius: 14px;
}

.info {
  flex: 1;
}

.title {
  font-size: 18px;
  font-weight: 600;
}

.meta {
  margin-top: 8px;
  opacity: 0.7;
}

.price-wrapper {
  text-align: right;
}

.subtotal {
  margin-top: 8px;

  font-size: 20px;
  font-weight: bold;
}

.summary {
  padding: 22px;

  border-radius: 18px;

  background: rgba(255,255,255,0.04);
}

.summary-row {
  display: flex;
  justify-content: space-between;

  margin-bottom: 16px;

  font-size: 18px;
}

.total {
  margin-top: 20px;

  font-size: 24px;
  font-weight: bold;
}

.actions {
  margin-top: 40px;

  display: flex;
  justify-content: space-between;
}
</style>