<template> 
    <div class="container mt-5"> 
        <h1 class="text-center">Welcome to Parcel Price Optimizer</h1> 
        <p class="lead text-center mt-3">Optimize your shipping costs with ease!</p> 
        <p class="text-center"> Use our tools to calculate the best shipping price for your parcels and find the optimal shipping options available. </p> 
        <div class="text-center mt-4"> 
            <router-link to="/calculate-parcel-price" class="btn btn-primary m-2">Calculate Parcel Price</router-link> 
            <router-link to="/calculate-optimal-price" class="btn btn-success m-2">Calculate Optimal Price</router-link> 
        </div> 
    </div> 
</template> 

<script> 
    export default { name: 'HomeView' }; 
</script> 

<style scoped> 
.container { 
    max-width: 800px; 
    margin: 0 auto; 
    padding: 20px; 
    background: #f7f7f7; 
    border-radius: 8px; 
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); 
} 

h1 { 
    color: #333; 
} 

.lead { 
    font-size: 1.25rem; 
    font-weight: 300; 
} 
.btn { 
    padding: 10px 20px; 
    font-size: 1rem; 
} 
</style>