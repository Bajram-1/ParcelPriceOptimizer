<template> 
    <div class="login-container"> 
        <div class="login-form"> 
            <h2>Login</h2> <form @submit.prevent="login"> 
                <div class="form-group"> 
                    <label for="email">Email:</label> 
                    <input type="email" id="email" v-model="email" required /> 
                    <span v-if="errors.Email" class="text-danger">{{ errors.Email }}</span>
                </div> 
                <div class="form-group password-group"> 
                    <label for="password">Password:</label> 
                    <input :type="showPassword ? 'text' : 'password'" id="password" v-model="password" required /> 
                    <span class="toggle-password" @click="togglePasswordVisibility"> 
                        <i :class="showPassword ? 'fa fa-eye' : 'fa fa-eye-slash'"></i> 
                    </span> 
                    <span v-if="errors.Password" class="text-danger">{{ errors.Password }}</span>
                </div>
                <button type="submit" class="btn btn-primary">
                    <i class="fas fa-sign-in-alt"></i> Login</button> 
            </form> 
            <div v-if="message" class="alert alert-info">{{ message }}</div>
        </div> 
    </div> 
</template>

<script> 
    import axios from 'axios'; 
    import { ref } from 'vue'; 
    import { useRouter } from 'vue-router'; 
    import store from '../store';
    
    export default { name: 'UserLogin', setup() 
    { 
        const email = ref(''); 
        const password = ref(''); 
        const showPassword = ref(false);
        const message = ref('');
        const errors = ref({});
        const router = useRouter(); 
        
        const login = async () => {
            try {
                const response = await axios.post('https://localhost:7084/api/auth/login', {
                    email: email.value,
                    password: password.value
                });

                console.log('Response:', response);  // Log the whole response

                if (response && response.data) {
                    const token = response.data.token;
                    localStorage.setItem('token', token);
                    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
                    store.commit('setToken', token); // Store token in Vuex
                    console.log('Logged in successfully:', response.data); 
                    router.push('/'); // Redirect to the home page
                } else {
                    console.error('No data in response');
                }
            } catch (error) {
                console.error('Error logging in:', error);
                if (error.response) {
                    console.error('Response error:', error.response.data); // Server error response
                } else if (error.request) {
                    console.error('Request error:', error.request); // No response from server
                } else {
                    console.error('Unexpected error:', error.message); // Any other error
                }
            }
        };

        const togglePasswordVisibility = () => 
        { 
            showPassword.value = !showPassword.value; 
        };

        return { email, password, showPassword, message, errors, login, togglePasswordVisibility }; 
    } 
}; 
</script>

<style scoped> 
.login-container 
{ 
    display: flex; 
    justify-content: center; 
    align-items: center; 
    height: 100vh; 
    background-color: #f8f9fa; 
} 

.login-form 
{ 
    background: #fff; 
    padding: 30px; 
    border-radius: 10px; 
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); 
    height: 400px;
    width: 500px; 
    text-align: center; 
} 

.login-form h2 
{ 
    margin-bottom: 20px; 
    font-size: 50px; 
    color: #333; 
} 

.form-group 
{ 
    margin-bottom: 15px; 
    text-align: left; 
    position: relative;
} 

.form-group label 
{ 
    display: block; 
    margin-bottom: 5px; 
    font-size: 25px;
    color: #555; 
} 

.form-group input 
{ 
    width: 100%; 
    padding: 10px; 
    border: 1px solid #ccc; 
    border-radius: 5px; 
} 

.password-group .toggle-password 
{ 
    position: absolute; 
    top: 55px;
    right: 10px; 
    cursor: pointer; 
} 

.toggle-password .fa 
{ 
    color: #555; 
}

.btn-primary 
{ 
    background-color: #007bff; 
    color: #fff; 
    padding: 10px 20px; 
    border: none; 
    border-radius: 5px; 
    cursor: pointer; 
    font-size: 25px; 
} 

.btn-primary:hover 
{ 
    background-color: #0056b3; 
} 

.text-danger 
{ 
    color: #dc3545; 
    font-size: 14px; 
}
</style>