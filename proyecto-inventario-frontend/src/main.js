import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// Importa el CSS principal de Tailwind
import './assets/main.css'

const app = createApp(App)

app.use(router)

app.mount('#app')