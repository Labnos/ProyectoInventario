<script setup lang="ts">
defineProps({
  products: Array as () => { productoNombre: string; totalVendido: number }[],
});
</script>

<template>
  <div class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200 h-full">
    <h3 class="text-lg font-semibold text-text-dark mb-4 font-serif">🏆 Productos Más Vendidos (Mes)</h3>
    <ul v-if="products && products.length > 0" class="space-y-4">
      <li v-for="(product, index) in products" :key="index" class="flex justify-between items-center text-sm">
        <span class="text-gray-600">{{ index + 1 }}. {{ product.productoNombre }}</span>
        <span class="font-bold text-text-dark bg-background-light px-2 py-1 rounded">{{ product.totalVendido }}</span>
      </li>
    </ul>
    <p v-else class="text-sm text-gray-500">Aún no hay datos de ventas este mes.</p>
  </div>
</template>