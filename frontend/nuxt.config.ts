// https://nuxt.com/docs/api/configuration/nuxt-config

// Dev proxy target — switch when changing backend:
// https profile (Visual Studio):  https://127.0.0.1:7053
// http profile (Visual Studio):   http://127.0.0.1:5274
// Docker/Podman container:        http://127.0.0.1:8080
const devApiTarget = 'http://127.0.0.1:8080'

export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },

  // Public API base URL — used by frontend API calls in production
  // Override via NUXT_PUBLIC_API_BASE env var on Vercel/hosting platform
  runtimeConfig: {
    public: {
      apiBase: ''   // empty = use proxy in dev, set to full URL in production
    }
  },

  nitro: {
    devProxy: {
      '/api': {
        target: `${devApiTarget}/api`,
        changeOrigin: true,
        prependPath: true
      }
    }
  }
})
