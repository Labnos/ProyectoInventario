<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import api from '@/services/api';
import { useNotifications } from '@/composables/useNotifications';

const email = ref('');
const password = ref('');
const isLoading = ref(false);
const router = useRouter();
const { addNotification } = useNotifications();

const handleLogin = async () => {
  isLoading.value = true;
  try {
    const response = await api.post('/api/auth/login', {
      username: email.value,
      password: password.value,
    });

    localStorage.setItem('token', response.data.token);
    api.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
    router.push('/dashboard');
  } catch (error) {
    addNotification('Credenciales incorrectas. Por favor, inténtalo de nuevo.', 'error');
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="screen">
    <div class="login-box">
      <h2>COMERCIALES EMILIAS</h2>
      <p class="subtitle">Sistema de Gestión Inventario</p>

      <form @submit.prevent="handleLogin">
        <div class="input-group">
          <i class="fas fa-envelope icon"></i>
          <input
            type="email"
            v-model="email"
            placeholder="Correo Electrónico"
            required
          />
        </div>

        <div class="input-group">
          <i class="fas fa-lock icon"></i>
          <input
            type="password"
            v-model="password"
            placeholder="Contraseña"
            required
          />
          <a href="#" class="forgot">¿Olvidaste tu contraseña?</a>
        </div>

        <button :disabled="isLoading">
          {{ isLoading ? 'Ingresando...' : 'INGRESAR AL SISTEMA' }}
        </button>
      </form>
    </div>
  </div>
</template>

<style scoped>
@import url('https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css');

.screen {
  background: linear-gradient(to bottom right, #FDF0E1, #FFE5CC);
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.login-box {
  background: white;
  border-radius: 1rem;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  max-width: 400px;
  width: 100%;
  padding: 2rem;
  border-top: 4px solid #FF8C00;
  position: relative;
}

.login-box::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 4px;
  background: #FF8C00;
  z-index: -1;
}

.login-box::before {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100%;
  height: 2px;
  background: #F28500;
  transform: translateY(8px);
  z-index: -1;
}

.logo {
  background-color: #FF8C00;
  color: white;
  font-weight: bold;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  display: inline-block;
  margin-bottom: 1rem;
  text-align: center;
}

h2 {
  text-align: center;
  color: #1A237E;
  font-size: 1.75rem;
  margin-top: 0.5rem;
}

.subtitle {
  text-align: center;
  color: #616161;
  font-size: 0.875rem;
  margin-bottom: 2rem;
}

.input-group {
  position: relative;
  margin-bottom: 1.5rem;
}

.icon {
  position: absolute;
  top: 50%;
  left: 0.75rem;
  transform: translateY(-50%);
  color: #FF8C00;
}

input {
  width: 100%;
  padding: 0.75rem 0.75rem 0.75rem 2.5rem;
  border: 1px solid #ccc;
  border-radius: 8px;
  font-size: 1rem;
  box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.1);
}

.forgot {
  display: block;
  text-align: right;
  margin-top: 0.5rem;
  color: #FF8C00;
  font-weight: 600;
  font-size: 0.875rem;
  text-decoration: none;
}

.forgot:hover {
  color: #F28500;
}

button {
  width: 100%;
  padding: 0.75rem;
  border: none;
  border-radius: 9999px;
  font-size: 1rem;
  font-weight: bold;
  color: white;
  background: linear-gradient(to right, #FF8C00, #F28500);
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  cursor: pointer;
  transition: background 0.3s ease;
}

button:hover {
  background: linear-gradient(to right, #F28500, #FF8C00);
}

button:disabled {
  background-color: #999;
  cursor: not-allowed;
}
</style>