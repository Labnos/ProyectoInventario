<script setup lang="ts">
defineProps({
  title: String,
  value: [String, Number],
  description: String,
  icon: String, 
});
</script>

<template>
  <div class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200 flex items-center space-x-4">
    <div v-if="icon" class="bg-blue-100 p-3 rounded-full">
      <svg v-if="icon === 'cash'" class="w-6 h-6 text-primary-blue" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v.01"></path></svg>
      <svg v-if="icon === 'warning'" class="w-6 h-6 text-yellow-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path></svg>
      <svg v-if="icon === 'chart'" class="w-6 h-6 text-accent-green" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 8v8m-4-5v5m-4-2v2"></path></svg>
      <svg v-if="icon === 'truck'" class="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"></path><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"></path></svg>
    </div>
    <div>
      <h3 class="text-sm font-medium text-gray-500">{{ title }}</h3>
      <p class="mt-1 text-3xl font-semibold text-text-dark">{{ value }}</p>
      <p class="text-xs text-gray-500">{{ description }}</p>
    </div>
  </div>
</template>