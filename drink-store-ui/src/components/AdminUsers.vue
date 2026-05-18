<template>
  <div class="page">
    <n-card class="card">

      <div class="header">
        <div>
          <h1>Administracija korisnika</h1>
        </div>
      </div>

      <n-data-table
        :columns="columns"
        :data="users"
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
import { deleteUserByIdApi, fetchUsersApi } from "../api/users.api";

const message = useMessage();

const loading = ref(false);
const saveLoading = ref(false);

const users = ref([]);

const dialog = useDialog();

const fetchUsers = async () => {
  loading.value = true;

  try {
    const res = await fetchUsersApi();

    users.value = res;
  } catch (e) {
    message.error(
      e.response?.data?.message ||
      "Pribavljanje podataka nije ispelo"
    );
  } finally {
    loading.value = false;
  }
};

const removeUser = (user) => {
  dialog.warning({
    title: "Brisanje korisnika",

    content:
      `Potvrdite brisanje korisnika - ${user.name} ${user.surname}?`,

    positiveText: "Potvrda",
    negativeText: "Odustanak",

    onPositiveClick: async () => {
      try {
        await deleteUserByIdApi(user.id);

        message.success("Uspesno brisanje");

        fetchUsers();
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
    title: "Ime",
    key: "name",
  },
  {
    title: "Prezime",
    key: "surname",
  },
  {
    title: "Adresa",
    key: "address",
  },
  {
    title: "Telefon",
    key: "phone",
  },
  {
    title: "Uloga u sistemu",
    key: "role",
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
              type: "error",
              ghost: true,
              onClick: () => removeUser(row),
            },
            { default: () => "Brisanje" }
          ),
        ]
      );
    },
  },
];

onMounted(() => {
  fetchUsers();
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