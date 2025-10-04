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
  <div class="flex items-center justify-center space-x-2 mt-4">
    <button @click="goToPage(currentPage - 1)" :disabled="currentPage === 1" class="px-3 py-1 border rounded-md disabled:opacity-50">
      Anterior
    </button>
    <span class="text-sm">Página {{ currentPage }} de {{ totalPages }}</span>
    <button @click="goToPage(currentPage + 1)" :disabled="currentPage === totalPages" class="px-3 py-1 border rounded-md disabled:opacity-50">
      Siguiente
    </button>
  </div>
</template>