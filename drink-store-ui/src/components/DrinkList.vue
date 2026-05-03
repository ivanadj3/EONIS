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
            <n-grid-item v-for="p in products" :key="p.id">
            <n-card class="card" hoverable>
                <img :src="p.image" class="img" />
                <div class="info">
                <div class="name">{{ p.name }}</div>
                <div class="price"> {{ p.price }} din.</div>
                <div><n-button type="primary" @click="routeToProductDetails(p.id)"">Vise detalja...</n-button></div>
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
  import { ref, computed, watch } from "vue";
  import { NInput, NSelect, NGrid, NGridItem, NCard, NPagination, NButton } from "naive-ui";
  import { fetchProductsApi } from "../api/products.api";
  import { useRouter } from "vue-router";

  const router = useRouter();

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
  const total = ref(0);

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

      products.value = res.data;
      total.value = res.total;
      page.value = res.page;
      pageSize.value = res.pageSize;
    } catch (e) {
      error.value = e;
    } finally {
      loading.value = false;
    }
  };

  const pageCount = computed(() =>
    Math.ceil(total.value / pageSize.value)
  );

  console.log(pageCount.value)

  function onPageSizeChange(size) {
    pageSize.value = size;
    page.value = 1;
  }

  const routeToProductDetails = (productId) => {
      router.push(`/drinks/${productId}`);
  }

  watch([search, sort, page, pageSize], () => {
    fetchProducts();
  }, { deep: true });

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
