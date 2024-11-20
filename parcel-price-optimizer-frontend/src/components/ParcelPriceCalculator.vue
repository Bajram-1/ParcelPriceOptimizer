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
            <button type="submit" class="btn btn-success w-100" :disabled="isLoading">
                <i :class="isLoading ? 'fas fa-spinner fa-spin' : 'fas fa-calculator me-2'"></i> Calculate Price
            </button>
        </form>
        <div v-if="couriers.length > 0" class="mt-4 text-center">
            <h4>Choose a Courier Company:</h4>
            <div class="form-check" v-for="(courier, index) in couriers" :key="index">
                <input
                    type="radio"
                    :id="'courier-' + index"
                    :value="courier"
                    v-model="selectedCourier"
                    class="form-check-input"
                />
                <label :for="'courier-' + index" class="form-check-label">
                    <strong>{{ courier.courier }}</strong>: {{ courier.price }}
                </label>
            </div>
            <button
                v-if="selectedCourier"
                @click="proceedToPayment"
                class="btn btn-primary mt-3"
            >
                <i class="fas fa-credit-card me-2"></i> Proceed to Payment
            </button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { useToast } from 'vue-toastification';

export default {
    name: 'ParcelPriceCalculator',
    data() {
        return {
            width: null,
            height: null,
            depth: null,
            weight: null,
            userId: this.getUserIdFromToken(),
            couriers: [],
            price: null,
            selectedCourier: null,
            isLoading: false,
        };
    },
    mounted() {
        const token = localStorage.getItem('token');
        if (!token) {
            console.error('No token found in localStorage');
            return;
        }

        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const exp = decodedToken.exp * 1000;
        if (Date.now() >= exp) {
            console.error('Token has expired.');
            this.refreshToken();
            return;
        }

        this.userId = decodedToken.sub || null;
        console.log('UserId fetched from token:', this.userId);
    },
    methods: {
        validateNumberInput(field) {
            if (this[field] !== null && this[field] < 1) {
                alert('Please enter a value greater than 0.');
                this[field] = null;
            }
        },
        getUserIdFromToken() {
            const token = localStorage.getItem('token');
            if (!token) {
                console.error('No token found in localStorage');
                return null;
            }
            const decodedToken = JSON.parse(atob(token.split('.')[1]));
            const exp = decodedToken.exp * 1000;
            if (Date.now() >= exp) {
                console.error('Token has expired.');
                this.refreshToken();
                return null;
            }
            return decodedToken.sub || null;
        },
        async refreshToken() {
            const token = localStorage.getItem('token');
            if (!token) {
                alert("You are not logged in. Redirecting to login...");
                this.$router.push('/login');
                return;
            }

            try {
                const response = await axios.post('/api/auth/refresh', { token });
                if (response.data && response.data.newToken) {
                    localStorage.setItem('token', response.data.newToken);
                    console.log('Token refreshed successfully');

                    const decodedToken = JSON.parse(atob(response.data.newToken.split('.')[1]));
                    this.userId = decodedToken.sub || null;
                    console.log('Updated UserId from refreshed token:', this.userId);
                } else {
                    console.error('Failed to refresh token.');
                }
            } catch (error) {
                console.error('Error refreshing token:', error);
            }
        },
        async calculatePrice() {
            const toast = useToast();

            if ([this.width, this.height, this.depth, this.weight].some(val => val <= 0)) {
                alert('All input values must be greater than 0.');
                return;
            }

            this.isLoading = true;
            try {
                console.log('Calculating price with:', {
                    width: this.width,
                    height: this.height,
                    depth: this.depth,
                    weight: this.weight,
                    userId: this.userId,
                });

                const response = await axios.post('https://localhost:7084/api/parcel/calculate', {
                    width: this.width,
                    height: this.height,
                    depth: this.depth,
                    weight: this.weight,
                    userId: this.userId || undefined,
                }, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('token')}`
                    }
                });

                console.log('API response:', response);

                if (Array.isArray(response.data) && response.data.length > 0) 
                {
                    this.couriers = response.data;
                    this.price = Math.max(
                    ...this.couriers.map(c => {
                    if (c.Price && typeof c.Price === 'string') 
                    {    
                        const priceString = c.Price.slice(1);
                        const parsedPrice = parseFloat(priceString);
                        return isNaN(parsedPrice) ? 0 : parsedPrice;
                    }
                    return 0;
                })
            );
                    toast.success('Prices calculated successfully.');
                } else {
                    toast.error('Invalid response data.');
                    console.error('Invalid response format:', response.data);
                }
            } catch (error) {
                console.error('Error calculating price:', error.response ? error.response.data : error.message);
                toast.error(`Error: ${error.response ? error.response.data : error.message}`);
            } finally {
                this.isLoading = false;
            }
        },
        async proceedToPayment() {
            if (!this.selectedCourier) {
                alert("Please select a courier.");
                return;
            }

            // Ensure the price is in a proper numeric format
            const priceString = this.selectedCourier.price;
            const cleanedPriceString = priceString.replace(/[^0-9.-]+/g, '');  // Remove any non-numeric characters (like $)
            const parsedPrice = parseFloat(cleanedPriceString);

            if (isNaN(parsedPrice)) {
                alert("Invalid price selected.");
                return;
            }

            try {
                const response = await axios.post('https://localhost:7084/api/payment/create-session', {
                    price: parsedPrice,
                    userId: this.userId || 'default-user-id',
                    courier: this.selectedCourier.courier,
                });

                if (response.data && response.data.url) {
                    window.location.href = response.data.url;
                } else {
                    console.error('Error: Stripe session creation failed.');
                }
            } catch (error) {
                console.error('Error proceeding to payment:', error);
            }
        }
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
    color: #333;
    margin-bottom: 20px;
}

.form-group {
    margin-bottom: 15px;
}

label {
    margin-bottom: 5px;
    color: #555;
}

input[type="number"] {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

input:focus {
    border-color: #4CAF50;
}

button {
    width: 100%;
    padding: 10px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

button.btn-success {
    background-color: #4CAF50;
    color: white;
}

button.btn-primary {
    background-color: #007BFF;
    color: white;
    margin-top: 10px;
}
</style>