<script setup lang="ts">
defineProps({
  alerts: Array,
});
</script>

<template>
  <div class="bg-white p-6 rounded-lg shadow-md">
    <h3 class="text-lg font-semibold text-gray-800 mb-4">🔔 Alertas y Notificaciones</h3>
    <ul v-if="alerts.length > 0" class="space-y-3">
      <li v-for="alert in alerts" :key="alert.id" class="text-sm text-yellow-800 bg-yellow-100 p-3 rounded-md">
        <strong>[Bajo Stock]</strong> El producto "{{ alert.nombre }}" tiene solo {{ alert.stock }} unidades.
      </li>
    </ul>
    <p v-else class="text-sm text-gray-500">No hay alertas nuevas.</p>
  </div>
</template>