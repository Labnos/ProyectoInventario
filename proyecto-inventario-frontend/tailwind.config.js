/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'primary-blue': '#3B82F6',
        'secondary-blue': '#93C5FD',
        'background-light': '#F3F4F6', // Fondo principal de la app
        'white-card': '#FFFFFF',      // Fondo para tarjetas y modales
        'text-dark': '#1F2937',        // Texto principal oscuro
        'accent-red': '#EF4444',
        'accent-green': '#10B981',
      },
      fontFamily: {
        serif: ['Roboto Slab', 'serif'], // Para títulos
        sans: ['Roboto', 'sans-serif'],   // Para el resto del texto
      },
    },
  },
  plugins: [],
}