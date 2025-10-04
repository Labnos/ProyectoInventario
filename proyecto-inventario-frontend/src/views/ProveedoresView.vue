<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import api from '../services/api';
import ProveedorModal from '../components/proveedores/ProveedorModal.vue';
import ConfirmationModal from '../components/shared/ConfirmationModal.vue';
import Pagination from '../components/shared/Pagination.vue';
import { useNotifications } from '../composables/useNotifications';

// --- Interfaces y Estado ---
interface Proveedor {
  id: number;
  nombreEmpresa: string;
  nombreContacto: string;
  telefono: string;
  direccion: string;
}

const proveedores = ref<Proveedor[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);
const { addNotification } = useNotifications();

const currentPage = ref(1);
const pageSize = ref(10);
const totalProveedores = ref(0);
const searchQuery = ref('');

const isProveedorModalOpen = ref(false);
const selectedProveedor = ref<Proveedor | null>(null);
const isConfirmModalOpen = ref(false);
const proveedorToDelete = ref<Proveedor | null>(null);

// --- Lógica de Datos ---
const fetchProveedores = async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const response = await api.get('/api/proveedores', {
      params: {
        pageNumber: currentPage.value,
        pageSize: pageSize.value,
        // En una implementación server-side, enviaríamos la búsqueda aquí:
        // query: searchQuery.value 
      }
    });
    proveedores.value = response.data.items;
    totalProveedores.value = response.data.totalCount;
  } catch (err) {
    addNotification('No se pudieron cargar los proveedores.', 'error');
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchProveedores);
watch(currentPage, fetchProveedores);

// --- Propiedad Computada para Búsqueda (Client-Side) ---
const filteredProveedores = computed(() => {
  if (!searchQuery.value) {
    return proveedores.value;
  }
  const query = searchQuery.value.toLowerCase();
  return proveedores.value.filter(p => 
    p.nombreEmpresa.toLowerCase().includes(query) ||
    p.nombreContacto.toLowerCase().includes(query)
  );
});

// --- Lógica de Modales y Acciones ---
const handleProveedorSaved = () => {
  isProveedorModalOpen.value = false;
  addNotification('Proveedor guardado con éxito.', 'success');
  fetchProveedores();
};

const handleDeleteProveedor = async () => {
  if (!proveedorToDelete.value) return;
  try {
    await api.delete(`/api/proveedores/${proveedorToDelete.value.id}`);
    addNotification('Proveedor eliminado correctamente.', 'success');
  } catch (err) {
    addNotification('Error al eliminar el proveedor.', 'error');
  } finally {
    isConfirmModalOpen.value = false;
    proveedorToDelete.value = null;
    fetchProveedores();
  }
};

const openAddModal = () => { selectedProveedor.value = null; isProveedorModalOpen.value = true; };
const openEditModal = (p: Proveedor) => { selectedProveedor.value = { ...p }; isProveedorModalOpen.value = true; };
const openConfirmDeleteModal = (p: Proveedor) => { proveedorToDelete.value = p; isConfirmModalOpen.value = true; };
</script>

<template>
  <div class="p-4 md:p-8 font-sans">
    <header class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-text-dark font-serif">Gestión de Proveedores</h1>
        <p class="text-gray-500">Administra tus contactos comerciales.</p>
      </div>
      <button @click="openAddModal" class="bg-primary-blue text-white-card font-semibold py-2 px-4 rounded-md hover:bg-blue-700">
        + Añadir Proveedor
      </button>
    </header>

    <div class="mb-6">
      <input 
        v-model="searchQuery"
        type="text" 
        placeholder="🔍 Buscar por nombre de empresa o contacto..." 
        class="w-full md:w-1/3 border border-gray-300 rounded-md p-2 focus:ring-primary-blue focus:border-primary-blue"
      />
    </div>

    <div v-if="isLoading" class="text-center py-20">Cargando...</div>

    <div v-else class="bg-white-card p-6 rounded-lg shadow-sm border">
      <div class="overflow-x-auto">
        <table class="w-full text-left">
          <thead>
            <tr class="border-b">
              <th class="py-3 px-4 text-sm font-semibold">EMPRESA</th>
              <th class="py-3 px-4 text-sm font-semibold">CONTACTO</th>
              <th class="py-3 px-4 text-sm font-semibold">TELÉFONO</th>
              <th class="py-3 px-4 text-sm font-semibold">ACCIONES</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="filteredProveedores.length === 0">
                <td colspan="4" class="text-center py-10 text-gray-500">No se encontraron proveedores.</td>
            </tr>
            <tr v-for="proveedor in filteredProveedores" :key="proveedor.id" class="border-b hover:bg-gray-50">
              <td class="py-3 px-4 font-medium">{{ proveedor.nombreEmpresa }}</td>
              <td class="py-3 px-4">{{ proveedor.nombreContacto }}</td>
              <td class="py-3 px-4">{{ proveedor.telefono }}</td>
              <td class="py-3 px-4 text-sm space-x-4">
                <button @click="openEditModal(proveedor)" class="text-primary-blue font-semibold">Editar</button>
                <button @click="openConfirmDeleteModal(proveedor)" class="text-accent-red font-semibold">Eliminar</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <Pagination 
        v-if="totalProveedores > 0"
        :current-page="currentPage"
        :total-items="totalProveedores"
        :page-size="pageSize"
        @page-changed="(page) => currentPage = page"
      />
    </div>

    <ProveedorModal 
      :isOpen="isProveedorModalOpen" 
      :proveedor="selectedProveedor" 
      @close="isProveedorModalOpen = false" 
      @saved="handleProveedorSaved" 
    />
    <ConfirmationModal
      :isOpen="isConfirmModalOpen"
      title="Confirmar Eliminación"
      :message="`¿Eliminar a '${proveedorToDelete?.nombreEmpresa}'?`"
      @close="isConfirmModalOpen = false"
      @confirm="handleDeleteProveedor"
    />
  </div>
</template>