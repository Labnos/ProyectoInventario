<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue';
import api from '../../services/api';

// --- Interfaces y Props ---
interface Producto {
  id?: number;
  nombre: string;
  tipoPrenda: string;
  proveedor: string;
  sucursal: string;
  precioAdquisicion: number;
  precioVenta: number;
  stock: number;
}

const props = defineProps<{
  isOpen: boolean;
  product: Producto | null;
}>();

const emit = defineEmits(['close', 'saved']);

// --- Estado del Formulario ---
const form = ref<Producto>({
  nombre: '',
  tipoPrenda: '',
  proveedor: '',
  sucursal: '',
  precioAdquisicion: 0,
  precioVenta: 0,
  stock: 0,
});
const isLoading = ref(false);
const errorMessage = ref('');

// --- Lógica del Componente ---
watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    // Si se abre para editar, carga los datos del producto
    if (props.product) {
      form.value = { ...props.product };
    } else {
      // Si se abre para añadir, resetea el formulario
      form.value = { nombre: '', tipoPrenda: '', proveedor: '', sucursal: '', precioAdquisicion: 0, precioVenta: 0, stock: 0 };
    }
    errorMessage.value = '';
  }
});

const handleSubmit = async () => {
  isLoading.value = true;
  errorMessage.value = '';
  try {
    if (form.value.id) {
      // Es una actualización (PUT)
      await api.put(`/api/productos/${form.value.id}`, form.value);
    } else {
      // Es una creación (POST)
      await api.post('/api/productos', form.value);
    }
    emit('saved');
  } catch (error) {
    console.error("Error al guardar el producto:", error);
    errorMessage.value = 'No se pudo guardar el producto. Revisa los datos.';
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white-card p-8 rounded-lg shadow-xl w-full max-w-2xl">
      <h2 class="text-2xl font-bold font-serif text-text-dark mb-6">
        {{ product ? 'Editar Producto' : 'Añadir Nuevo Producto' }}
      </h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Nombre del Producto</label>
            <input v-model="form.nombre" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2 focus:ring-primary-blue focus:border-primary-blue" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Tipo de Prenda</label>
            <input v-model="form.tipoPrenda" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
        </div>
        
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Proveedor</label>
            <input v-model="form.proveedor" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Sucursal</label>
            <input v-model="form.sucursal" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
        </div>
        
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Stock</label>
            <input v-model.number="form.stock" type="number" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Precio Adquisición (Q)</label>
            <input v-model.number="form.precioAdquisicion" type="number" step="0.01" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Precio Venta (Q)</label>
            <input v-model.number="form.precioVenta" type="number" step="0.01" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
        </div>

        <div v-if="errorMessage" class="text-sm text-accent-red">{{ errorMessage }}</div>

        <div class="flex justify-end space-x-4 pt-4">
          <button type="button" @click="$emit('close')" class="bg-gray-200 text-gray-800 px-4 py-2 rounded-md hover:bg-gray-300">
            Cancelar
          </button>
          <button type="submit" :disabled="isLoading" class="bg-primary-blue text-white-card px-4 py-2 rounded-md hover:bg-blue-700 disabled:bg-blue-300">
            {{ isLoading ? 'Guardando...' : 'Guardar Producto' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>