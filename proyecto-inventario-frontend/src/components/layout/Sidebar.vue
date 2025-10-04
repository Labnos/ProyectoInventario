<script setup lang="ts">
import { RouterLink } from 'vue-router';

// Lista de enlaces para el menú
const navLinks = [
  { name: 'Dashboard', path: '/dashboard' },
  { name: 'Ventas', path: '/ventas' },
  { name: 'Inventario', path: '/inventario' },
  { name: 'Clientes', path: '/clientes' },
  { name: 'Proveedores', path: '/proveedores' },
  { name: 'Reportes', path: '/reportes' },
  { name: 'Promociones', path: '/promociones' },
];
</script>

<template>
  <aside class="w-64 bg-white-card border-r border-gray-200 flex flex-col">
    <div class="p-4 border-b border-gray-200">
      <h2 class="text-xl font-bold font-serif text-text-dark">Comerciales Emilias</h2>
    </div>
    
    <nav class="flex-1 p-4">
      <ul>
        <li v-for="link in navLinks" :key="link.name">
          <RouterLink
            :to="link.path"
            class="block px-4 py-2 rounded-md text-gray-600 hover:bg-gray-100 hover:text-text-dark transition-colors"
            active-class="bg-blue-100 text-primary-blue font-semibold"
          >
            {{ link.name }}
          </RouterLink>
        </li>
      </ul>
    </nav>

    <div class="p-4 border-t border-gray-200">
      <button class="w-full px-4 py-2 text-left text-sm text-gray-600 hover:bg-gray-100 rounded-md">
        Cerrar Sesión
      </button>
    </div>
  </aside>
</template>