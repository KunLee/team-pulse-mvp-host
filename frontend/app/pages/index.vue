<template>
  <div class="submit-page">
    <div class="card">
      <h2>How Are You Feeling Today?</h2>

      <div class="score-selector">
        <button
          v-for="n in 5"
          :key="n"
          class="score-btn"
          :class="{ active: form.score === n }"
          @click="form.score = n"
        >
          {{ n }}
        </button>
      </div>

      <div class="field">
        <select v-model="form.categoryId" class="input">
          <option value="" disabled>Pulse Category</option>
          <option v-for="cat in categories" :key="cat.id" :value="cat.id">
            {{ cat.name }}
          </option>
        </select>
      </div>

      <div class="field">
        <textarea
          v-model="form.comment"
          class="input"
          placeholder="Optional comment"
          rows="3"
        />
      </div>

      <p v-if="successMessage" class="success-msg">
        <svg class="tick" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
          <circle cx="10" cy="10" r="9" stroke="#2e7d32" stroke-width="1.5"/>
          <path d="M6 10l3 3 5-5" stroke="#2e7d32" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
        {{ successMessage }}
      </p>
      <p v-if="errorMessage" class="error-msg">{{ errorMessage }}</p>

      <button class="submit-btn" :disabled="!canSubmit || submitting" @click="submitPulse">
        {{ submitting ? 'Submitting...' : 'Submit Pulse' }}
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Category {
  id: string
  name: string
}

const { public: { apiBase } } = useRuntimeConfig()
const api = (path: string) => apiBase ? `${apiBase}${path}` : path

const categories = ref<Category[]>([])
const submitting = ref(false)
const successMessage = ref('')
const errorMessage = ref('')

const form = reactive({
  score: 0,
  categoryId: '',
  comment: ''
})

const canSubmit = computed(() => form.score > 0 && form.categoryId !== '')

onMounted(async () => {
  try {
    categories.value = await $fetch<Category[]>(api('/api/pulse/categories'))
  } catch {
    errorMessage.value = 'Failed to load categories. Is the backend running?'
  }
})

async function submitPulse() {
  submitting.value = true
  successMessage.value = ''
  errorMessage.value = ''

  try {
    await $fetch(api('/api/pulse'), {
      method: 'POST',
      body: {
        score: form.score,
        categoryId: form.categoryId,
        comment: form.comment || null
      }
    })
    successMessage.value = 'Pulse submitted — thank you!'
    form.score = 0
    form.categoryId = ''
    form.comment = ''
  } catch {
    errorMessage.value = 'Something went wrong. Please try again.'
  } finally {
    submitting.value = false
  }
}
</script>

<style scoped>
.submit-page {
  display: flex;
  justify-content: center;
}

.card {
  background: #fff;
  border-radius: 12px;
  padding: 2rem;
  width: 100%;
  max-width: 420px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.08);
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

h2 {
  font-size: 1.5rem;
  font-weight: 800;
  color: #1a1a2e;
  line-height: 1.3;
}

.score-selector {
  display: flex;
  gap: 0.75rem;
}

.score-btn {
  flex: 1;
  aspect-ratio: 1;
  border-radius: 50%;
  border: 2px solid #dde1ec;
  background: #fff;
  font-size: 1.1rem;
  font-weight: 700;
  color: #1a1a2e;
  cursor: pointer;
  transition: all 0.15s;
  font-family: inherit;
}

.score-btn:hover {
  border-color: #e07b39;
  color: #e07b39;
}

.score-btn.active {
  background: #e07b39;
  border-color: #e07b39;
  color: #fff;
}

.input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #dde1ec;
  border-radius: 8px;
  font-size: 1rem;
  color: #1a1a2e;
  outline: none;
  transition: border-color 0.15s;
  font-family: inherit;
  resize: none;
  background: #fff;
}

.input:focus {
  border-color: #e07b39;
}

.submit-btn {
  background: #e07b39;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 0.9rem;
  font-size: 1.05rem;
  font-weight: 700;
  cursor: pointer;
  transition: background 0.15s;
  font-family: inherit;
  letter-spacing: 0.3px;
}

.submit-btn:hover:not(:disabled) {
  background: #c96a2a;
}

.submit-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.success-msg {
  color: #2e7d32;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  gap: 0.4rem;
}

.tick {
  width: 18px;
  height: 18px;
  flex-shrink: 0;
}

.error-msg {
  color: #c62828;
  font-size: 0.9rem;
}
</style>