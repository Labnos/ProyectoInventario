<script setup lang="ts">
import { ref, onMounted } from 'vue';
import api from '../services/api';

// Importa los componentes hijo que crearemos a continuación
import KpiCard from '../components/dashboard/KpiCard.vue';
import SalesChart from '../components/dashboard/SalesChart.vue';
import AlertsPanel from '../components/dashboard/AlertsPanel.vue';
import TopProductsList from '../components/dashboard/TopProductsList.vue';

// --- Estado para almacenar los datos del backend ---
const dailyRevenue = ref(0);
const dailySalesCount = ref(0);
const pendingOrders = ref(0);
const lowStockProducts = ref([]);
const topProducts = ref([]);
const weeklySales = ref({}); // Datos para el gráfico
const isLoading = ref(true);

// --- Función para cargar todos los datos del dashboard ---
onMounted(async () => {
  try {
    // Aquí harías las llamadas a tus endpoints de /api/reportes
    // Por ahora, usaremos datos de ejemplo mientras construimos la UI.
    
    // Ejemplo de llamada real que podrías usar:
    // const revenueRes = await api.get('/api/reportes/ventas-por-fecha/2025-10-03');
    // dailyRevenue.value = revenueRes.data.totalVentas;
    
    // --- Datos de Ejemplo ---
    dailyRevenue.value = 2175.50;
    dailySalesCount.value = 18;
    pendingOrders.value = 5;
    
    lowStockProducts.value = [
      { id: 1, nombre: 'Huipil Cobán', stock: 2 },
      { id: 2, nombre: 'Corte de Sololá', stock: 4 },
    ];
    
    topProducts.value = [
      { nombre: 'Corte Totonicapán', vendidos: 38 },
      { nombre: 'Blusa Quiché', vendidos: 31 },
      { nombre: 'Faja Sololá', vendidos: 25 },
    ];

  } catch (error) {
    console.error("Error al cargar los datos del dashboard:", error);
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <div class="p-4 md:p-8 bg-gray-50 min-h-screen">
    <h1 class="text-3xl font-bold text-gray-800 mb-6" style="font-family: 'Roboto Slab', serif;">
      Resumen General
    </h1>

    <div v-if="isLoading" class="text-center text-gray-500">
      Cargando datos...
    </div>

    <div v-else class="space-y-8">
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <KpiCard title="Ingresos Hoy" :value="`Q ${dailyRevenue.toFixed(2)}`" description="Basado en 18 ventas" />
        <KpiCard title="Ventas del Día" :value="dailySalesCount" description="12 en tienda, 6 online" />
        <KpiCard title="Pedidos Pendientes" :value="pendingOrders" description="Por pagar o entregar" />
        <KpiCard title="Bajo Inventario" :value="lowStockProducts.length" description="Productos por agotar" />
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div class="lg:col-span-2 space-y-6">
          <SalesChart />
          <AlertsPanel :alerts="lowStockProducts" />
        </div>
        
        <div class="lg:col-span-1">
          <TopProductsList :products="topProducts" />
        </div>
      </div>
    </div>
  </div>
</template>