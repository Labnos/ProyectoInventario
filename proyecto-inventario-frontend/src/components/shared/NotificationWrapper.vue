<script setup lang="ts">
import { useNotifications } from '../../composables/useNotifications';
const { notifications } = useNotifications();
</script>

<template>
  <div class="fixed top-5 right-5 z-50 space-y-3 w-80">
    <div
      v-for="notification in notifications"
      :key="notification.id"
      class="px-4 py-3 rounded-md shadow-lg text-white-card animate-fade-in-out"
      :class="{
        'bg-accent-green': notification.type === 'success',
        'bg-accent-red': notification.type === 'error',
      }"
    >
      {{ notification.message }}
    </div>
  </div>
</template>

<style scoped>
@keyframes fade-in-out {
  0% { opacity: 0; transform: translateX(100%); }
  15% { opacity: 1; transform: translateX(0); }
  85% { opacity: 1; transform: translateX(0); }
  100% { opacity: 0; transform: translateX(100%); }
}
.animate-fade-in-out {
  animation: fade-in-out 3s ease-in-out forwards;
}
</style>