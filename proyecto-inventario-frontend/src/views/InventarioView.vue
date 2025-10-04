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
const pageSize = ref(10);
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
    addNotification(message, 'error');
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchProducts);
watch(currentPage, fetchProducts);

// --- Lógica de Modales y Acciones con Notificaciones ---
const handleProductSaved = () => {
  isProductModalOpen.value = false;
  addNotification('Producto guardado con éxito.', 'success');
  fetchProducts(); 
};

const handleDeleteProduct = async () => {
  if (!productToDelete.value) return;
  try {
    await api.delete(`/api/productos/${productToDelete.value.id}`);
    addNotification('Producto eliminado correctamente.', 'success');
  } catch (err) {
    addNotification('Error al eliminar el producto.', 'error');
  } finally {
    isConfirmModalOpen.value = false;
    productToDelete.value = null;
    // Si estamos en la última página y borramos el último item, volvemos a la anterior
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
</script>

<template>
  <div class="p-4 md:p-8 font-sans">
    <header class="flex justify-between items-center mb-8">
       <div>
        <h1 class="text-3xl font-bold text-text-dark font-serif">Gestión de Inventario</h1>
        <p class="text-gray-500">Añade, edita y consulta tus productos.</p>
      </div>
      <button @click="openAddModal" class="bg-primary-blue text-white-card font-semibold py-2 px-4 rounded-md hover:bg-blue-700 transition-colors">
        + Añadir Producto
      </button>
    </header>

    <div v-if="isLoading" class="text-center py-20 text-gray-500">Cargando productos...</div>
    <div v-else-if="error" class="text-center py-20 text-accent-red">{{ error }}</div>

    <div v-else class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200">
      <div class="overflow-x-auto">
        <table class="w-full text-left min-w-[600px]">
          <thead class="border-b border-gray-200">
            <tr>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">PRODUCTO</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">SUCURSAL</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">PRECIO VENTA</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">STOCK</th>
              <th class="py-3 px-4 font-semibold text-sm text-gray-600">ACCIONES</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="producto in productos" :key="producto.id" class="border-b border-gray-100 hover:bg-gray-50">
              <td class="py-3 px-4 text-text-dark font-medium">{{ producto.nombre }}</td>
              <td class="py-3 px-4 text-gray-600">{{ producto.sucursal }}</td>
              <td class="py-3 px-4 text-gray-600">Q {{ producto.precioVenta.toFixed(2) }}</td>
              <td class="py-3 px-4">
                <span 
                  class="px-2 py-1 text-xs font-semibold rounded-full"
                  :class="{
                    'bg-green-100 text-accent-green': producto.stock > 5,
                    'bg-yellow-100 text-yellow-800': producto.stock > 0 && producto.stock <= 5,
                    'bg-red-100 text-accent-red': producto.stock === 0
                  }"
                >
                  {{ producto.stock }}
                </span>
              </td>
              <td class="py-3 px-4 text-sm space-x-4">
                <button @click="openEditModal(producto)" class="text-primary-blue hover:underline font-semibold">Editar</button>
                <button @click="openConfirmDeleteModal(producto)" class="text-accent-red hover:underline font-semibold">Eliminar</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <Pagination 
        v-if="totalProducts > 0"
        :current-page="currentPage"
        :total-items="totalProducts"
        :page-size="pageSize"
        @page-changed="(page) => currentPage = page"
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
      :message="`¿Estás seguro de que deseas eliminar '${productToDelete?.nombre}'?`"
      @close="isConfirmModalOpen = false"
      @confirm="handleDeleteProduct"
    />
  </div>
</template>