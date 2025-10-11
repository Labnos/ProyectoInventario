<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import api from '@/services/api';
import { useNotifications } from '@/composables/useNotifications';

const email = ref('');
const password = ref('');
const isLoading = ref(false);
const router = useRouter();
const { addNotification } = useNotifications();

const handleLogin = async () => {
  isLoading.value = true;
  try {
    const response = await api.post('/login', {
      email: email.value,
      password: password.value,
    });
    localStorage.setItem('authToken', response.data.token);
    api.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
    router.push('/dashboard');
  } catch (error) {
    addNotification('Credenciales incorrectas. Por favor, inténtalo de nuevo.', 'error');
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-[#FDF0E1] to-[#FFE5CC] p-4 font-sans">
    <div class="relative bg-white rounded-xl shadow-2xl w-full max-w-sm overflow-hidden border-t-4 border-[#FF8C00] transition-transform hover:scale-[1.01]">
      <div class="absolute bottom-0 left-0 w-full h-4 bg-[#FF8C00] -z-10"></div>
      <div class="absolute bottom-0 left-0 w-full h-2 bg-[#F28500] -z-10 translate-y-2"></div>

      <div class="p-8 pb-12">
        <h2 class="text-3xl font-extrabold text-center text-gray-800 mb-8 uppercase tracking-wide">Login</h2>

        <form @submit.prevent="handleLogin" class="space-y-6">
          <!-- Email -->
          <div>
            <label for="email" class="sr-only">Email ID</label>
            <div class="relative">
              <span class="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-400">
                <font-awesome-icon :icon="['fas', 'envelope']" />
              </span>
              <input
                id="email"
                v-model="email"
                type="email"
                required
                autocomplete="email"
                placeholder="Email ID"
                class="input-field"
                aria-label="Email ID"
              />
            </div>
          </div>

          <!-- Password -->
          <div>
            <label for="password" class="sr-only">Password</label>
            <div class="relative">
              <span class="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-400">
                <font-awesome-icon :icon="['fas', 'lock']" />
              </span>
              <input
                id="password"
                v-model="password"
                type="password"
                required
                autocomplete="current-password"
                placeholder="Password"
                class="input-field"
                aria-label="Password"
              />
              <a href="#" class="absolute inset-y-0 right-0 flex items-center pr-3 text-[#FF8C00] hover:text-[#F28500] text-sm font-semibold">
                Forgot?
              </a>
            </div>
          </div>

          <!-- Submit -->
          <div class="pt-4">
            <button
              type="submit"
              :disabled="isLoading"
              class="w-full flex justify-center py-3 px-4 rounded-full text-lg font-bold text-white bg-gradient-to-r from-[#FF8C00] to-[#F28500] hover:from-[#F28500] hover:to-[#FF8C00] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[#FF8C00] disabled:bg-gray-400 transition-all shadow-lg"
            >
              {{ isLoading ? 'Logging in...' : 'Login' }}
            </button>
          </div>
        </form>

        <!-- Footer -->
        <div class="text-center mt-8 text-gray-600 text-sm">
          Don't have an account?
          <a href="#" class="text-[#FF8C00] hover:text-[#F28500] font-semibold">Sign Up</a>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.input-field {
  @apply w-full pl-10 pr-4 py-3 border-b border-gray-300 bg-transparent text-gray-700 placeholder-gray-400 transition-colors;
  @apply focus:outline-none focus:border-b-2 focus:border-[#FF8C00];
}
</style>
