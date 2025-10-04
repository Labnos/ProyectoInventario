<script setup lang="ts">
import { ref, watch } from 'vue';
import api from '../../services/api';

interface Promocion {
  id?: number;
  nombre: string;
  descripcion: string;
  porcentajeDescuento: number;
  fechaInicio: string;
  fechaFin: string;
  activa: boolean;
}

const props = defineProps<{
  isOpen: boolean;
  promotion: Promocion | null;
}>();

const emit = defineEmits(['close', 'saved']);

const form = ref<Promocion>({
  nombre: '',
  descripcion: '',
  porcentajeDescuento: 0,
  fechaInicio: new Date().toISOString().split('T')[0],
  fechaFin: new Date().toISOString().split('T')[0],
  activa: true,
});

watch(() => props.isOpen, (newVal) => {
  if (newVal) {
    if (props.promotion) {
      form.value = { 
        ...props.promotion,
        fechaInicio: new Date(props.promotion.fechaInicio).toISOString().split('T')[0],
        fechaFin: new Date(props.promotion.fechaFin).toISOString().split('T')[0],
      };
    } else {
      form.value = {
        nombre: '',
        descripcion: '',
        porcentajeDescuento: 0,
        fechaInicio: new Date().toISOString().split('T')[0],
        fechaFin: new Date().toISOString().split('T')[0],
        activa: true,
      };
    }
  }
});

const handleSubmit = async () => {
  try {
    if (form.value.id) {
      await api.put(`/api/promociones/${form.value.id}`, form.value);
    } else {
      await api.post('/api/promociones', form.value);
    }
    emit('saved');
  } catch (error) {
    console.error('Error al guardar la promoción', error);
  }
};
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white-card p-8 rounded-lg shadow-xl w-full max-w-lg">
      <h2 class="text-2xl font-bold font-serif text-text-dark mb-6">{{ promotion ? 'Editar' : 'Crear' }} Promoción</h2>
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label class="block text-sm font-medium">Nombre</label>
          <input v-model="form.nombre" type="text" required class="mt-1 w-full border rounded-md p-2" />
        </div>
        <div>
          <label class="block text-sm font-medium">Porcentaje de Descuento (%)</label>
          <input v-model.number="form.porcentajeDescuento" type="number" required class="mt-1 w-full border rounded-md p-2" />
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium">Fecha de Inicio</label>
            <input v-model="form.fechaInicio" type="date" required class="mt-1 w-full border rounded-md p-2" />
          </div>
          <div>
            <label class="block text-sm font-medium">Fecha de Fin</label>
            <input v-model="form.fechaFin" type="date" required class="mt-1 w-full border rounded-md p-2" />
          </div>
        </div>
        <div class="flex items-center">
          <input v-model="form.activa" type="checkbox" class="h-4 w-4 rounded" />
          <label class="ml-2 block text-sm">Activa</label>
        </div>
        <div class="flex justify-end space-x-4 pt-4">
          <button type="button" @click="$emit('close')" class="bg-gray-200 px-4 py-2 rounded-md">Cancelar</button>
          <button type="submit" class="bg-primary-blue text-white-card px-4 py-2 rounded-md">Guardar</button>
        </div>
      </form>
    </div>
  </div>
</template>