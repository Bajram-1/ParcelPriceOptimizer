import 'bootstrap/dist/css/bootstrap.min.css';
import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import axios from 'axios';
import '@fortawesome/fontawesome-free/css/all.css';
import '@fortawesome/fontawesome-free/js/all.js';
import 'bootstrap-icons/font/bootstrap-icons.css';
import Toast from "vue-toastification"; 
import "vue-toastification/dist/index.css";

axios.defaults.baseURL = 'https://localhost:7084';
axios.defaults.headers.common['Content-Type'] = 'application/json';

const app = createApp(App); 
app.use(router); 
app.use(store);

app.use(Toast, 
{ 
    position: 'top-right', 
    timeout: 3000, 
    closeOnClick: true, 
    pauseOnFocusLoss: true, 
    pauseOnHover: true, 
    draggable: true, 
    draggablePercent: 0.6, 
    showCloseButtonOnHover: false, 
    hideProgressBar: false, 
    closeButton: 'button', 
    icon: true,
    rtl: false
});

createApp(App).use(router).use(store).mount('#app')
