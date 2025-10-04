<script setup lang="ts">
import { Bar } from 'vue-chartjs';
import { Chart as ChartJS, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale } from 'chart.js';

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale);

const chartData = {
  labels: ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'],
  datasets: [
    {
      label: 'Ventas de la Semana',
      backgroundColor: '#3B82F6', // Corresponde a 'primary-blue'
      borderColor: '#3B82F6',
      borderRadius: 4,
      data: [450, 780, 1200, 950, 1500, 1300, 1800],
    },
  ],
};

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
};
</script>

<template>
  <div class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200">
    <h3 class="text-lg font-semibold text-text-dark mb-4 font-serif">Ventas de la Semana</h3>
    <div class="h-64">
      <Bar :data="chartData" :options="chartOptions" />
    </div>
  </div>
</template>