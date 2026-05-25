<template>
  <div class="page">
    <n-card class="card">

      <div class="header">
        <div>
          <h1>Moje porudzbine</h1>
          <p>Pregled istorije porudžbina</p>
        </div>

        <n-button @click="fetchOrders">
          Učitajte ponovo
        </n-button>
      </div>

      <n-spin :show="loading">

        <n-empty
          v-if="orders.length === 0 && !loading"
          description="Nema pronadjenih porudžbina"
        />

        <div
          v-for="order in orders"
          :key="order.id"
          class="order"
        >

          <div class="order-top">

            <div>
              <div class="order-id">
                Porudžbina #{{ order.id }}
              </div>

              <div class="date">
                {{ formatDate(order.createdAt) }}
              </div>
            </div>

            <div class="right">

              <n-tag
                :type="order.paid ? 'success' : 'warning'"
                size="large"
              >
                {{ order.paid ? 'Plaćena' : 'Nije plaćena' }}
              </n-tag>

              <div class="total">
                {{ order.totalAmount.toFixed(2) }} din.
              </div>

            </div>

          </div>

          <div class="items">

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
                  Količina: {{ item.quantity }}
                </div>
              </div>

              <div class="price">
                {{ item.totalPrice.toFixed(2) }} din.
              </div>

            </div>

          </div>

          <div class="actions">

            <n-button
              secondary
              @click="viewDetails(order.id)"
            >
              Detalji porudzbine
            </n-button>

            <n-button
              v-if="!order.paid"
              type="primary"
              :loading="payingOrderId === order.id"
              @click="pay(order.id)"
            >
              Platite
            </n-button>

          </div>

        </div>

      </n-spin>

    </n-card>
  </div>
</template>

<script setup>
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import { useMessage } from "naive-ui";
    import { getOrdersApi } from "../api/order.api";

    const router = useRouter();
    const message = useMessage();

    const loading = ref(false);
    const payingOrderId = ref(null);

    const orders = ref([]);

    const fetchOrders = async () => {
        loading.value = true;

        try {
            const res = await getOrdersApi();

            orders.value = res.data;
        } catch (e) {
            message.error(
            e.response?.data?.message ||
            "Neuspešno učitavanje porudžbina"
            );
        } finally {
            loading.value = false;
        }
    };

    const pay = async (orderId) => {
        router.push("/pre-checkout/" + orderId + "?hideCreated=true");
    };

    const viewDetails = (id) => {
      router.push(`/orders/${id}`);
    };

    const formatDate = (date) => {
      return new Date(date).toLocaleString("sr-RS")
    };

    onMounted(() => {
      fetchOrders();
    });
</script>

<style scoped>
    .page {
        min-height: 100vh;
        padding: 32px;
    }

    .card {
        max-width: 1100px;
        margin: 0 auto;

        border-radius: 24px;

    }

    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;

        margin-bottom: 28px;
    }

    .header h1 {
        margin: 0;
        font-size: 34px;
    }

    .header p {
        opacity: 0.7;
        margin-top: 6px;
    }

    .order {
        padding: 24px;

        border-radius: 18px;

        margin-bottom: 20px;
        border: 1px solid rgba(255,255,255,0.06);

        background-color: antiquewhite;
    }

    .order-top {
        display: flex;
        justify-content: space-between;
        align-items: center;

        margin-bottom: 20px;
    }

    .order-id {
        font-size: 20px;
        font-weight: bold;
    }

    .date {
        margin-top: 6px;
        opacity: 0.7;
    }

    .right {
        display: flex;
        align-items: center;
        gap: 16px;
    }

    .total {
        font-size: 24px;
        font-weight: bold;
    }

    .items {
        display: flex;
        flex-direction: column;
        gap: 12px;
    }

    .item {
        display: flex;
        align-items: center;

        padding: 14px;

        border-radius: 14px;
        background-color: beige;
    }

    .image {
        width: 70px;
        height: 70px;

        object-fit: cover;

        border-radius: 12px;

        margin-right: 16px;
    }

    .info {
        flex: 1;
    }

    .title {
        font-weight: 600;
        font-size: 16px;
    }

    .meta {
        margin-top: 6px;
        opacity: 0.7;
    }

    .price {
        font-weight: bold;
        font-size: 18px;
    }

    .actions {
        margin-top: 22px;

        display: flex;
        justify-content: flex-end;
        gap: 12px;
    }
</style>
