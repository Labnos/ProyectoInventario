<template>
  <div>
    <h2>Iniciar sesión</h2>
    <input v-model="username" placeholder="Usuario" />
    <input v-model="password" type="password" placeholder="Contraseña" />
    <button @click="login">Entrar</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import api from '../services/api';
import { useRouter } from 'vue-router';

const username = ref('');
const password = ref('');
const router = useRouter();

const login = async () => {
  const res = await api.post('/auth/login', { username: username.value, password: password.value });
  localStorage.setItem('token', res.data.token);
  router.push('/dashboard');
};
</script>