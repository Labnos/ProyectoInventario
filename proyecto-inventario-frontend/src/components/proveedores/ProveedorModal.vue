<script setup lang="ts">
import { ref, watch } from 'vue';
import api from '../../services/api';

// --- Interfaces y Props ---
interface Proveedor {
  id?: number;
  nombreEmpresa: string;
  nombreContacto: string;
  telefono: string;
  direccion: string;
}

const props = defineProps<{
  isOpen: boolean;
  proveedor: Proveedor | null;
}>();

const emit = defineEmits(['close', 'saved']);

// --- Estado del Formulario ---
const form = ref<Proveedor>({
  nombreEmpresa: '',
  nombreContacto: '',
  telefono: '',
  direccion: '',
});
const isLoading = ref(false);
const errorMessage = ref('');

// --- Lógica del Componente ---
watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    if (props.proveedor) {
      form.value = { ...props.proveedor };
    } else {
      form.value = { nombreEmpresa: '', nombreContacto: '', telefono: '', direccion: '' };
    }
    errorMessage.value = '';
  }
});

const handleSubmit = async () => {
  isLoading.value = true;
  errorMessage.value = '';
  try {
    if (form.value.id) {
      // Actualización (PUT)
      await api.put(`/api/proveedores/${form.value.id}`, form.value);
    } else {
      // Creación (POST)
      await api.post('/api/proveedores', form.value);
    }
    emit('saved');
  } catch (error) {
    errorMessage.value = 'No se pudo guardar el proveedor. Revisa los datos.';
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white-card p-8 rounded-lg shadow-xl w-full max-w-lg">
      <h2 class="text-2xl font-bold font-serif text-text-dark mb-6">
        {{ proveedor ? 'Editar Proveedor' : 'Añadir Nuevo Proveedor' }}
      </h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-gray-700">Nombre de la Empresa / Proveedor</label>
          <input v-model="form.nombreEmpresa" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2 focus:ring-primary-blue focus:border-primary-blue" />
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Nombre del Contacto</label>
            <input v-model="form.nombreContacto" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Teléfono</label>
            <input v-model="form.telefono" type="text" class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
        </div>
        
        <div>
          <label class="block text-sm font-medium text-gray-700">Dirección</label>
          <input v-model="form.direccion" type="text" class="mt-1 w-full border border-gray-300 rounded-md p-2" />
        </div>

        <div v-if="errorMessage" class="text-sm text-accent-red">{{ errorMessage }}</div>

        <div class="flex justify-end space-x-4 pt-4">
          <button type="button" @click="$emit('close')" class="bg-gray-200 text-gray-800 px-4 py-2 rounded-md hover:bg-gray-300">
            Cancelar
          </button>
          <button type="submit" :disabled="isLoading" class="bg-primary-blue text-white-card px-4 py-2 rounded-md hover:bg-blue-700 disabled:bg-blue-300">
            {{ isLoading ? 'Guardando...' : 'Guardar Proveedor' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>