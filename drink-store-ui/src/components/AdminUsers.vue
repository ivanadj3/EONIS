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

  <n-modal
    v-model:show="resetModal"
    preset="card"
    title="Reset Password"
    style="width: 500px"
  >

  <n-form
    ref="resetFormRef"
    :model="resetForm"
    :rules="resetRules"
    label-placement="top"
  >

    <n-form-item
      label="Nova lozinka"
      path="password"
    >
      <n-input
        v-model:value="resetForm.password"
        type="password"
        placeholder=""
      />
    </n-form-item>

    <n-form-item
      label="Potvrda lozinke"
      path="confirmPassword"
    >
      <n-input
        v-model:value="resetForm.confirmPassword"
        type="password"
        placeholder=""
      />
    </n-form-item>

  </n-form>

  <template #footer>

    <div class="modal-actions">

      <n-button
        @click="resetModal = false"
      >
        Odustanite
      </n-button>

      <n-button
        type="primary"
        :loading="resetLoading"
        @click="resetPassword"
      >
        Promenite lozinku
      </n-button>

    </div>

  </template>

</n-modal>
</template>

<script setup>
import { ref, computed, h, onMounted } from "vue";
import {
  NButton,
  NImage,
  useDialog,
  useMessage,
} from "naive-ui";
import { changePasswordApi, deleteUserByIdApi, fetchUsersApi } from "../api/users.api";

const message = useMessage();

const loading = ref(false);
const saveLoading = ref(false);

const users = ref([]);

const dialog = useDialog();

const resetModal = ref(false);
const selectedUser = ref(null);
const resetLoading = ref(false);
const resetFormRef = ref(null);
const resetForm = ref({
  password: "",
  confirmPassword: "",
});

const resetRules = {
  password: [
    {
      required: true,
      message: "Obavezno polje",
      trigger: "blur",
    },
    {
      min: 6,
      message: "Minimum 6 karaktera",
      trigger: "blur",
    },
  ],

  confirmPassword: [
    {
      required: true,
      message: "Obavezno polje",
      trigger: "blur",
    },

    {
      validator(_, value) {
        return (
          value === resetForm.value.password
        );
      },

      message: "Lozinke se ne poklapaju",

      trigger: ["blur", "input"],
    },
  ],
};

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
          "Neočekivana greška se dogodila"
        );
      }
    },
  });
};

const openChangePasswordModal = (user) => {
  selectedUser.value = user;

  resetForm.value = {
    password: "",
    confirmPassword: "",
  };

  resetModal.value = true;
}

const resetPassword = async () => {

  await resetFormRef.value?.validate();

  resetLoading.value = true;

  try {

    await changePasswordApi({
        userId: selectedUser.value.id,
        password: resetForm.value.password,
      })

    message.success(
      "Lozinka je uspešno promenjena"
    );

    resetModal.value = false;

  } catch (e) {

    message.error(
      e.response?.data?.message ||
      "Promena lozinke nije uspela. Pokušajte ponovo"
    );

  } finally {

    resetLoading.value = false;
  }
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
          h(
            NButton,
            {
              type: "primary",
              ghost: true,
              onClick: () => openChangePasswordModal(row),
            },
            { default: () => "Promena lozinke" }
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