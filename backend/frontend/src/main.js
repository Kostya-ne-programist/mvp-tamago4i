import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import AOS from 'aos'

import 'bootswatch/dist/zephyr/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import '@fortawesome/fontawesome-free/css/all.min.css'
import 'aos/dist/aos.css'

const app = createApp(App)

app.use(AOS)
app.use(createPinia())
app.use(router)

AOS.init();

app.mount('#app')
