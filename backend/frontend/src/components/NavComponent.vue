<script setup>

</script>

<template>
  <div data-aos="flip-down" class="container text-center">
    <nav class="navbar navbar-expand-sm m-3">
      <router-link class="navbar-brand d-flex flex-row" to="/home">
        <i class="fa-solid fa-folder fa-beat me-3"></i>
        <h4 class="font-monospace">MVP</h4>
      </router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav mx-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/"><i class="fa-solid fa-house me-1"></i>Головна</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/main"><i class="fa-solid fa-brain me-1"></i>Логіка</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/about"><i class="fa-solid fa-info-circle me-1"></i>Про нас</router-link>
          </li>
        </ul>
      </div>
    </nav>
  </div>
</template>

<style scoped>

</style>
