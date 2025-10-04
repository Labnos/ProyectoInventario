<script setup lang="ts">
import { useNotifications } from '../../composables/useNotifications';
const { notifications } = useNotifications();
</script>

<template>
  <div class="fixed top-5 right-5 z-50 space-y-3">
    <div
      v-for="notification in notifications"
      :key="notification.id"
      class="px-4 py-3 rounded-md shadow-lg text-white-card"
      :class="{
        'bg-accent-green': notification.type === 'success',
        'bg-accent-red': notification.type === 'error',
      }"
    >
      {{ notification.message }}
    </div>
  </div>
</template>