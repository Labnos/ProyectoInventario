import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../views/LoginView.vue';
import DashboardView from '../views/DashboardView.vue';

// --- 1. Importa todas las nuevas vistas ---
import InventarioView from '../views/InventarioView.vue';
import VentasView from '../views/VentasView.vue';
import ClientesView from '../views/ClientesView.vue';
import ProveedoresView from '../views/ProveedoresView.vue';
import ReportesView from '../views/ReportesView.vue';
import PromocionesView from '../views/PromocionesView.vue';

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', name: 'Login', component: LoginView },
  { path: '/dashboard', name: 'Dashboard', component: DashboardView },
  
  // --- 2. Añade las nuevas rutas a la lista ---
  { path: '/inventario', name: 'Inventario', component: InventarioView },
  { path: '/ventas', name: 'Ventas', component: VentasView },
  { path: '/clientes', name: 'Clientes', component: ClientesView },
  { path: '/proveedores', name: 'Proveedores', component: ProveedoresView },
  { path: '/reportes', name: 'Reportes', component: ReportesView },
  { path: '/promociones', name: 'Promociones', component: PromocionesView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Tu guarda de navegación está perfecta, no necesita cambios.
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.path !== '/login' && !token) {
    next('/login');
  } else {
    next();
  }
});

export default router;