<template>
  <div class="page">
    <n-card class="card">

      <div class="header">
        <div>
          <h1>Administracija proizvoda</h1>
        </div>

        <n-button
          type="primary"
          size="large"
          @click="openCreateModal"
        >
          + Novi proizvod
        </n-button>
      </div>

      <n-data-table
        :columns="columns"
        :data="products"
        :loading="loading"
        :bordered="false"
        striped
      />

      <n-modal
        v-model:show="showModal"
        preset="card"
        :title="editingProduct ? 'Izmena proizvoda' : 'Novi proizvod'"
        style="width: 650px"
      >

        <n-form
          ref="formRef"
          :model="form"
          :rules="rules"
          label-placement="top"
        >

          <n-grid :cols="2" :x-gap="16">

            <n-form-item-gi
              label="Naziv"
              path="name"
            >
              <n-input
                v-model:value="form.name"
                placeholder="Naziv"
              />
            </n-form-item-gi>

            <n-form-item-gi
              label="Cena"
              path="price"
            >
              <n-input-number
                v-model:value="form.price"
                :min="1"
                style="width: 100%"
              />
            </n-form-item-gi>

          </n-grid>

          <n-form-item
            label="Slika (link)"
            path="image"
          >
            <n-input
              v-model:value="form.image"
              placeholder="https://..."
            />
          </n-form-item>

          <n-form-item
            label="Zalihe"
            path="stock"
          >
            <n-input-number
              v-model:value="form.stock"
            />
          </n-form-item>

          <div class="preview-wrapper">
            <img
              :src="form.image || fallbackImage"
              class="preview-image"
            />
          </div>

        </n-form>

        <template #footer>
          <div class="modal-actions">

            <n-button @click="showModal = false">
              Cancel
            </n-button>

            <n-button
              type="primary"
              :loading="saveLoading"
              @click="saveProduct"
            >
              Sacuvajte
            </n-button>

          </div>
        </template>

      </n-modal>

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
import { deleteProductApi, fetchProductsApi, postProductApi, updateProductApi } from "../api/products.api";

const message = useMessage();
const dialog = useDialog();

const loading = ref(false);
const saveLoading = ref(false);

const products = ref([]);

const search = ref("");
const sort = ref("newest");

const showModal = ref(false);
const editingProduct = ref(null);

const formRef = ref(null);

const fallbackImage =
  "https://picsum.photos/300/300";

const form = ref({
  name: "",
  stock: 0,
  price: 1,
  image: "",
});

const rules = {
  name: [
    {
      required: true,
      message: "Required",
      trigger: "blur",
    },
  ],

  price: [
    {
      required: true,
      type: "number",
      message: "Required",
      trigger: "blur",
    },
  ],

  stock: [
    {
      required: true,
      type: "number",
      message: "Required",
      trigger: "blur",
    },
  ],

  image: [
    {
      required: true,
      message: "Required",
      trigger: "blur",
    },
  ],
};

const fetchProducts = async () => {
  loading.value = true;

  try {
    const res = await fetchProductsApi();

    products.value = res.data;
  } catch (e) {
    message.error(
      e.response?.data?.message ||
      "Failed to load products"
    );
  } finally {
    loading.value = false;
  }
};

const openCreateModal = () => {
  editingProduct.value = null;

  form.value = {
    title: "",
    stock: 0,
    price: 1,
    image: "https://picsum.photos/400/400",
  };

  showModal.value = true;
};

const openEditModal = (product) => {
  editingProduct.value = product;

  form.value = {
    name: product.name,
    stock: product.stock,
    price: product.price,
    image: product.image,
  };

  showModal.value = true;
};

const saveProduct = async () => {
  try {
    await formRef.value?.validate();

    saveLoading.value = true;

    if (editingProduct.value) {
      await updateProductApi(editingProduct.value.id, form.value);
    } else {
      await postProductApi(form.value);
    }

    message.success("Uspesno sacuvano");

    showModal.value = false;

    fetchProducts();
  } catch (e) {
    message.error(
      e.response?.data?.message ||
      "Neocekivana greska. Pokusajte ponovo."
    );
  } finally {
    saveLoading.value = false;
  }
};

const removeProduct = (product) => {
  dialog.warning({
    title: "Brisanje proizvoda",

    content:
      `Potvrdite brisanje proizvoda - ${product.name}?`,

    positiveText: "Potvrda",
    negativeText: "Odustanak",

    onPositiveClick: async () => {
      try {
        await deleteProductApi(product.id);

        message.success("Uspesno brisanje");

        fetchProducts();
      } catch (e) {
        message.error(
          e.response?.data?.message ||
          "Neocekivana greska se dogodila"
        );
      }
    },
  });
};

const columns = [
  {
    title: "Slika",
    key: "image",

    render(row) {
      return h(NImage, {
        src: row.image,
        width: 70,
        height: 70,
        style: {
          borderRadius: "12px",
          objectFit: "cover",
        },
      });
    },
  },

  {
    title: "Naziv",
    key: "name",
  },

  {
    title: "Cena",
    key: "price",

    render(row) {
      return `${row.price.toFixed(2)} din.`;
    },
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
              secondary: true,
              onClick: () => openEditModal(row),
            },
            { default: () => "Izmena" }
          ),

          h(
            NButton,
            {
              type: "error",
              ghost: true,
              onClick: () => removeProduct(row),
            },
            { default: () => "Brisanje" }
          ),
        ]
      );
    },
  },
];

onMounted(() => {
  fetchProducts();
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

.filters {
  display: flex;
  gap: 16px;

  margin-bottom: 24px;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.preview-wrapper {
  display: flex;
  justify-content: center;

  margin-top: 20px;
}

.preview-image {
  width: 220px;
  height: 220px;

  object-fit: cover;

  border-radius: 20px;

}
</style>