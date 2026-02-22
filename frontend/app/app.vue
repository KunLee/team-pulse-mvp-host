<template>
  <div class="app">
    <header class="navbar">
      <div class="navbar-inner">
        <span class="brand">Team Pulse</span>
        <nav>
          <NuxtLink to="/" class="nav-link">Submit Pulse</NuxtLink>
          <NuxtLink to="/summary" class="nav-link">Summary</NuxtLink>
        </nav>
      </div>
    </header>

    <main class="page-content">
      <NuxtPage />
    </main>
  </div>
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  background: #f5f6fa;
  color: #1a1a2e;
  -webkit-font-smoothing: antialiased;
}

.navbar {
  background: #1a1a2e;
  padding: 0 2rem;
  height: 60px;
  display: flex;
  align-items: center;
}

.navbar-inner {
  width: 100%;
  max-width: 1100px;
  margin: 0 auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.brand {
  color: #fff;
  font-size: 1.2rem;
  font-weight: 700;
  letter-spacing: 0.5px;
}

nav {
  display: flex;
  gap: 1.5rem;
}

.nav-link {
  color: #aab0c6;
  text-decoration: none;
  font-size: 0.95rem;
  transition: color 0.2s;
}

.nav-link:hover,
.nav-link.router-link-active {
  color: #e07b39;
}

.page-content {
  max-width: 1100px;
  margin: 2.5rem auto;
  padding: 0 1.5rem;
}
</style>