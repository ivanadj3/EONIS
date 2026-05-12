<template>
  <div class="page">

    <n-card class="card">

      <div class="icon">
        ✅
      </div>

      <h1 v-if="!hideOrderCreatedText">Porudzbina je kreirana!</h1>

      <p class="text" v-if="!hideOrderCreatedText">
        Vasa porudzbina je uspesno sacuvana.
      </p>

      <p class="text">
        Klikom na dugme, bicete preusmereni na Stripe gateway za placanje.
      </p>

      <div class="note">
        Placanje je moguce izvrsiti i na stranici za pregled porudzbina. 
        Ukoliko ne izvrsite placanje u roku od 3 dana, administratori ce obrisati Vasu porudzbinu.
      </div>

      <div class="summary">
        <div>
          <span>ID porudzbine</span>
          <strong># {{ orderId }}</strong>
        </div>

        <div>
          <span>Ukupno</span>
          <strong>{{ total }}</strong>
        </div>
      </div>

      <n-alert
        type="info"
        style="margin-top: 20px"
      >
        Stripe postuje sve politike privatnosti podataka.
      </n-alert>

      <div class="actions">

        <n-button secondary @click="goBack">
          Nazad
        </n-button>

        <n-button
          type="primary"
          size="large"
          :disabled="loading"
          @click="goToStripe"
        >
          Nastavite na placanje
        </n-button>

      </div>

    </n-card>

  </div>
</template>

<script setup>
    import { useRoute } from "vue-router";
    import { createCheckoutSessionApi } from "../api/stripe.api";
    import { useMessage } from "naive-ui";
    import { ref } from "vue";

    const route = useRoute();
    const message = useMessage();

    const orderId = route.params.id;
    const hideOrderCreatedText = route.query.hideCreated;

     const loading = ref(false);

    // from backend ideally
    const total = "124.99";

    const goBack = () => {
        history.back();
    };

    const goToStripe = async () => {
        try {
            loading.value = true;
            const res = await createCheckoutSessionApi({
                orderId
            });

            const url = res.sessionUrl;
            
            window.location.href = url;
        } catch (e) {
            message.error(e.response?.data?.message || "Neocekivana greska");
        } finally {
            loading.value = false;
        }
    }

</script>

<style scoped>
    .page {
        min-height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;

    }

    .card {
        width: 550px;
        padding: 32px;
        border-radius: 20px;
        text-align: center;

        backdrop-filter: blur(10px);
    }

    .icon {
        font-size: 64px;
        margin-bottom: 20px;
    }

    .text {
        opacity: 0.75;
        margin-top: 10px;
    }

    .note {
        padding: 5px;
        background-color: rgb(214, 176, 4);
    }

    .summary {
        margin-top: 24px;

        display: flex;
        justify-content: space-between;

        padding: 20px;

        border-radius: 12px;

    }

    .actions {
        margin-top: 28px;

        display: flex;
        justify-content: space-between;
    }
</style>
