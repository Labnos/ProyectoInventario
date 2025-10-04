/** @type {import('tailwindcss').Config} */
export default {
  // Archivos que Tailwind debe escanear para encontrar las clases que se están usando.
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],

  // Aquí es donde personalizamos el diseño.
  theme: {
    extend: {
      // Definimos nuestra paleta de colores personalizada.
      colors: {
        // Paleta Principal
        'primary-blue': '#3B82F6', // Azul para acciones principales y enlaces.

        // Tonos Neutros para estructura y texto
        'text-dark': '#1F2937',     // Gris oscuro para el texto principal.
        'background-light': '#F3F4F6', // Gris claro para el fondo general de la página.
        'white-card': '#FFFFFF',      // Blanco para el fondo de tarjetas y formularios.

        // Colores de Acento para comunicar estados del sistema
        'accent-green': '#10B981',    // Verde para mensajes de éxito y estados positivos.
        'accent-red': '#EF4444',      // Rojo para errores, alertas críticas y acciones destructivas.
      },

      // Definimos nuestra tipografía personalizada.
      fontFamily: {
        'sans': ['Roboto', 'sans-serif'], // Fuente principal para la interfaz y el texto.
        'serif': ['Roboto Slab', 'serif'], // Fuente para títulos principales.
      }
    },
  },

  // Plugins adicionales de Tailwind (por ahora no necesitamos ninguno).
  plugins: [],
}