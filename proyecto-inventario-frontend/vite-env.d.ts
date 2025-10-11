/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_API_URL: string;
  // Aquí puedes agregar más variables de entorno si las necesitas en el futuro
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}