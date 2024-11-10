<template> 
    <div class="register-container"> 
        <div class="register-form"> 
            <h2>Register</h2> 
            <form @submit.prevent="register"> 
                <div class="form-group"> 
                    <label for="firstName">First Name:</label> 
                    <input type="text" id="firstName" v-model="firstName" required /> 
                </div> 
                <div class="form-group"> 
                    <label for="lastName">Last Name:</label> 
                    <input type="text" id="lastName" v-model="lastName" required /> 
                </div> 
                <div class="form-group"> 
                    <label for="email">Email:</label> 
                    <input type="email" id="email" v-model="email" required /> 
                </div> 
                <div class="form-group"> 
                    <label for="password">Password:</label> 
                    <input type="password" id="password" v-model="password" required /> 
                </div> 
                <div class="form-group"> 
                    <label for="confirmPassword">Confirm Password:</label> 
                    <input type="password" id="confirmPassword" v-model="confirmPassword" required /> 
                </div> 
                <button type="submit" class="btn btn-primary">Register</button> 
            </form> 
            <div v-if="message" class="alert alert-info">{{ message }}</div> 
            </div> 
        </div> 
    </template>

<script> 
    import axios from 'axios'; 
    import { ref } from 'vue'; 
    import { useRouter } from 'vue-router'; 
    
    export default 
    { 
        name: 'UserRegister', 
        setup() 
        { 
            const firstName = ref(''); 
            const lastName = ref(''); 
            const email = ref(''); 
            const password = ref(''); 
            const confirmPassword = ref(''); 
            const message = ref(''); 
            const router = useRouter(); 
            const register = async () => 
            { 
                try 
                { 
                    const response = await axios.post('https://localhost:7084/api/auth/register', 
                    { 
                        firstName: firstName.value, 
                        lastName: lastName.value, 
                        email: email.value, 
                        password: password.value, 
                        confirmPassword: confirmPassword.value 
                    }); 
                    message.value = response.data.Message; 
                    
                    // Optionally redirect to the login page after successful registration 
                    setTimeout(() => 
                    { 
                        router.push('/login'); 
                    }, 3000); 
                    
                    // Redirect after 3 seconds 
                } 
                catch (error) 
                { 
                    console.error('Error registering:', error.response.data); 
                } 
            }; 
            return { 
                firstName, 
                lastName, 
                email, 
                password, 
                confirmPassword, 
                message, 
                register 
            }; 
        } 
    }; 
</script>

<style scoped> 
.register-container 
{ 
    display: flex; 
    justify-content: center; 
    align-items: center; 
    height: 90vh; 
    background-color: #f8f9fa; 
} 

.register-form 
{ 
    background: #fff; 
    padding: 30px; 
    border-radius: 10px; 
    box-shadow: 0 0 20px rgba(0, 0, 0, 0.2); 
    width: 500px; 
    height: 700px; 
    text-align: center; 
} 

.register-form h2 
{ 
    margin-bottom: 20px; 
    font-size: 50px; 
    color: #333; 
} 

.form-group 
{ 
    margin-bottom: 15px; 
    text-align: left; 
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

.alert 
{ 
    margin-top: 20px; 
    padding: 10px; 
    background-color: #e9ecef; 
    border-radius: 5px; 
} 
</style>