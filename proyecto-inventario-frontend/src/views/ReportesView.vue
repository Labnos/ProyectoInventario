<template>
  <div class="p-4 sm:p-6 bg-gray-100 min-h-screen">
    <h1 class="text-2xl sm:text-3xl font-bold mb-6 text-gray-800">Módulo de Reportes</h1>

    <div class="bg-white p-4 sm:p-6 rounded-lg shadow-md">
      <div class="border-b border-gray-200">
        <nav class="-mb-px flex space-x-4 sm:space-x-8" aria-label="Tabs">
          <button
            @click="activeTab = 'ventas'"
            :class="[
              'whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm transition-colors',
              activeTab === 'ventas'
                ? 'border-indigo-500 text-indigo-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Ventas
          </button>
          <button
            @click="activeTab = 'ganancias'"
            :class="[
              'whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm transition-colors',
              activeTab === 'ganancias'
                ? 'border-indigo-500 text-indigo-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Ganancias
          </button>
          <button
            @click="activeTab = 'inventario'"
            :class="[
              'whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm transition-colors',
              activeTab === 'inventario'
                ? 'border-indigo-500 text-indigo-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
            ]"
          >
            Inventario
          </button>
        </nav>
      </div>

      <div class="mt-6">
        <VentasReport v-if="activeTab === 'ventas'" />
        <GananciasReport v-if="activeTab === 'ganancias'" />
        <InventarioReport v-if="activeTab === 'inventario'" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import VentasReport from '@/components/reportes/VentasReport.vue';
import GananciasReport from '@/components/reportes/GananciasReport.vue';
import InventarioReport from '@/components/reportes/InventarioReport.vue';

const activeTab = ref('ventas');
</script>