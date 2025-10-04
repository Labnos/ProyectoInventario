<script setup lang="ts">
defineProps({
  alerts: Array as () => { id: number; nombre: string; stock: number }[],
});
</script>

<template>
  <div class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200">
    <h3 class="text-lg font-semibold text-text-dark mb-4 font-serif">🔔 Alertas y Notificaciones</h3>
    <ul v-if="alerts && alerts.length > 0" class="space-y-3">
      <li v-for="alert in alerts" :key="alert.id" class="text-sm text-red-800 bg-red-100 p-3 rounded-md border border-accent-red">
        <strong>[Bajo Stock]</strong> El producto "{{ alert.nombre }}" tiene solo {{ alert.stock }} unidades restantes.
      </li>
    </ul>
    <p v-else class="text-sm text-gray-500">No hay alertas nuevas en este momento.</p>
  </div>
</template>