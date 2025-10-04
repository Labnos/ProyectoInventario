<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import api from '../services/api';
import PromotionModal from '../components/promociones/PromotionModal.vue';
import ConfirmationModal from '../components/shared/ConfirmationModal.vue';
import { useNotifications } from '../composables/useNotifications';

// --- Interfaces y Estado ---
interface Promocion {
  id: number;
  nombre: string;
  porcentajeDescuento: number;
  fechaInicio: string;
  fechaFin: string;
  activa: boolean;
}

const promociones = ref<Promocion[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);
const { addNotification } = useNotifications();
const filterState = ref<'Todas' | 'Activas' | 'Programadas' | 'Finalizadas'>('Todas');

const isPromotionModalOpen = ref(false);
const selectedPromotion = ref<Promocion | null>(null);
const isConfirmModalOpen = ref(false);
const promotionToDelete = ref<Promocion | null>(null);

// --- Lógica de Datos ---
const fetchPromotions = async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const response = await api.get('/api/promociones');
    promociones.value = response.data;
  } catch (err) {
    addNotification('No se pudieron cargar las promociones.', 'error');
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchPromotions);

const filteredPromotions = computed(() => {
  const now = new Date();
  switch (filterState.value) {
    case 'Activas':
      return promociones.value.filter(p => new Date(p.fechaInicio) <= now && new Date(p.fechaFin) >= now && p.activa);
    case 'Programadas':
      return promociones.value.filter(p => new Date(p.fechaInicio) > now && p.activa);
    case 'Finalizadas':
      return promociones.value.filter(p => new Date(p.fechaFin) < now || !p.activa);
    default:
      return promociones.value;
  }
});

// --- Lógica de Modales y Acciones ---
const handlePromotionSaved = () => {
  isPromotionModalOpen.value = false;
  addNotification('Promoción guardada con éxito.', 'success');
  fetchPromotions();
};

const handleDeletePromotion = async () => {
  if (!promotionToDelete.value) return;
  try {
    await api.delete(`/api/promociones/${promotionToDelete.value.id}`);
    addNotification('Promoción eliminada correctamente.', 'success');
  } catch (err) {
    addNotification('Error al eliminar la promoción.', 'error');
  } finally {
    isConfirmModalOpen.value = false;
    promotionToDelete.value = null;
    fetchPromotions();
  }
};

const openAddModal = () => { selectedPromotion.value = null; isPromotionModalOpen.value = true; };
const openEditModal = (promo: Promocion) => { selectedPromotion.value = { ...promo }; isPromotionModalOpen.value = true; };
const openConfirmDeleteModal = (promo: Promocion) => { promotionToDelete.value = promo; isConfirmModalOpen.value = true; };
</script>

<template>
  <div class="p-4 md:p-8 font-sans">
    <header class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-text-dark font-serif">Gestión de Promociones</h1>
        <p class="text-gray-500">Crea y administra campañas de descuento.</p>
      </div>
      <button @click="openAddModal" class="bg-primary-blue text-white-card font-semibold py-2 px-4 rounded-md hover:bg-blue-700 transition-colors">
        + Crear Promoción
      </button>
    </header>

    <div class="mb-6 flex space-x-2">
      <button @click="filterState = 'Todas'" :class="{'bg-primary-blue text-white-card': filterState === 'Todas'}" class="px-4 py-2 text-sm rounded-md border">Todas</button>
      <button @click="filterState = 'Activas'" :class="{'bg-primary-blue text-white-card': filterState === 'Activas'}" class="px-4 py-2 text-sm rounded-md border">Activas</button>
      <button @click="filterState = 'Programadas'" :class="{'bg-primary-blue text-white-card': filterState === 'Programadas'}" class="px-4 py-2 text-sm rounded-md border">Programadas</button>
      <button @click="filterState = 'Finalizadas'" :class="{'bg-primary-blue text-white-card': filterState === 'Finalizadas'}" class="px-4 py-2 text-sm rounded-md border">Finalizadas</button>
    </div>

    <div v-if="isLoading" class="text-center py-20">Cargando...</div>

    <div v-else class="bg-white-card p-6 rounded-lg shadow-sm border">
      <table class="w-full text-left">
        <thead>
          <tr class="border-b">
            <th class="py-3 px-4 text-sm font-semibold">NOMBRE</th>
            <th class="py-3 px-4 text-sm font-semibold">DESCUENTO</th>
            <th class="py-3 px-4 text-sm font-semibold">DURACIÓN</th>
            <th class="py-3 px-4 text-sm font-semibold">ESTADO</th>
            <th class="py-3 px-4 text-sm font-semibold">ACCIONES</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="promo in filteredPromotions" :key="promo.id" class="border-b hover:bg-gray-50">
            <td class="py-3 px-4 font-medium">{{ promo.nombre }}</td>
            <td class="py-3 px-4">{{ promo.porcentajeDescuento }}%</td>
            <td class="py-3 px-4">{{ new Date(promo.fechaInicio).toLocaleDateString() }} - {{ new Date(promo.fechaFin).toLocaleDateString() }}</td>
            <td class="py-3 px-4">
              <span :class="{'bg-green-100 text-green-800': promo.activa, 'bg-gray-100 text-gray-800': !promo.activa}" class="px-2 py-1 text-xs rounded-full">{{ promo.activa ? 'Activa' : 'Inactiva' }}</span>
            </td>
            <td class="py-3 px-4 text-sm space-x-4">
              <button @click="openEditModal(promo)" class="text-primary-blue font-semibold">Editar</button>
              <button @click="openConfirmDeleteModal(promo)" class="text-accent-red font-semibold">Eliminar</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <PromotionModal :isOpen="isPromotionModalOpen" :promotion="selectedPromotion" @close="isPromotionModalOpen = false" @saved="handlePromotionSaved" />
    <ConfirmationModal :isOpen="isConfirmModalOpen" title="Confirmar Eliminación" :message="`¿Eliminar la promoción '${promotionToDelete?.nombre}'?`" @close="isConfirmModalOpen = false" @confirm="handleDeletePromotion" />
  </div>
</template>