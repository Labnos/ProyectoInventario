<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'; // 1. Importa 'computed'
import api from '../services/api';
import ClientModal from '../components/clientes/ClientModal.vue';
import ConfirmationModal from '../components/shared/ConfirmationModal.vue';
import { useNotifications } from '../composables/useNotifications';

// --- Interfaces y Estado ---
interface Cliente {
  id: number;
  nombreCompleto: string;
  numeroContacto: string;
  historialCompras: string;
  preferencias: string;
  ubicacion: string;
}

const clientes = ref<Cliente[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);
const { addNotification } = useNotifications();

// 2. Estado para la búsqueda
const searchQuery = ref('');

const isClientModalOpen = ref(false);
const selectedClient = ref<Cliente | null>(null);
const isConfirmModalOpen = ref(false);
const clientToDelete = ref<Cliente | null>(null);


// --- 3. Propiedad Computada para Filtrar Clientes ---
const filteredClients = computed(() => {
  if (!searchQuery.value) {
    return clientes.value;
  }
  const query = searchQuery.value.toLowerCase();
  return clientes.value.filter(cliente => 
    cliente.nombreCompleto.toLowerCase().includes(query) ||
    cliente.numeroContacto.toLowerCase().includes(query)
  );
});


// --- Lógica de Datos ---
const fetchClients = async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const response = await api.get('/api/clientes');
    clientes.value = response.data;
  } catch (err) {
    const message = 'No se pudieron cargar los clientes.';
    error.value = message;
    addNotification(message, 'error');
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchClients);

// --- Lógica de Modales y Acciones (sin cambios) ---
const handleClientSaved = () => { /* ... */ };
const handleDeleteClient = async () => { /* ... */ };
const openAddModal = () => { /* ... */ };
const openEditModal = (client: Cliente) => { /* ... */ };
const openConfirmDeleteModal = (client: Cliente) => { /* ... */ };

// (Aquí va la lógica de los modales y acciones que ya teníamos, la omito por brevedad pero debe estar en tu archivo)
</script>

<template>
  <div class="p-4 md:p-8 font-sans">
    <header class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-text-dark font-serif">Gestión de Clientes</h1>
        <p class="text-gray-500">Administra la información de tus clientes frecuentes.</p>
      </div>
      <button @click="openAddModal" class="bg-primary-blue text-white-card font-semibold py-2 px-4 rounded-md hover:bg-blue-700 transition-colors">
        + Añadir Cliente
      </button>
    </header>

    <div class="mb-6">
      <input 
        v-model="searchQuery"
        type="text" 
        placeholder="🔍 Buscar por nombre o contacto..." 
        class="w-full md:w-1/3 border border-gray-300 rounded-md p-2 focus:ring-primary-blue focus:border-primary-blue"
      />
    </div>

    <div v-if="isLoading" class="text-center py-20 text-gray-500">Cargando clientes...</div>
    <div v-else-if="error" class="text-center py-20 text-accent-red">{{ error }}</div>

    <div v-else class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200">
      <div class="overflow-x-auto">
        <table class="w-full text-left min-w-[600px]">
          <thead class="border-b border-gray-200">
            <tr>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">NOMBRE COMPLETO</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">CONTACTO</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">UBICACIÓN</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">ACCIONES</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="filteredClients.length === 0">
              <td colspan="4" class="text-center py-10 text-gray-500">No se encontraron clientes.</td>
            </tr>
            <tr v-for="cliente in filteredClients" :key="cliente.id" class="border-b border-gray-100 hover:bg-gray-50">
              <td class="py-3 px-4 text-text-dark font-medium">{{ cliente.nombreCompleto }}</td>
              <td class="py-3 px-4 text-gray-600">{{ cliente.numeroContacto }}</td>
              <td class="py-3 px-4 text-gray-600">{{ cliente.ubicacion }}</td>
              <td class="py-3 px-4 text-sm space-x-4">
                <button @click="openEditModal(cliente)" class="text-primary-blue hover:underline font-semibold">Editar</button>
                <button @click="openConfirmDeleteModal(cliente)" class="text-accent-red hover:underline font-semibold">Eliminar</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <ClientModal 
      :isOpen="isClientModalOpen" 
      :client="selectedClient" 
      @close="isClientModalOpen = false" 
      @saved="handleClientSaved" 
    />
    <ConfirmationModal
      :isOpen="isConfirmModalOpen"
      title="Confirmar Eliminación"
      :message="`¿Estás seguro de que deseas eliminar a '${clientToDelete?.nombreCompleto}'?`"
      @close="isConfirmModalOpen = false"
      @confirm="handleDeleteClient"
    />
  </div>
</template>