<template>
  <div class="page">
    <n-card class="card">

      <div class="header">
        <div>
          <h1>Administracija porudžbina</h1>
        </div>
      </div>

      <n-data-table
        :columns="columns"
        :data="orders"
        :loading="loading"
        :bordered="false"
        striped
      />

    </n-card>
  </div>
</template>

<script setup>
import { ref, computed, h, onMounted } from "vue";
import {
  NButton,
  NImage,
  useDialog,
  useMessage,
} from "naive-ui";
import { deleteOrderByIdApi, getOrdersAdminApi } from "../api/order.api";
import { useRouter } from "vue-router";

const message = useMessage();
const router = useRouter();

const loading = ref(false);
const saveLoading = ref(false);

const orders = ref([]);

const dialog = useDialog();

const fetchOrders = async () => {
  loading.value = true;

  try {
    const res = await getOrdersAdminApi();

    orders.value = res.data;
  } catch (e) {
    message.error(
      e.response?.data?.message ||
      "Pribavljanje podataka nije ispelo"
    );
  } finally {
    loading.value = false;
  }
};

const removeOrder = (order) => {
  dialog.warning({
    title: "Brisanje porudzbine",

    content:
      `Potvrdite brisanje porudzbine - # ${order.id}?`,

    positiveText: "Potvrda",
    negativeText: "Odustanak",

    onPositiveClick: async () => {
      try {
        await deleteOrderByIdApi(order.id);

        message.success("Uspesno brisanje");

        fetchOrders();
      } catch (e) {
        message.error(
          e.response?.data?.message ||
          "Neočekivana greška se dogodila"
        );
      }
    },
  });
};

const viewDetails = (id) => {
  router.push(`/orders/${id}`);
};

const columns = [
  {
    title: "ID",
    key: "id",
  },
  {
    title: "Kupac",
    key: "user",
  },
  {
    title: "Datum",
    key: "createdAt",
    
    render(row) {
      return `${formatDate(row.createdAt)}`;
    },
  },
  {
    title: "Cena",
    key: "price",

    render(row) {
      return `${row.totalAmount.toFixed(2)} din.`;
    },
  },
  {
    title: "Plaćeno",
    key: "paid",

    render(row) {
      return row.paid ? "DA" : "ne"
    }
  },
  {
    title: "Opcije",
    key: "actions",

    render(row) {
      return h(
        "div",
        {
          style: {
            display: "flex",
            gap: "10px",
          },
        },
        [
          h(
            NButton,
            {
              type: "primary",
              ghost: true,
              onClick: () => viewDetails(row.id),
            },
            { default: () => "Detalji porudzbine" }
          ),
          h(
            NButton,
            {
              type: "error",
              ghost: true,
              disabled: !row.deleteable,
              onClick: () => removeOrder(row),
            },
            { default: () => "Brisanje" }
          ),
        ]
      );
    },
  },
];

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
  max-width: 1300px;
  margin: 0 auto;

  border-radius: 24px;

}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;

  margin-bottom: 24px;
}

.header h1 {
  margin: 0;
  font-size: 36px;
}

.header p {
  margin-top: 8px;
  opacity: 0.7;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

</style>