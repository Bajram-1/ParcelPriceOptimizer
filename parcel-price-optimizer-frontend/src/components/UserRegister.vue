<template> 
    <div class="register-container"> 
        <div class="register-form"> 
            <h2>Register</h2> 
            <form @submit.prevent="register"> 
                <div class="form-group"> 
                    <label for="firstName">First Name:</label> 
                    <input type="text" id="firstName" v-model="firstName" required /> 
                    <span v-if="errors.FirstName" class="text-danger">{{ errors.FirstName }}</span> 
                </div> 
                <div class="form-group"> 
                    <label for="lastName">Last Name:</label> 
                    <input type="text" id="lastName" v-model="lastName" required /> 
                    <span v-if="errors.LastName" class="text-danger">{{ errors.LastName }}</span> 
                </div> 
                <div class="form-group"> 
                    <label for="email">Email:</label> 
                    <input type="email" id="email" v-model="email" required /> 
                    <span v-if="errors.Email" class="text-danger">{{ errors.Email }}</span> 
                </div> 
                <div class="form-group password-group" @mouseover="showPasswordTooltip = true" @mouseleave="showPasswordTooltip = false"> 
                    <label for="password">Password:</label> 
                    <input :type="showPassword ? 'text' : 'password'" id="password" v-model="password" required /> 
                    <span class="toggle-password" @click="togglePasswordVisibility"> 
                        <i :class="showPassword ? 'fas fa-eye' : 'fas fa-eye-slash'"></i> 
                    </span> 
                    <span v-if="errors.Password" class="text-danger">{{ errors.Password }}</span> 
                    <span class="password-tooltip" v-if="showPasswordTooltip"> 
                        <ul> 
                            <li>At least 8 characters</li> 
                            <li>At least one digit</li> 
                            <li>At least one uppercase letter</li> 
                            <li>At least one lowercase letter</li> 
                            <li>At least one non-alphanumeric character</li> 
                        </ul> 
                    </span>
                </div>
                <div class="form-group password-group"> 
                    <label for="confirmPassword">Confirm Password:</label> 
                    <input :type="showConfirmPassword ? 'text' : 'password'" id="confirmPassword" v-model="confirmPassword" required /> 
                    <span class="toggle-password" @click="toggleConfirmPasswordVisibility"> 
                        <i :class="showConfirmPassword ? 'fas fa-eye' : 'fas fa-eye-slash'"></i> 
                    </span> 
                    <span v-if="errors.ConfirmPassword" class="text-danger">{{ errors.ConfirmPassword }}</span> 
                </div> 
                <button type="submit" class="btn btn-primary"> 
                    <i class="fas fa-user-plus"></i> Register</button> 
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
            const showPassword = ref(false);
            const showConfirmPassword = ref(false); 
            const showPasswordTooltip = ref(false);
            const message = ref(''); 
            const errors = ref({});
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
                    setTimeout(() => 
                    { 
                        router.push('/login'); 
                    }, 3000); 
                } 
                catch (error) 
                { 
                    if (error.response && error.response.data) 
                    { 
                        errors.value = error.response.data.errors || {}; 
                    } 
                    else 
                    { 
                        console.error('Error registering:', error); 
                    } 
                } 
            }; 
            
            const togglePasswordVisibility = () => 
            { 
                showPassword.value = !showPassword.value; 
            }; 
            
            const toggleConfirmPasswordVisibility = () => 
            { 
                showConfirmPassword.value = !showConfirmPassword.value; 
            }; 
            
            return { 
                firstName, 
                lastName, 
                email, 
                password, 
                confirmPassword, 
                showPassword, 
                showConfirmPassword, 
                showPasswordTooltip,
                message, 
                errors,
                register, 
                togglePasswordVisibility, 
                toggleConfirmPasswordVisibility 
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

.password-group 
{
    position: relative; 
} 

.password-tooltip 
{ 
    visibility: visible; 
    width: 250px; 
    background-color: #f9f9f9; 
    color: #555; 
    text-align: left; 
    border-radius: 5px; 
    padding: 10px; 
    position: absolute; 
    z-index: 1; 
    top: 35px;
    left: 110%; 
    right: 0; 
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); 
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

.tooltip 
{ 
    position: relative; 
    display: inline-block; 
    cursor: pointer; 
    margin-left: 10px; 
} 

.tooltip .tooltiptext 
{ 
    visibility: hidden; 
    width: 250px; 
    background-color: #f9f9f9; 
    color: #555; 
    text-align: left; 
    border-radius: 5px; 
    padding: 10px; 
    position: absolute; 
    z-index: 1; 
    top: 0; 
    left: 110%; 
    transform: translateX(-50%); 
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); 
} 

.tooltip:hover .tooltiptext 
{ 
    visibility: visible; 
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

.text-danger 
{ 
    color: #dc3545; 
    font-size: 14px; 
}
</style>