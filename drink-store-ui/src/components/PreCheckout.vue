<template>
  <div class="page">

    <n-card class="card">

      <div class="icon">
        ✅
      </div>

      <h1>Porudzbina je kreirana!</h1>

      <p class="text">
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

    const route = useRoute();

    const orderId = route.params.id;

    // from backend ideally
    const total = "124.99";

    const goBack = () => {
    history.back();
    };

    const goToStripe = async () => {

    // call backend to create stripe session
    // then redirect

    window.location.href =
        "https://checkout.stripe.com/...";
    };
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
