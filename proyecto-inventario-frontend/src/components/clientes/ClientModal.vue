<script setup lang="ts">
import { ref, watch } from 'vue';
import api from '../../services/api';

// --- Interfaces y Props ---
interface Cliente {
  id?: number;
  nombreCompleto: string;
  numeroContacto: string;
  historialCompras?: string; // Opcional por ahora
  preferencias: string;
  ubicacion: string;
}

const props = defineProps<{
  isOpen: boolean;
  client: Cliente | null;
}>();

const emit = defineEmits(['close', 'saved']);

// --- Estado del Formulario ---
const form = ref<Cliente>({
  nombreCompleto: '',
  numeroContacto: '',
  preferencias: '',
  ubicacion: '',
});
const isLoading = ref(false);
const errorMessage = ref('');

// --- Lógica del Componente ---
watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    if (props.client) {
      form.value = { ...props.client };
    } else {
      form.value = { nombreCompleto: '', numeroContacto: '', preferencias: '', ubicacion: '' };
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
      await api.put(`/api/clientes/${form.value.id}`, form.value);
    } else {
      // Creación (POST)
      await api.post('/api/clientes', form.value);
    }
    emit('saved');
  } catch (error) {
    errorMessage.value = 'No se pudo guardar el cliente. Revisa los datos.';
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white-card p-8 rounded-lg shadow-xl w-full max-w-lg">
      <h2 class="text-2xl font-bold font-serif text-text-dark mb-6">
        {{ client ? 'Editar Cliente' : 'Añadir Nuevo Cliente' }}
      </h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700">Nombre Completo</label>
            <input v-model="form.nombreCompleto" type="text" required class="mt-1 w-full border border-gray-300 rounded-md p-2 focus:ring-primary-blue focus:border-primary-blue" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700">Número de Contacto</label>
            <input v-model="form.numeroContacto" type="text" class="mt-1 w-full border border-gray-300 rounded-md p-2" />
          </div>
        </div>
        
        <div>
          <label class="block text-sm font-medium text-gray-700">Ubicación</label>
          <input v-model="form.ubicacion" type="text" class="mt-1 w-full border border-gray-300 rounded-md p-2" />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Preferencias y Notas</label>
          <textarea v-model="form.preferencias" rows="3" class="mt-1 w-full border border-gray-300 rounded-md p-2" placeholder="Ej: Le gustan los cortes de Totonicapán..."></textarea>
        </div>

        <div v-if="errorMessage" class="text-sm text-accent-red">{{ errorMessage }}</div>

        <div class="flex justify-end space-x-4 pt-4">
          <button type="button" @click="$emit('close')" class="bg-gray-200 text-gray-800 px-4 py-2 rounded-md hover:bg-gray-300">
            Cancelar
          </button>
          <button type="submit" :disabled="isLoading" class="bg-primary-blue text-white-card px-4 py-2 rounded-md hover:bg-blue-700 disabled:bg-blue-300">
            {{ isLoading ? 'Guardando...' : 'Guardar Cliente' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>