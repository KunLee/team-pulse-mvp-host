<template>
  <div class="summary-page">
    <h1>Team Pulse Summary</h1>

    <div v-if="pending" class="loading">Loading...</div>
    <div v-else-if="error" class="error-msg">Failed to load summary. Is the backend running?</div>

    <template v-else-if="summary">
      <!-- Stats row -->
      <div class="stats-row">
        <div class="stat-card">
          <p class="stat-label">Total Submissions</p>
          <p class="stat-value">{{ summary.count }}</p>
        </div>
        <div class="stat-card">
          <p class="stat-label">Average Score</p>
          <p class="stat-value">{{ summary.averageScore }}</p>
        </div>
        <div class="stat-card">
          <p class="stat-label">Most Common Score</p>
          <p class="stat-value">{{ mostCommonScore }}</p>
        </div>
      </div>

      <!-- Score distribution chart -->
      <div class="card">
        <h2>Score Distribution</h2>
        <div class="chart-wrapper">
          <div
            v-for="(count, score) in summary.scores"
            :key="score"
            class="bar-group"
          >
            <span class="bar-count">{{ count }}</span>
            <div class="bar-track">
              <div
                class="bar-fill"
                :class="{ 'bar-fill--highlight': isHighest(count) }"
                :style="{ height: barHeight(count) + '%' }"
              />
            </div>
            <span class="bar-label">{{ score }}</span>
          </div>
        </div>
        <p class="chart-note">Aggregated results only. Individual submissions remain anonymous.</p>
      </div>

      <!-- Category breakdown -->
      <div class="card">
        <h2>By Category</h2>
        <table class="category-table">
          <thead>
            <tr>
              <th>Category</th>
              <th>Submissions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="cat in summary.categories" :key="cat.id">
              <td>{{ cat.name }}</td>
              <td>{{ cat.count }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
interface CategorySummary {
  id: string
  name: string
  count: number
}

interface Summary {
  count: number
  averageScore: number
  scores: Record<string, number>
  categories: CategorySummary[]
}

const { public: { apiBase } } = useRuntimeConfig()
const { data: summary, pending, error } = await useFetch<Summary>(
  apiBase ? `${apiBase}/api/pulse/summary` : '/api/pulse/summary'
)

const mostCommonScore = computed(() => {
  if (!summary.value) return '-'
  const scores = summary.value.scores
  return Object.entries(scores).reduce((a, b) => (b[1] > a[1] ? b : a))[0]
})

const maxCount = computed(() => {
  if (!summary.value) return 1
  return Math.max(...Object.values(summary.value.scores), 1)
})

function barHeight(count: number): number {
  return Math.round((count / maxCount.value) * 100)
}

function isHighest(count: number): boolean {
  return count === maxCount.value && count > 0
}
</script>

<style scoped>
.summary-page {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

h1 {
  font-size: 1.8rem;
  font-weight: 800;
  color: #1a1a2e;
}

.loading {
  color: #888;
}

.error-msg {
  color: #c62828;
}

.stats-row {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
}

.stat-card {
  background: #fff;
  border-radius: 12px;
  padding: 1.75rem 1.5rem 1.5rem;
  box-shadow: 0 2px 12px rgba(0,0,0,0.07);
  text-align: center;
  position: relative;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.stat-card::before {
  content: '';
  position: absolute;
  top: -12px;
  left: 0;
  right: 0;
  height: 4px;
  background: #e07b39;
  border-radius: 3px;
  transition: opacity 0.2s ease, transform 0.2s ease;
  transform: scaleX(0.6);
  opacity: 0;
}

.stat-card:hover::before {
  opacity: 1;
  transform: scaleX(1);
}

.stat-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(0,0,0,0.12);
}

.stat-label {
  font-size: 0.78rem;
  color: #aaa;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin-bottom: 0.6rem;
  font-weight: 500;
}

.stat-value {
  font-size: 2.4rem;
  font-weight: 800;
  color: #1a1a2e;
  line-height: 1;
}

.card {
  background: #fff;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 12px rgba(0,0,0,0.07);
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

h2 {
  font-size: 1.15rem;
  font-weight: 700;
  color: #1a1a2e;
}

.chart-wrapper {
  display: flex;
  align-items: flex-end;
  gap: 1.5rem;
  padding: 0 0.5rem;
}

.bar-group {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.4rem;
  flex: 1;
}

.bar-track {
  width: 100%;
  height: 120px;
  border-radius: 6px;
  display: flex;
  align-items: flex-end;
  overflow: hidden;
  background: #f0f2f8;
}

.bar-fill {
  width: 100%;
  background: #2e3561;
  border-radius: 6px 6px 0 0;
  transition: height 0.4s ease;
  min-height: 4px;
}

.bar-fill--highlight {
  background: #e07b39;
}

.bar-count {
  font-size: 0.8rem;
  color: #555;
  font-weight: 600;
}

.bar-label {
  font-size: 0.85rem;
  font-weight: 700;
  color: #1a1a2e;
}

.chart-note {
  font-size: 0.75rem;
  color: #aaa;
}

.category-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
}

.category-table th {
  text-align: left;
  padding: 0.6rem 0.75rem;
  border-bottom: 2px solid #f0f2f8;
  color: #888;
  font-size: 0.8rem;
  text-transform: uppercase;
}

.category-table td {
  padding: 0.65rem 0.75rem;
  border-bottom: 1px solid #f0f2f8;
}

@media (max-width: 480px) {
  .stats-row {
    gap: 0.5rem;
  }

  .stat-card {
    padding: 1.25rem 0.75rem 1rem;
  }

  .stat-value {
    font-size: 1.8rem;
  }

  .stat-label {
    font-size: 0.65rem;
  }
}
</style>