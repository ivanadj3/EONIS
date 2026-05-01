<template>
  <div class="page">
    <div class="header">
      <div class="controls">
        <n-input v-model:value="search" placeholder="Search products..." clearable />

        <n-select
          v-model:value="sort"
          :options="sortOptions"
          placeholder="Sort"
          style="width: 180px"
        />
      </div>
    </div>

    <n-spin :show="loading">
        <div class="grid">
        <n-grid :cols="4" x-gap="16" y-gap="16" responsive="screen">
            <n-grid-item v-for="p in paginatedProducts" :key="p.id">
            <n-card class="card" hoverable>
                <img :src="p.image" class="img" />
                <div class="info">
                <div class="name">{{ p.title }}</div>
                <div class="price">$ {{ p.price.toFixed(2) }}</div>
                </div>
            </n-card>
            </n-grid-item>
        </n-grid>
        </div>
    </n-spin>

    <div class="footer">
      <n-pagination
        v-model:page="page"
        :page-count="pageCount"
        :page-size="pageSize"
        show-size-picker
        :page-sizes="[8, 12, 16]"
        @update:page-size="onPageSizeChange"
      />
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import { NInput, NSelect, NGrid, NGridItem, NCard, NPagination } from "naive-ui";
import { fetchProductsApi } from "../api/products.api";

const search = ref("");
const sort = ref("none");
const page = ref(1);
const pageSize = ref(8);

const loading = ref(false);
const error = ref(null);

const sortOptions = [
  { label: "None", value: "none" },
  { label: "Price: Low to High", value: "asc" },
  { label: "Price: High to Low", value: "desc" }
];

const products = ref([]);

const filtered = computed(() => {
  let list = products.value;

  if (search.value) {
    list = list.filter(p =>
      p.title.toLowerCase().includes(search.value.toLowerCase())
    );
  }

  if (sort.value === "asc") {
    list = [...list].sort((a, b) => a.price - b.price);
  }
  if (sort.value === "desc") {
    list = [...list].sort((a, b) => b.price - a.price);
  }

  return list;
});

const fetchProducts = async () => {
  loading.value = true;
  error.value = null;

  try {
    const res = await fetchProductsApi({
      search: search.value,
      sort: sort.value,
      page: page.value,
      pageSize: pageSize.value,
    });

    products.value = res;
    // total.value = res.total;
  } catch (e) {
    error.value = e;
  } finally {
    loading.value = false;
  }
};

const pageCount = computed(() =>
  Math.ceil(filtered.value.length / pageSize.value)
);

const paginatedProducts = computed(() => {
  const start = (page.value - 1) * pageSize.value;
  return filtered.value.slice(start, start + pageSize.value);
});

function onPageSizeChange(size) {
  pageSize.value = size;
  page.value = 1;
}

fetchProducts()

</script>

<style scoped>
.page {
  padding: 24px;
  min-height: 100vh;
  color: white;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.title {
  font-size: 28px;
  font-weight: bold;
  letter-spacing: 1px;
}

.controls {
  display: flex;
  gap: 12px;
}

.grid {
  margin-top: 20px;
}

.card {
  border-radius: 16px;
  overflow: hidden;
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
  transition: transform 0.2s ease;
}

.card:hover {
  transform: translateY(-6px);
}

.img {
  width: 100%;
  height: 160px;
  object-fit: cover;
}

.info {
  padding: 10px;
  display: flex;
  justify-content: space-between;
}

.name {
  font-size: 14px;
}

.price {
  font-weight: bold;
  color: #068b02;
}

.footer {
  margin-top: 24px;
  display: flex;
  justify-content: center;
}
</style>
