<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import api from '../services/api';
import ProductModal from '../components/inventario/ProductModal.vue';
import ConfirmationModal from '../components/shared/ConfirmationModal.vue';
import Pagination from '../components/shared/Pagination.vue';
import { useNotifications } from '../composables/useNotifications';

// --- Interfaces y Estado ---
interface Producto { id: number; nombre: string; tipoPrenda: string; proveedor: string; sucursal: string; precioAdquisicion: number; precioVenta: number; stock: number; }
const productos = ref<Producto[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);
const { addNotification } = useNotifications();

const currentPage = ref(1);
const pageSize = ref(10); // Mostrar 10 productos por página
const totalProducts = ref(0);

const isProductModalOpen = ref(false);
const selectedProduct = ref<Producto | null>(null);
const isConfirmModalOpen = ref(false);
const productToDelete = ref<Producto | null>(null);

// --- Lógica de Datos ---
const fetchProducts = async () => {
  isLoading.value = true;
  error.value = null;
  try {
    const response = await api.get('/api/productos', {
      params: {
        pageNumber: currentPage.value,
        pageSize: pageSize.value
      }
    });
    productos.value = response.data.items;
    totalProducts.value = response.data.totalCount;
  } catch (err) {
    const message = 'No se pudieron cargar los productos.';
    error.value = message;
    addNotification('No se pudieron cargar los productos.', 'error');
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchProducts);
watch(currentPage, fetchProducts);

// --- Lógica de Modales y Acciones con Notificaciones ---
const handleProductSaved = () => {
  isProductModalOpen.value = false;
  addNotification('Producto guardado con éxito', 'success');
  fetchProducts(); 
};

const handleDeleteProduct = async () => {
  if (!productToDelete.value) return;
  try {
    await api.delete(`/api/productos/${productToDelete.value.id}`);
    addNotification('Producto eliminado correctamente', 'success');
  } catch (err) {
    addNotification('Error al eliminar el producto', 'error');
  } finally {
    isConfirmModalOpen.value = false;
    productToDelete.value = null;
    if (productos.value.length === 1 && currentPage.value > 1) {
      currentPage.value--;
    } else {
      fetchProducts();
    }
  }
};

const openAddModal = () => { selectedProduct.value = null; isProductModalOpen.value = true; };
const openEditModal = (p: Producto) => { selectedProduct.value = { ...p }; isProductModalOpen.value = true; };
const openConfirmDeleteModal = (p: Producto) => { productToDelete.value = p; isConfirmModalOpen.value = true; };

// --- Funciones de formato ---
const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('es-GT', { style: 'currency', currency: 'GTQ' }).format(value);
};
</script>

<template>
  <div class="p-4 sm:p-6 lg:p-8 font-sans">
    <header class="flex flex-col sm:flex-row justify-between sm:items-center mb-8">
       <div>
        <h1 class="text-3xl font-bold text-text-dark font-serif">Gestión de Inventario</h1>
        <p class="mt-1 text-gray-500">Añade, edita y consulta las prendas de tu negocio.</p>
      </div>
      <button 
        @click="openAddModal" 
        class="mt-4 sm:mt-0 bg-primary-blue text-white-card font-semibold py-2 px-5 rounded-lg shadow-sm hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-blue transition-colors flex items-center gap-2"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
        </svg>
        Añadir Producto
      </button>
    </header>

    <div v-if="isLoading" class="text-center py-20 text-gray-500">Cargando productos...</div>
    <div v-else-if="error" class="text-center py-20 text-accent-red bg-red-50 p-4 rounded-lg">{{ error }}</div>

    <div v-else class="bg-white-card p-4 sm:p-6 rounded-xl shadow-md border border-gray-200">
      <div class="overflow-x-auto">
        <table class="w-full text-left min-w-[600px]">
          <thead class="border-b-2 border-gray-200">
            <tr>
              <th class="py-3 px-4 font-semibold text-sm text-gray-500 uppercase tracking-wider">Producto</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-500 uppercase tracking-wider">Sucursal</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-500 uppercase tracking-wider text-right">Precio Venta</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-500 uppercase tracking-wider text-center">Stock</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-500 uppercase tracking-wider">Acciones</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-100">
            <tr v-for="producto in productos" :key="producto.id" class="hover:bg-gray-50 transition-colors">
              <td class="py-4 px-4 text-text-dark font-medium">{{ producto.nombre }}</td>
              <td class="py-4 px-4 text-gray-600">{{ producto.sucursal }}</td>
              <td class="py-4 px-4 text-gray-600 text-right">{{ formatCurrency(producto.precioVenta) }}</td>
              <td class="py-4 px-4 text-center">
                <span 
                  class="px-3 py-1 text-xs font-bold rounded-full"
                  :class="{
                    'bg-green-100 text-green-800': producto.stock > 5,
                    'bg-yellow-100 text-yellow-800': producto.stock > 0 && producto.stock <= 5,
                    'bg-red-100 text-accent-red': producto.stock === 0
                  }"
                >
                  {{ producto.stock }}
                </span>
              </td>
              <td class="py-4 px-4 text-sm space-x-4">
                <button @click="openEditModal(producto)" class="text-primary-blue hover:underline font-semibold">Editar</button>
                <button @click="openConfirmDeleteModal(producto)" class="text-accent-red hover:underline font-semibold">Eliminar</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <Pagination 
        v-if="!isLoading && totalProducts > 0"
        class="mt-4"
        :current-page="currentPage"
        :total-items="totalProducts"
        :page-size="pageSize"
        @page-changed="(page: any) => currentPage = page"
      />
    </div>

    <ProductModal 
      :isOpen="isProductModalOpen" 
      :product="selectedProduct" 
      @close="isProductModalOpen = false" 
      @saved="handleProductSaved" 
    />
    <ConfirmationModal
      :isOpen="isConfirmModalOpen"
      title="Confirmar Eliminación"
      :message="`¿Estás seguro de que deseas eliminar '${productToDelete?.nombre}'? Esta acción no se puede deshacer.`"
      @close="isConfirmModalOpen = false"
      @confirm="handleDeleteProduct"
    />
  </div>
</template>