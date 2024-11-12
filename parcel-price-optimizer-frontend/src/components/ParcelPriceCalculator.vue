<template> 
    <div class="container mt-4"> 
        <h1 class="text-center">Calculate Parcel Price</h1> 
        <form @submit.prevent="calculatePrice" class="p-4 border rounded bg-white shadow"> 
            <div class="mb-3"> 
                <label for="width" class="form-label">Width (cm):</label> 
                <input type="number" v-model.number="width" class="form-control" required min="1" @blur="validateNumberInput('width')" /> 
            </div> 
            <div class="mb-3"> 
                <label for="height" class="form-label">Height (cm):</label> 
                <input type="number" v-model.number="height" class="form-control" required min="1" @blur="validateNumberInput('height')" /> 
            </div> 
            <div class="mb-3"> 
                <label for="depth" class="form-label">Depth (cm):</label> 
                <input type="number" v-model.number="depth" class="form-control" required min="1" @blur="validateNumberInput('depth')" /> 
            </div> 
            <div class="mb-3"> 
                <label for="weight" class="form-label">Weight (kg):</label> 
                <input type="number" v-model.number="weight" class="form-control" required min="1" @blur="validateNumberInput('weight')" /> 
            </div> 
            <input type="hidden" v-model="userId" /> 
            <button type="submit" class="btn btn-success w-100">
                <i class="fas fa-calculator me-2"></i> Calculate Price</button> 
        </form> 
        <div v-if="price !== null" class="mt-4 text-center"> 
            <h2 v-if="price === -1">The parcel does not meet the criteria for any company.</h2> 
            <h2 v-else>Price: €{{ price }}</h2> 
            <button v-if="price !== -1" @click="proceedToPayment" class="btn btn-primary mt-3">Proceed to Payment</button> 
        </div> 
    </div> 
</template>

<script> 
    import axios from 'axios'; 
    
    export default { 
        name: 'ParcelPriceCalculator', 
        
        data() 
        { 
            return { 
                width: null, 
                height: null, 
                depth: null, 
                weight: null, 
                price: null, 
                userId: this.getUserIdFromToken(),
            }; 
        }, 
        mounted() 
        { 
            const token = localStorage.getItem('token'); 
            
            if (token) 
            { 
                const decodedToken = JSON.parse(atob(token.split('.')[1])); 
                this.userId = decodedToken.sub; 
                console.log('UserId fetched from token:', this.userId); 
            } 
            else 
            { 
                console.error('No token found in localStorage'); 
            } 
        }, methods: 
        { 
            validateNumberInput(field) 
            {
                if (this[field] !== null && this[field] < 1) 
                {
                    alert('Please enter a value greater than 0.');
                    this[field] = null;
                }
            },
            getUserIdFromToken() 
            { 
                const token = localStorage.getItem('token'); 
                if (token) 
                { 
                    const decodedToken = JSON.parse(atob(token.split('.')[1])); 
                    const exp = decodedToken.exp * 1000; 
                    if (Date.now() >= exp) 
                    { 
                        console.error('Token has expired.'); 
                        this.refreshToken();
                        return null; 
                    } 
                    return decodedToken.sub; 
                } 
                else 
                { 
                    console.error('No token found in localStorage'); 
                    return null; 
                } 
            },
            async refreshToken() 
            { 
                const token = localStorage.getItem('token'); 
                if (!token) 
                    return; 
                try 
                { 
                    const response = await axios.post('/api/auth/refresh', 
                    { 
                        token 
                    }); 
                    if (response.data && response.data.newToken) 
                    { 
                        localStorage.setItem('token', response.data.newToken); 
                        console.log('Token refreshed successfully'); 
                        this.setUserId(); 
                    }
                    else 
                    { 
                        console.error('Failed to refresh token.');
                    } 
                } 
                catch (error) 
                { 
                    console.error('Error refreshing token:', error); 
                } 
            },
            async calculatePrice() 
            { 
                this.userId = this.getUserIdFromToken();
                if (!this.userId) 
                { 
                    console.error('User ID is null. User may not be logged in.'); 
                    return; 
                }

                if ([this.width, this.height, this.depth, this.weight].some(val => val <= 0)) 
                {
                    alert('All input values must be greater than 0.');
                    return;
                }

                try 
                { 
                    console.log("Sending request to calculate price with data:", 
                    { 
                        width: this.width, 
                        height: this.height, 
                        depth: this.depth, 
                        weight: this.weight, 
                        userId: this.userId,
                    }); 
                    
                    if (!this.width || !this.height || !this.depth || !this.weight || !this.userId) 
                    { 
                        console.error('Invalid input data:', 
                        { 
                            width: this.width, 
                            height: this.height, 
                            depth: this.depth, 
                            weight: this.weight, 
                            userId: this.userId, 
                        }); 
                        return; 
                    } 
                    const response = await axios.post('https://localhost:7084/api/parcel/calculate', 
                    { 
                        width: this.width, 
                        height: this.height, 
                        depth: this.depth, 
                        weight: this.weight, 
                        userId: this.userId, 
                    }); 
                    console.log("Response received:", response); 
                    
                    if (response && response.data) 
                    { 
                        this.price = response.data.price; 
                        console.log("Price calculated successfully:", this.price); 
                    } 
                    else 
                    { 
                        console.error('Error: Calculate price response data is undefined'); 
                    } 
                } 
                catch (error) 
                { 
                    if (error.response) 
                    { 
                        console.error('Error response data:', error.response.data); 
                        console.error('Error response status:', error.response.status); 
                        console.error('Error response headers:', error.response.headers); 
                    } 
                    else if (error.request) 
                    { 
                        console.error('Error request data:', error.request); 
                    } 
                    else 
                    { 
                        console.error('Error message:', error.message); 
                    } 
                } 
            }, 
            async proceedToPayment() 
            { 
                this.userId = this.getUserIdFromToken();
                if (!this.userId) 
                { 
                    console.error('User ID is null. User may not be logged in.'); 
                    return; 
                }
                try 
                { 
                    const response = await axios.post('https://localhost:7084/api/payment/create-session', 
                    { 
                        price: this.price, 
                        userId: this.userId 
                    }); 
                    if (response.data && response.data.url) 
                    { 
                        window.location.href = response.data.url; 
                    } 
                    else 
                    { 
                        console.error('Error: Stripe session creation failed.'); 
                    } 
                } 
                catch (error) 
                { 
                    console.error('Error proceeding to payment:', error); 
                } 
            }, 
        }, 
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