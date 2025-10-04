<script setup lang="ts">
import { ref } from 'vue';
import api from '../services/api';
import { useRouter } from 'vue-router';

// --- Estado del Componente ---
const username = ref('');
const password = ref('');
const errorMessage = ref('');
const isLoading = ref(false);

const router = useRouter();

// --- Lógica de Inicio de Sesión ---
const handleLogin = async () => {
  if (!username.value || !password.value) {
    errorMessage.value = 'Por favor, ingresa tu usuario y contraseña.';
    return;
  }

  isLoading.value = true;
  errorMessage.value = '';

  try {
    const response = await api.post('/api/login', {
      username: username.value,
      password: password.value,
    });
    
    // Guardar el token y redirigir
    localStorage.setItem('token', response.data.token);
    router.push('/dashboard');

  } catch (error) {
    errorMessage.value = 'Usuario o contraseña incorrectos. Inténtalo de nuevo.';
    console.error('Error de autenticación:', error);
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="w-full max-w-md p-8 space-y-8 bg-white rounded-lg shadow-md">
      
      <div class="text-center">
        <div class="w-24 h-24 mx-auto mb-4 bg-gray-200 rounded-full">
          </div>
        <h1 class="text-3xl font-bold text-gray-800" style="font-family: 'Roboto Slab', serif;">
          Comerciales Emilias
        </h1>
        <p class="mt-2 text-gray-600">
          Sistema de Gestión de Inventario
        </p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        <div>
          <label for="username" class="text-sm font-medium text-gray-700">Usuario</label>
          <input
            id="username"
            v-model="username"
            type="text"
            required
            class="w-full px-3 py-2 mt-1 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
            placeholder="tu-usuario"
          />
        </div>

        <div>
          <label for="password" class="text-sm font-medium text-gray-700">Contraseña</label>
          <input
            id="password"
            v-model="password"
            type="password"
            required
            class="w-full px-3 py-2 mt-1 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
            placeholder="••••••••"
          />
        </div>

        <div v-if="errorMessage" class="p-3 text-sm text-center text-red-700 bg-red-100 rounded-md">
          {{ errorMessage }}
        </div>

        <div>
          <button
            type="submit"
            :disabled="isLoading"
            class="w-full py-2 text-white bg-blue-600 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:bg-blue-300 disabled:cursor-not-allowed"
          >
            <span v-if="isLoading">Ingresando...</span>
            <span v-else>Ingresar al Sistema</span>
          </button>
        </div>
        
        <div class="text-sm text-center">
          <a href="#" class="font-medium text-blue-600 hover:text-blue-500">
            ¿Olvidaste tu contraseña?
          </a>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
/* Estilos adicionales con la fuente recomendada */
body {
  font-family: 'Roboto', sans-serif;
}
</style>