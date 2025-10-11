<script setup lang="ts">
import { useRoute } from 'vue-router';
import { RouterView } from 'vue-router';
import Sidebar from './components/layout/Sidebar.vue';
import NotificationWrapper from './components/shared/NotificationWrapper.vue';

const route = useRoute();
</script>

<template>
  <NotificationWrapper />
  
  <RouterView v-if="route.name === 'Login'" />
  
  <div v-else class="flex h-screen bg-gray-100">
    <Sidebar />
    <main class="flex-1 overflow-y-auto">
      <RouterView />
    </main>
  </div>
</template>