import { createRouter, createWebHistory } from 'vue-router'; 
import HomeView from '../components/Home.vue'; 
import ParcelPriceCalculator from '../components/ParcelPriceCalculator.vue'; 
import OptimalPriceCalculator from '../components/OptimalPriceCalculator.vue'; 
import UserLogin from '../components/UserLogin.vue'; 
import UserRegister from '../components/UserRegister.vue'; 
import store from '../store'; const routes = [ 
   { 
      path: '/', 
      name: 'Home', 
      component: HomeView 
   }, 
   { 
      path: '/calculate-parcel-price', 
      name: 'ParcelPriceCalculator', 
      component: ParcelPriceCalculator 
   }, 
   { 
      path: '/calculate-optimal-price', 
      name: 'OptimalPriceCalculator', 
      component: OptimalPriceCalculator 
   }, 
   { 
      path: '/login', 
      name: 'UserLogin', 
      component: UserLogin 
   }, 
   { 
      path: '/register', 
      name: 'UserRegister', 
      component: UserRegister 
   } 
]; 

const router = createRouter(
   { 
      history: createWebHistory(process.env.BASE_URL), routes 
   }); 
   router.beforeEach((to, from, next) => 
      { 
         const publicPages = ['/login', '/register']; 
         const authRequired = !publicPages.includes(to.path); 
         const loggedIn = store.state.token; 
         
         if (authRequired && !loggedIn) 
         { 
            return next('/login'); 
         } 
         next(); 
      }); 
      
      export default router;