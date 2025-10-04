<script setup lang="ts">
import { ref } from 'vue';
// Importa los componentes que mostrarán cada reporte
import VentasReport from '../components/reportes/VentasReport.vue';

// --- Estado del Componente ---
const reportType = ref<'ventas' | 'inventario' | 'vendedores'>('ventas');
const startDate = ref(new Date().toISOString().split('T')[0]);
const endDate = ref(new Date().toISOString().split('T')[0]);
const generatedReportKey = ref(0); // Para forzar la recarga del componente hijo

const generateReport = () => {
  // Esta clave cambia cada vez que se genera un reporte,
  // forzando al componente hijo a recargar sus datos con las nuevas fechas.
  generatedReportKey.value++;
};
</script>

<template>
  <div class="p-4 md:p-8 font-sans">
    <header class="mb-8">
      <h1 class="text-3xl font-bold text-text-dark font-serif">Reportes Detallados</h1>
      <p class="text-gray-500">Analiza el rendimiento de tu negocio.</p>
    </header>

    <div class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200 mb-8 flex flex-wrap gap-4 items-center">
      <div>
        <label class="block text-sm font-medium text-gray-700">Tipo de Reporte</label>
        <select v-model="reportType" class="mt-1 border border-gray-300 rounded-md p-2">
          <option value="ventas">Reporte de Ventas</option>
          <option value="inventario" disabled>Reporte de Inventario (Próximamente)</option>
          <option value,="vendedores" disabled>Reporte de Vendedores (Próximamente)</option>
        </select>
      </div>

      <div>
        <label class="block text-sm font-medium text-gray-700">Desde</label>
        <input v-model="startDate" type="date" class="mt-1 border border-gray-300 rounded-md p-2" />
      </div>
      <div>
        <label class="block text-sm font-medium text-gray-700">Hasta</label>
        <input v-model="endDate" type="date" class="mt-1 border border-gray-300 rounded-md p-2" />
      </div>

      <div class="self-end">
        <button @click="generateReport" class="bg-primary-blue text-white-card font-semibold py-2 px-4 rounded-md hover:bg-blue-700 transition-colors">
          Generar Reporte
        </button>
      </div>
    </div>

    <div>
      <VentasReport 
        v-if="reportType === 'ventas'" 
        :start-date="startDate" 
        :end-date="endDate"
        :key="generatedReportKey"
      />
      </div>
  </div>
</template>