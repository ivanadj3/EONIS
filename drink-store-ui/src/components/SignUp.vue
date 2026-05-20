<template>
  <div class="page">
    <n-card class="card">
      
      <div class="logo">🥤 DrinkStore</div>

      <n-form
        ref="formRef"
        :model="form"
        :rules="rules"
        label-placement="top"
      >

        <n-form-item label="Ime" path="name">
          <n-input v-model:value="form.name" placeholder="Ime" />
        </n-form-item>

        <n-form-item label="Prezime" path="surname">
          <n-input v-model:value="form.surname" placeholder="Prezime" />
        </n-form-item>

        <n-form-item label="Adresa" path="address">
          <n-input v-model:value="form.address" placeholder="Adresa" />
        </n-form-item>

        <n-form-item label="Telefon" path="phone">
          <n-input v-model:value="form.phone" placeholder="Telefon" />
        </n-form-item>
        
        <n-form-item label="Email" path="email">
          <n-input v-model:value="form.email" placeholder="Enter email" />
        </n-form-item>

        <n-form-item label="Lozinka" path="password">
          <n-input
            type="password"
            v-model:value="form.password"
            placeholder="Lozinka"
          />
        </n-form-item>

        <n-form-item label="Potvrdi lozinku" path="confirmPassword">
            <n-input
                type="password"
                v-model:value="form.confirmPassword"
                placeholder="Potvrdi lozinku"
            />
        </n-form-item>

        <n-button
          type="primary"
          size="large"
          block
          :disabled="!isFormValid || loading"
          @click="handleSignUp"
        >
          {{ loading ? "Učitavanje..." : "Registracija" }}
        </n-button>

      </n-form>

      <n-alert v-if="error" type="error" style="margin-top: 16px">
        {{ error }}
      </n-alert>

      <div class="login-link">
        Imate nalog?
        
        <router-link to="/login" class="link">
            Ulogujte se
        </router-link>
      </div>

    </n-card>
  </div>
</template>

<script setup>
    import { ref, computed, watch } from "vue";
    import { useRouter } from "vue-router";
    import { signUpApi } from "../api/users.api";
    import { useMessage } from "naive-ui";

    const router = useRouter();

    const formRef = ref(null);

    const form = ref({
        name: "",
        surname: "",
        address: "",
        phone: "",
        email: "",
        password: "",
        confirmPassword: "",
    });

    const isFormValid = ref(false);

    const loading = ref(false);
    const error = ref("");

    const message = useMessage();

    const validateConfirmPassword = (
            rule,
            value
        ) => {
            if (!value) {
                return new Error("Obavezno polje");
            }

            if (value !== form.value.password) {
                return new Error("Lozinke se ne poklapaju");
            }

            return true;
        };

    const rules = {
        name: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
        ],
        surname: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
        ],
        address: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
        ],
        phone: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
        ],
        email: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
            { type: "email", message: "Neispravan format", trigger: "blur" },
        ],
        password: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
            { min: 6, message: "Minimum 6 karaktera", trigger: "blur" },
        ],
        confirmPassword: [
            {
                validator: validateConfirmPassword,
                trigger: ["blur", "input"],
            },
        ],
    };

    const handleSignUp = async () => {
        error.value = "";

        await formRef.value?.validate();

        loading.value = true;

        try {
            const res = await signUpApi({
                name: form.value.name,
                surname: form.value.surname,
                address: form.value.address,
                email: form.value.email,
                password: form.value.password,
                phone: form.value.phone,
            });

            message.success("Uspešna registracija!");

            setTimeout(() => {
                router.push("/");
            }, 1000);
        } catch (e) {
            error.value = e.response?.data?.message || "Login failed";
        } finally {
            loading.value = false;
        }
    };

    const validateForm = async () => {
        try {
            await formRef.value?.validate();
            isFormValid.value = true;
        } catch {
            isFormValid.value = false;
        }
    };

    watch(form, () => {
        validateForm();
    }, { deep: true });
</script>

<style scoped>
    .page {
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .card {
        width: 400px;
        padding: 24px;
        border-radius: 16px;
        background: rgba(255, 255, 255, 0.05);
        backdrop-filter: blur(10px);
    }

    .logo {
        text-align: center;
        font-size: 26px;
        font-weight: bold;
        margin-bottom: 20px;
    }

    .login-link {
        margin-top: 16px;
        text-align: center;
    }

    .link {
        margin-left: 6px;
        text-decoration: none;
        font-weight: 600;
    }
</style>