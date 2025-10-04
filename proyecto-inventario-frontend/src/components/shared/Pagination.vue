<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  currentPage: number;
  totalItems: number;
  pageSize: number;
}>();

const emit = defineEmits(['page-changed']);

const totalPages = computed(() => Math.ceil(props.totalItems / props.pageSize));

const goToPage = (page: number) => {
  if (page >= 1 && page <= totalPages.value) {
    emit('page-changed', page);
  }
};
</script>

<template>
  <div class="flex items-center justify-end space-x-4 text-sm mt-6">
    <span class="text-gray-600">
      Mostrando {{ Math.min((currentPage - 1) * pageSize + 1, totalItems) }} - {{ Math.min(currentPage * pageSize, totalItems) }} de {{ totalItems }}
    </span>
    <button @click="goToPage(currentPage - 1)" :disabled="currentPage === 1" class="px-3 py-1 border rounded-md disabled:opacity-50 hover:bg-gray-100 transition-colors">
      Anterior
    </button>
    <button @click="goToPage(currentPage + 1)" :disabled="currentPage === totalPages" class="px-3 py-1 border rounded-md disabled:opacity-50 hover:bg-gray-100 transition-colors">
      Siguiente
    </button>
  </div>
</template>