<template> 
  <div id="app" class="bg-light"> 
    <nav class="navbar navbar-expand-lg navbar-light bg-white mb-4 shadow-sm"> 
      <div class="container"> 
        <router-link to="/" class="navbar-brand">Home</router-link> 
        <router-link v-if="isAuthenticated" to="/calculate-parcel-price" class="nav-link">Calculate Parcel Price</router-link> 
        <router-link v-if="!isAuthenticated" to="/login" class="nav-link">Login</router-link> 
        <router-link v-if="!isAuthenticated" to="/register" class="nav-link">Register</router-link> 
        <a v-if="isAuthenticated" @click="logout" class="nav-link" style="cursor: pointer;">Logout</a> 
      </div> 
    </nav> 
    <router-view /> 
  </div> 
</template> 

<script> 
  import { computed } from 'vue'; 
  import { useStore } from 'vuex'; 
  import { useRouter } from 'vue-router';
  
  export default 
  { 
    name: 'AppRouter', 
    setup() 
    { 
      const store = useStore(); 
      const router = useRouter();
      const isAuthenticated = computed(() => !!store.state.token); 
      const logout = () => 
      { 
        store.dispatch('logout');
        router.push('/login'); 
      }; 
      return { isAuthenticated, logout }; 
    } 
  }; 
</script>

<style> 
.navbar .nav-link { 
  margin-right: 20px; 
  font-weight: bold; 
  color: #4CAF50; 
} 

.navbar .nav-link:hover { 
  color: #45a049; 
}

nav { 
  display: flex; 
  justify-content: center; 
  margin-bottom: 20px; 
} 

a.router-link-active { 
  text-decoration: none; 
  font-weight: bold; 
  color: #4CAF50; 
} 

a.router-link-exact-active { 
  text-decoration: none; 
  font-weight: bold; 
  color: #4CAF50; 
} 

a.router-link-active:hover, a.router-link-exact-active:hover { 
  color: #45a049; 
} 

nav a { 
  text-decoration: none; 
  color: #4CAF50; 
  margin-right: 50px; 
} 

nav a:hover { 
  color: #45a049; 
} 
</style>