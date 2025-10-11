<template>
  <div>
    <h2 class="text-xl sm:text-2xl font-semibold mb-4 text-gray-700">Reporte de Ganancias por Producto</h2>
    <div class="grid grid-cols-1 sm:grid-cols-3 gap-4 mb-6 items-end">
      <div>
        <label for="inicio" class="block text-sm font-medium text-gray-700">Fecha de Inicio</label>
        <input type="date" v-model="fechaInicio" id="inicio" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-2">
      </div>
      <div>
        <label for="fin" class="block text-sm font-medium text-gray-700">Fecha de Fin</label>
        <input type="date" v-model="fechaFin" id="fin" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm p-2">
      </div>
      <button @click="generarReporte" class="bg-indigo-600 text-white px-4 py-2 rounded-md shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 w-full sm:w-auto">
        Generar Reporte
      </button>
    </div>

    <div v-if="isLoading" class="text-center py-10">
      <p class="text-gray-500">Cargando datos...</p>
    </div>
    <div v-else-if="reporteData.length > 0" class="overflow-x-auto bg-white rounded-lg shadow">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Producto</th>
            <th scope="col" class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase tracking-wider">Cantidad Vendida</th>
            <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Ganancia Total</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="(item, index) in reporteData" :key="index" class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ item.producto }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 text-center">{{ item.cantidadVendida }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 text-right">{{ formatCurrency(item.gananciaTotal) }}</td>
          </tr>
        </tbody>
        <tfoot class="bg-gray-100">
            <tr class="font-bold text-gray-700">
                <td class="px-6 py-3 text-left text-sm" colspan="2">Ganancia Total General</td>
                <td class="px-6 py-3 text-right text-sm">{{ formatCurrency(gananciaTotalGeneral) }}</td>
            </tr>
        </tfoot>
      </table>
    </div>
    <div v-else class="text-center py-10 border-2 border-dashed border-gray-300 rounded-lg">
      <p class="text-gray-500">No hay datos para mostrar. Por favor, selecciona un rango de fechas.</p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import api from '@/services/api';

const fechaInicio = ref('');
const fechaFin = ref('');
const reporteData = ref([]);
const isLoading = ref(false);

const generarReporte = async () => {
  if (fechaInicio.value && fechaFin.value) {
    isLoading.value = true;
    try {
      const response = await api.get('/reportes/ganancia-por-producto', {
        params: {
          inicio: fechaInicio.value,
          fin: fechaFin.value,
        },
      });
      reporteData.value = response.data;
    } catch (error) {
      console.error("Error al generar el reporte de ganancias:", error);
      alert("No se pudo cargar el reporte de ganancias.");
    } finally {
      isLoading.value = false;
    }
  }
};

const gananciaTotalGeneral = computed(() => {
    return reporteData.value.reduce((total, item) => total + item.gananciaTotal, 0);
});

const formatCurrency = (value) => {
  return new Intl.NumberFormat('es-GT', { style: 'currency', currency: 'GTQ' }).format(value);
};
</script>