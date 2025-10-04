<script setup lang="ts">
import { ref, onMounted } from 'vue';
import api from '../services/api';

// Importa los componentes hijo
import KpiCard from '../components/dashboard/KpiCard.vue';
import SalesChart from '../components/dashboard/SalesChart.vue';
import AlertsPanel from '../components/dashboard/AlertsPanel.vue';
import TopProductsList from '../components/dashboard/TopProductsList.vue';

// --- Interfaces para tipar los datos de la API ---
interface LowStockProduct {
  id: number;
  nombre: string;
  stock: number;
}

interface TopProduct {
  productoNombre: string;
  totalVendido: number;
}

// --- Estado del Componente ---
const dailyRevenue = ref(0);
const topProducts = ref<TopProduct[]>([]);
const lowStockProducts = ref<LowStockProduct[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);

// --- Carga de Datos desde la API ---
onMounted(async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const today = new Date().toISOString().split('T')[0];
    const [revenueRes, topProductsRes, lowStockRes] = await Promise.all([
      api.get(`/api/reportes/ventas-por-fecha/${today}`),
      api.get('/api/reportes/productos-mas-vendidos?top=5'),
      api.get('/api/reportes/bajo-stock/5') // Umbral de 5 para bajo stock
    ]);

    dailyRevenue.value = revenueRes.data.totalVentas || 0;
    topProducts.value = topProductsRes.data || [];
    lowStockProducts.value = lowStockRes.data || [];

  } catch (err) {
    console.error("Error al cargar los datos del dashboard:", err);
    error.value = 'No se pudieron cargar los datos. Inténtalo de nuevo más tarde.';
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <div class="p-4 md:p-8 bg-background-light min-h-screen font-sans">
    <header class="mb-8">
      <h1 class="text-3xl font-bold text-text-dark font-serif">
        Resumen General
      </h1>
      <p class="text-gray-500">Bienvenido al panel de control.</p>
    </header>

    <div v-if="isLoading" class="text-center py-10">
      <p class="text-gray-500">Cargando datos del dashboard...</p>
    </div>

    <div v-else-if="error" class="bg-red-100 border border-accent-red text-accent-red px-4 py-3 rounded-md">
      <p>{{ error }}</p>
    </div>
    
    <div v-else class="space-y-8">
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <KpiCard title="Ingresos Hoy" :value="`Q ${dailyRevenue.toFixed(2)}`" description="Total de ventas del día" icon="cash" />
        <KpiCard title="Bajo Inventario" :value="lowStockProducts.length" description="Productos por agotarse" icon="warning" />
        <KpiCard title="Ventas del Día" value="N/A" description="Endpoint no disponible" icon="chart" />
        <KpiCard title="Pedidos Pendientes" value="N/A" description="Endpoint no disponible" icon="truck" />
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div class="lg:col-span-2 space-y-8">
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