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
        
        <n-form-item label="Email" path="email">
          <n-input v-model:value="form.email" placeholder="Email" />
        </n-form-item>

        <n-form-item label="Lozinka" path="password">
          <n-input
            type="password"
            v-model:value="form.password"
            placeholder="Lozinka"
          />
        </n-form-item>

        <n-button
          type="primary"
          size="large"
          block
          :disabled="!isFormValid || loading"
          @click="handleLogin"
        >
          {{ loading ? "Učitavanje..." : "Login" }}
        </n-button>

      </n-form>

      <n-alert v-if="error" type="error" style="margin-top: 16px">
        {{ error }}
      </n-alert>

      <div class="signup-link">
        Nemate nalog?
        
        <router-link to="/sign-up" class="link">
            Registrujte se
        </router-link>
      </div>

    </n-card>
  </div>
</template>

<script setup>
    import { ref, computed, watch } from "vue";
    import { useRouter } from "vue-router";
    import { loginApi } from "../api/auth.api";
    import { setUser } from "../store/user";

    const router = useRouter();

    const formRef = ref(null);

    const form = ref({
        email: "",
        password: "",
    });

    const isFormValid = ref(false);

    const loading = ref(false);
    const error = ref("");

    // VALIDATION RULES
    const rules = {
        email: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
            { type: "email", message: "Neispravan format", trigger: "blur" },
        ],
        password: [
            { required: true, message: "Obavezno polje", trigger: "blur" },
            { min: 6, message: "Minimum 6 karaktera", trigger: "blur" },
        ],
    };

    const handleLogin = async () => {
        error.value = "";

        await formRef.value?.validate();

        loading.value = true;

        try {
            const res = await loginApi({
                email: form.value.email,
                password: form.value.password,
            });

            const token = res.token;

            setUser(token);

            // redirect
            router.push("/");
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

    .signup-link {
        margin-top: 16px;
        text-align: center;
    }

    .link {
        margin-left: 6px;
        text-decoration: none;
        font-weight: 600;
    }

  
</style>