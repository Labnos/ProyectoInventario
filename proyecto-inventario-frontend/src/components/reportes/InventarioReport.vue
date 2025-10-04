<template>
  <div class="p-4 sm:p-6 bg-gray-100 min-h-screen">
    <h1 class="text-2xl sm:text-3xl font-bold mb-6 text-gray-800">Historial de Ventas</h1>

    <div class="bg-white p-4 sm:p-6 rounded-lg shadow-md">
      <div class="flex space-x-2 border-b border-gray-200 pb-4 mb-4">
        <button
          @click="setPeriod('hoy')"
          :class="['px-4 py-2 text-sm font-medium rounded-md transition-colors', activePeriod === 'hoy' ? 'bg-indigo-600 text-white' : 'bg-gray-200 text-gray-700 hover:bg-gray-300']"
        >
          Hoy
        </button>
        <button
          @click="setPeriod('semana')"
          :class="['px-4 py-2 text-sm font-medium rounded-md transition-colors', activePeriod === 'semana' ? 'bg-indigo-600 text-white' : 'bg-gray-200 text-gray-700 hover:bg-gray-300']"
        >
          Esta Semana
        </button>
        <button
          @click="setPeriod('mes')"
          :class="['px-4 py-2 text-sm font-medium rounded-md transition-colors', activePeriod === 'mes' ? 'bg-indigo-600 text-white' : 'bg-gray-200 text-gray-700 hover:bg-gray-300']"
        >
          Este Mes
        </button>
      </div>

      <div v-if="isLoading" class="text-center py-10">
        <p class="text-gray-500">Cargando ventas...</p>
      </div>
      <div v-else-if="sales.length > 0" class="overflow-x-auto">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID Venta</th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Cliente</th>
              <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Fecha</th>
              <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Total</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="sale in sales" :key="sale.id" class="hover:bg-gray-50">
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">#{{ sale.id }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ sale.cliente?.nombre || 'N/A' }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ formatDate(sale.fecha) }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 text-right">{{ formatCurrency(sale.total) }}</td>
            </tr>
          </tbody>
           <tfoot class="bg-gray-100">
            <tr class="font-bold text-gray-700">
                <td class="px-6 py-3 text-left text-sm" colspan="3">Ventas Totales del Período</td>
                <td class="px-6 py-3 text-right text-sm">{{ formatCurrency(totalSales) }}</td>
            </tr>
        </tfoot>
        </table>
      </div>
      <div v-else class="text-center py-10 border-2 border-dashed border-gray-300 rounded-lg">
        <p class="text-gray-500">No se encontraron ventas para el período seleccionado.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import api from '@/services/api';

const sales = ref([]);
const isLoading = ref(false);
const activePeriod = ref('hoy');

const totalSales = computed(() => {
    return sales.value.reduce((sum, sale) => sum + sale.total, 0);
});

// Función para obtener las ventas según el período
const fetchSales = async () => {
  isLoading.value = true;
  const today = new Date();
  let startDate, endDate = new Date();

  switch (activePeriod.value) {
    case 'hoy':
      startDate = new Date(today.setHours(0, 0, 0, 0));
      break;
    case 'semana':
      const firstDayOfWeek = today.getDate() - today.getDay();
      startDate = new Date(today.setDate(firstDayOfWeek));
      startDate.setHours(0, 0, 0, 0);
      break;
    case 'mes':
      startDate = new Date(today.getFullYear(), today.getMonth(), 1);
      startDate.setHours(0, 0, 0, 0);
      break;
  }
  
  try {
    const response = await api.get('/reportes/ventas-entre-fechas', {
      params: {
        inicio: startDate.toISOString().split('T')[0],
        fin: endDate.toISOString().split('T')[0],
      },
    });
    sales.value = response.data;
  } catch (error) {
    console.error("Error al obtener las ventas:", error);
    sales.value = [];
  } finally {
    isLoading.value = false;
  }
};

const setPeriod = (period) => {
  activePeriod.value = period;
  fetchSales();
};

const formatCurrency = (value) => {
  return new Intl.NumberFormat('es-GT', { style: 'currency', currency: 'GTQ' }).format(value);
};

const formatDate = (dateString) => {
  const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
  return new Date(dateString).toLocaleDateString('es-GT', options);
};

// Cargar las ventas de hoy al montar el componente
onMounted(() => {
  fetchSales();
});
</script>