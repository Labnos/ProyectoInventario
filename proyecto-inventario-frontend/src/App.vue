<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { RouterView } from 'vue-router';

// Importa el componente del menú lateral que crearemos
import Sidebar from './components/layout/Sidebar.vue';
import NotificationWrapper from './components/shared/NotificationWrapper.vue'; // Importa el wrapper

const route = useRoute();

// Esta función computada revisa si la ruta actual es la de login
const isLoginPage = computed(() => route.name === 'Login');
</script>

<template>
  <div v-if="isLoginPage">
    <RouterView />
  </div>
  
  <div v-else class="flex h-screen bg-background-light">
    <Sidebar />
    <main class="flex-1 overflow-y-auto">
      <RouterView />
    </main>
    <NotificationWrapper /> <div v-if="isLoginPage"> ... </div>
  </div>
</template>