<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import api from '../services/api';
import ProductModal from '../components/inventario/ProductModal.vue';
import ConfirmationModal from '../components/shared/ConfirmationModal.vue';
import Pagination from '../components/shared/Pagination.vue'; // Importa la paginación
import { useNotifications } from '../composables/useNotifications'; // Importa las notificaciones

// --- Interfaces y Estado ---
interface Producto { id: number; nombre: string; tipoPrenda: string; proveedor: string; sucursal: string; precioAdquisicion: number; precioVenta: number; stock: number; }
const productos = ref<Producto[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);
const { addNotification } = useNotifications();

// Estado de Paginación
const currentPage = ref(1);
const pageSize = ref(10);
const totalProducts = ref(0);

// Estado de Filtros
const searchQuery = ref('');
const selectedSucursal = ref('Todas');
const selectedStatus = ref('Todos');

// Modales
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
    console.error("Error al cargar los productos:", err);
    error.value = 'No se pudieron cargar los productos.';
    addNotification(error.value, 'error');
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchProducts);

// Recargar datos cuando cambia la página
watch(currentPage, fetchProducts);

// Propiedad computada para filtros (ahora solo se aplica a la página actual)
const filteredProducts = computed(() => {
  let filtered = productos.value;
  if (searchQuery.value) { /* ... lógica de filtro ... */ }
  // ... resto de la lógica de filtros
  return filtered;
});

// --- Lógica de Modales y Acciones ---
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
    console.error("Error al eliminar el producto:", err);
    addNotification('Error al eliminar el producto.', 'error');
  } finally {
    isConfirmModalOpen.value = false;
    productToDelete.value = null;
    fetchProducts();
  }
};
// ... resto de la lógica de modales sin cambios ...
const openAddModal = () => { selectedProduct.value = null; isProductModalOpen.value = true; };
const openEditModal = (p: Producto) => { selectedProduct.value = { ...p }; isProductModalOpen.value = true; };
const openConfirmDeleteModal = (p: Producto) => { productToDelete.value = p; isConfirmModalOpen.value = true; };
</script>

<template>
  <div class="p-4 md:p-8 font-sans">
    <header class="flex justify-between items-center mb-8">
      </header>

    <div class="mb-6 bg-white-card p-4 rounded-lg shadow-sm border ...">
      </div>

    <div v-if="isLoading" class="text-center py-10">Cargando productos...</div>
    <div v-else-if="error" class="text-center py-10 text-accent-red">{{ error }}</div>

    <div v-else class="bg-white-card p-6 rounded-lg shadow-sm border border-gray-200">
      <table class="w-full text-left">
        </table>

      <Pagination 
        :current-page="currentPage"
        :total-items="totalProducts"
        :page-size="pageSize"
        @page-changed="(page) => currentPage = page"
        class="mt-6"
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