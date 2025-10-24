import axios from 'axios';
import router from '../router'; // Importa el router para poder redirigir
import { useNotifications } from '@/composables/useNotifications'; // Importa el composable de notificaciones

// Extrae la función addNotification para usarla fácilmente
const { addNotification } = useNotifications();

// 1. Crea la instancia de Axios
const api = axios.create({
  // Lee la URL base de tu API desde las variables de entorno (.env)
  baseURL: (import.meta as any).env?.VITE_API_URL ?? '', 
});

// 2. Interceptor de Peticiones: Se ejecuta ANTES de que cada petición salga
api.interceptors.request.use(config => {
  // Obtiene el token guardado en el localStorage
  const token = localStorage.getItem('token'); 
  
  // Si existe un token, lo añade al encabezado 'Authorization'
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config; // Continúa con la petición
}, error => {
  // Si hay un error al configurar la petición, lo rechaza
  return Promise.reject(error);
});

// 3. Interceptor de Respuestas: Se ejecuta DESPUÉS de recibir cada respuesta del backend
api.interceptors.response.use(response => {
  // Si la respuesta es exitosa (código 2xx), simplemente la devuelve
  return response;
}, error => {
  // Si hay un error en la respuesta...
  if (error.response) {
    // Caso especial: Error 401 Unauthorized (Token inválido o expirado)
    if (error.response.status === 401) {
      // Limpia el token inválido del localStorage
      localStorage.removeItem('token'); 
      // Borra el encabezado de autorización por defecto
      delete api.defaults.headers.common['Authorization'];
      // Muestra una notificación al usuario
      addNotification('Tu sesión ha expirado. Por favor, inicia sesión de nuevo.', 'error');
      // Redirige al usuario a la página de login
      router.push('/login'); 
    } 
    // Puedes añadir manejo para otros códigos de error aquí si lo necesitas
    // else if (error.response.status === 403) { ... } 
  }
  
  // Rechaza la promesa para que el componente que hizo la llamada sepa que hubo un error
  return Promise.reject(error);
});

// Exporta la instancia configurada de Axios para usarla en otros archivos
export default api;