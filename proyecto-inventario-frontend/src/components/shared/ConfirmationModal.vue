<script setup lang="ts">
defineProps<{
  isOpen: boolean;
  title: string;
  message: string;
}>();

const emit = defineEmits(['close', 'confirm']);
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 bg-black bg-opacity-60 flex justify-center items-center z-50">
    <div class="bg-white-card p-8 rounded-lg shadow-xl w-full max-w-md text-center">
      <h2 class="text-2xl font-bold font-serif text-text-dark mb-4">{{ title }}</h2>
      <p class="text-gray-600 mb-8">{{ message }}</p>
      
      <div class="flex justify-center space-x-4">
        <button @click="$emit('close')" class="bg-gray-200 text-gray-800 font-semibold px-6 py-2 rounded-md hover:bg-gray-300 transition-colors">
          Cancelar
        </button>
        <button @click="$emit('confirm')" class="bg-accent-red text-white-card font-semibold px-6 py-2 rounded-md hover:bg-red-700 transition-colors">
          Sí, Eliminar
        </button>
      </div>
    </div>
  </div>
</template>