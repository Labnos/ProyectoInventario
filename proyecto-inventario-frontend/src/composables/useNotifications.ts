import { ref, readonly } from 'vue';

interface Notification {
  id: number;
  message: string;
  type: 'success' | 'error';
}

const notifications = ref<Notification[]>([]);

export function useNotifications() {
  const addNotification = (message: string, type: 'success' | 'error' = 'success') => {
    const id = Date.now();
    notifications.value.push({ id, message, type });

    setTimeout(() => {
      notifications.value = notifications.value.filter(n => n.id !== id);
    }, 3000); // La notificación desaparece después de 3 segundos
  };

  return {
    notifications: readonly(notifications),
    addNotification,
  };
}