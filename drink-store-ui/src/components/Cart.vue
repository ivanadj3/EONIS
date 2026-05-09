<template>
  <div class="page">

    <n-card class="card" title="Pregled korpe">

      <div v-if="items.length === 0" class="empty">
        🛒 Nema stavki
      </div>

      <div v-else>

        <div v-for="item in items" :key="item.id" class="item">

          <img :src="item.image" class="img" />

          <div class="info">
            <div class="title">{{ item.title }}</div>
            <div class="price">$ {{ item.price }}</div>
          </div>

          <div class="qty">
            <n-input-number
              v-model:value="item.quantity"
              :min="1"
              @update:value="updateCart"
            />
          </div>

          <div class="subtotal">
            $ {{ (item.price * item.quantity).toFixed(2) }}
          </div>

          <n-button type="error" @click="remove(item.id)">
            Izbaci iz korpe
          </n-button>

        </div>

        <n-divider />

        <div class="total">
          <span>Ukupan iznos:</span>
          <strong>{{ total.toFixed(2) }}</strong>
        </div>

        <div class="actions">
          <n-button @click="clear">Isprazni korpu</n-button>
          <n-button type="primary" size="large" @click="checkout" :disabled="loading">
            {{ loading ? "Ucitavanje..." : "Placanje" }}
          </n-button>
        </div>

      </div>

    </n-card>

  </div>
</template>

<script setup>
    import { computed, ref } from "vue";
    import { cartItems, clearCart, removeFromCart } from "../store/cart";
    import { makeOrderApi } from "../api/order.api";
    import { useMessage } from "naive-ui";
    import { useRouter } from "vue-router";

    const items = cartItems;
    const loading = ref(false);

    const message = useMessage();
    const router = useRouter();

    const updateCart = () => {
        localStorage.setItem("cart", JSON.stringify(items.value));
    };

    const remove = (id) => {
        removeFromCart(id);
    };

    const clear = () => {
        clearCart();
    };

    const total = computed(() => {
      return items.value.reduce((sum, i) => {
          return sum + i.price * i.quantity;
      }, 0);
    });

    const checkout = async() => {
        loading.value = true;

        try {
            const body = items.value.map(x => ({
              productId: x.id,
              quantity: x.quantity
            }));

            const res = await makeOrderApi({
                items: body
            });

            message.success('Porudzbina je uspesno sacuvana')

            router.push("/pre-checkout/" + res.id);
        } catch (e) {
            console.log(e)
            message.error(e.response?.data?.message || "Neocekivana greska")
        } finally {
            loading.value = false;
        }
    }
</script>

<style scoped>
    .page {
        padding: 24px;
        display: flex;
        justify-content: center;
        min-height: 100vh;
    }

    .card {
        width: 900px;
        border-radius: 16px;
    }

    .item {
        display: flex;
        align-items: center;
        gap: 16px;
        padding: 12px 0;
        border-bottom: 1px solid #eee;
    }

    .img {
        width: 70px;
        height: 70px;
        object-fit: cover;
        border-radius: 8px;
    }

    .info {
        flex: 1;
    }

    .title {
        font-weight: 600;
    }

    .price {
        opacity: 0.7;
    }

    .qty {
        width: 120px;
    }

    .subtotal {
        width: 100px;
        text-align: right;
    }

    .total {
        display: flex;
        justify-content: space-between;
        font-size: 20px;
        margin-top: 16px;
    }

    .actions {
        display: flex;
        justify-content: space-between;
        margin-top: 20px;
    }

    .empty {
        text-align: center;
        padding: 40px;
        font-size: 18px;
        opacity: 0.7;
    }
</style>
