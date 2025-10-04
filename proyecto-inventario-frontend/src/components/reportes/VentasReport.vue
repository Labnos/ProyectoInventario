<script setup lang="ts">
import { ref, onMounted } from 'vue';
import api from '../../services/api';

const props = defineProps<{
  startDate: string;
  endDate: string;
}>();

// --- Interfaces y Estado ---
interface VentaReporte {
  id: number;
  fecha: string;
  clienteNombre: string;
  total: number;
  // Añade más campos si tu endpoint los devuelve
}

const ventas = ref<VentaReporte[]>([]);
const totalRevenue = ref(0);
const isLoading = ref(true);
const error = ref<string | null>(null);

// --- Lógica de Datos ---
onMounted(async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const response = await api.get('/api/reportes/ventas-entre-fechas', {
      params: {
        inicio: props.startDate,
        fin: props.endDate,
      }
    });
    ventas.value = response.data;
    // Calculamos el total de ingresos
    totalRevenue.value = ventas.value.reduce((sum, venta) => sum + venta.total, 0);
  } catch (err) {
    error.value = 'No se pudo generar el reporte de ventas.';
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <div v-if="isLoading" class="text-center py-10">Generando reporte...</div>
  <div v-else-if="error" class="text-center py-10 text-accent-red">{{ error }}</div>
  
  <div v-else class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200">
    <h2 class="text-xl font-bold font-serif text-text-dark mb-4">
      Reporte de Ventas ({{ new Date(startDate).toLocaleDateString() }} - {{ new Date(endDate).toLocaleDateString() }})
    </h2>

    <div class="grid grid-cols-2 gap-4 mb-6">
      <div class="bg-gray-100 p-4 rounded-md">
        <h3 class="text-sm text-gray-600">Ingresos Totales</h3>
        <p class="text-2xl font-bold text-text-dark">Q {{ totalRevenue.toFixed(2) }}</p>
      </div>
      <div class="bg-gray-100 p-4 rounded-md">
        <h3 class="text-sm text-gray-600">Número de Ventas</h3>
        <p class="text-2xl font-bold text-text-dark">{{ ventas.length }}</p>
      </div>
    </div>

    <table class="w-full text-left">
      <thead class="border-b">
        <tr>
          <th class="py-2 px-3 text-sm font-semibold">ID Venta</th>
          <th class="py-2 px-3 text-sm font-semibold">Fecha</th>
          <th class="py-2 px-3 text-sm font-semibold">Cliente</th>
          <th class="py-2 px-3 text-sm font-semibold">Total</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="venta in ventas" :key="venta.id" class="border-b hover:bg-gray-50">
          <td class="py-2 px-3">#{{ venta.id }}</td>
          <td class="py-2 px-3">{{ new Date(venta.fecha).toLocaleDateString() }}</td>
          <td class="py-2 px-3">{{ venta.clienteNombre }}</td>
          <td class="py-2 px-3 font-medium">Q {{ venta.total.toFixed(2) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>