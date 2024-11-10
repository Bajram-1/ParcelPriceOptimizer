<template>
 <div class="container mt-4"> 
    <h1 class="text-center">Calculate Parcel Price</h1> 
    
    <form @submit.prevent="calculatePrice" class="p-4 border rounded bg-white shadow"> 
        <div class="mb-3"> 
            <label for="width" class="form-label">Width (cm):</label> 
            <input type="number" v-model="width" class="form-control" required /> 
        </div> 
        <div class="mb-3"> 
            <label for="height" class="form-label">Height (cm):</label> 
            <input type="number" v-model="height" class="form-control" required /> 
        </div> 
        <div class="mb-3"> 
            <label for="depth" class="form-label">Depth (cm):</label> 
            <input type="number" v-model="depth" class="form-control" required /> 
        </div> 
        <div class="mb-3"> 
            <label for="weight" class="form-label">Weight (kg):</label> 
            <input type="number" v-model="weight" class="form-control" required /> 
        </div> 
        <div class="mb-3"> 
            <label for="customerName" class="form-label">Customer Name:</label> 
            <input type="text" v-model="customerName" class="form-control" required /> 
        </div> 
        <div class="mb-3"> 
            <label for="customerEmail" class="form-label">Customer Email:</label> 
            <input type="email" v-model="customerEmail" class="form-control" required /> 
        </div> 
        
        <button type="submit" class="btn btn-success w-100">Calculate Price</button> 
    </form> 
    <div v-if="price !== null" class="mt-4 text-center"> 
        <h2>Price: €{{ price }}</h2> 
    </div> 
</div> 
</template> 

<script> 
    import axios from 'axios'; 
    
    export default { name: 'ParcelPriceCalculator', data() { return { 
        width: null, 
        height: null, 
        depth: null, 
        weight: null, 
        customerName: '', 
        customerEmail: '', 
        price: null }; 
    }, 
    methods: { async calculatePrice() { try { 
        const response = await axios.post('https://localhost:7084/api/parcel/calculate', { 
            width: this.width, 
            height: this.height, 
            depth: this.depth, 
            weight: this.weight, 
            customerName: this.customerName, 
            customerEmail: this.customerEmail }); 
            this.price = response.data.price; 
            
            await axios.post('https://localhost:7084/api/customerinput/save', { 
                width: this.width, 
                height: this.height, 
                depth: this.depth, 
                weight: this.weight, 
                customerName: this.customerName, 
                customerEmail: this.customerEmail, 
                price: this.price 
            });
        
        } catch (error) { 
            console.error('Error calculating price:', error); 
            } 
        } 
    } 
}; 
</script>

<style scoped> 
.container { 
    max-width: 600px; 
    margin: 0 auto; 
    padding: 20px; 
    background: #f7f7f7; 
    border-radius: 8px; 
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
} 

h1 { 
    text-align: center; 
    color: #333; 
    margin-bottom: 20px; 
} 

.form-group { 
    margin-bottom: 15px; 
} 

label { 
    display: block; 
    margin-bottom: 5px; 
    color: #555; 
} 

input[type="number"], input[type="text"], input[type="email"] { 
    width: 100%; 
    padding: 10px; 
    border: 1px solid #ccc; 
    border-radius: 4px; 
    box-sizing: border-box; 
} 

input[type="number"]:focus, input[type="text"]:focus, input[type="email"]:focus { 
    border-color: #4CAF50; 
} 

.btn-submit { 
    width: 100%; 
    padding: 12px 20px; 
    background-color: #4CAF50; 
    color: white; 
    border: none; 
    border-radius: 4px; 
    cursor: pointer; 
    font-size: 16px; 
    margin-top: 10px; 
} 

.btn-submit:hover { 
    background-color: #45a049; 
} 

.price-result { 
    margin-top: 20px; 
    text-align: center; 
    font-size: 20px; 
    color: #333; 
} 

.price-result h2 { 
    color: #4CAF50; 
} 
</style>