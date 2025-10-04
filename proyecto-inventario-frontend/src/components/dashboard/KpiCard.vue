<script setup lang="ts">
defineProps({
  title: String,
  value: [String, Number],
  description: String,
});
</script>

<template>
  <div class="bg-white p-6 rounded-lg shadow-md">
    <h3 class="text-sm font-medium text-gray-500">{{ title }}</h3>
    <p class="mt-2 text-3xl font-bold text-gray-900">{{ value }}</p>
    <p class="mt-1 text-xs text-gray-500">{{ description }}</p>
  </div>
</template>