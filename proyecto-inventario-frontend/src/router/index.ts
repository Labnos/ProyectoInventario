import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'; // 1. Importa RouteRecordRaw
import LoginView from '../views/LoginView.vue';
import DashboardView from '../views/DashboardView.vue';
import InventarioView from '../views/InventarioView.vue';
import VentasView from '../views/VentasView.vue';
import ClientesView from '../views/ClientesView.vue';
import ProveedoresView from '../views/ProveedoresView.vue';
import ReportesView from '../views/ReportesView.vue';
import PromocionesView from '../views/PromocionesView.vue';

// 2. Añade el tipo explícito al arreglo de rutas
const routes: readonly RouteRecordRaw[] = [
  { path: '/', redirect: '/login' },
  { path: '/login', name: 'Login', component: LoginView },
  { path: '/dashboard', name: 'Dashboard', component: DashboardView, meta: { title: 'Dashboard' } },
  { path: '/inventario', name: 'Inventario', component: InventarioView, meta: { title: 'Inventario' } },
  { path: '/ventas', name: 'Ventas', component: VentasView, meta: { title: 'Ventas' } },
  { path: '/clientes', name: 'Clientes', component: ClientesView, meta: { title: 'Clientes' } },
  { path: '/proveedores', name: 'Proveedores', component: ProveedoresView, meta: { title: 'Proveedores' } },
  { path: '/reportes', name: 'Reportes', component: ReportesView, meta: { title: 'Reportes' } },
  { path: '/promociones', name: 'Promociones', component: PromocionesView, meta: { title: 'Promociones' } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});


router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.path !== '/login' && !token) {
    next('/login');
  } else {
    next();
  }
});

export default router;