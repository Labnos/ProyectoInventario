<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { RouterView } from 'vue-router';
import Sidebar from './components/layout/Sidebar.vue';
import NotificationWrapper from './components/shared/NotificationWrapper.vue';
</script>

<template>
  <NotificationWrapper />
  
  <div v-if="computed(() => useRoute().name === 'Login').value">
    <RouterView />
  </div>
  
  <div v-else class="flex h-screen bg-background-light">
    <Sidebar />
    <main class="flex-1 overflow-y-auto">
      <RouterView />
    </main>
  </div>
</template>