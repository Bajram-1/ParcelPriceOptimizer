<template>
    <div class="container text-center mt-5 p-4 shadow-sm rounded bg-light">
        <h1 class="text-success mb-3"><i class="fas fa-check-circle me-2"></i>Payment Successful</h1>
        <p class="lead">{{ confirmationMessage }}</p>
        <i class="fas fa-smile-beam fa-4x text-warning my-4"></i>
        <div class="mt-4">
            <router-link to="/" class="btn btn-outline-success btn-lg">
                <i class="fas fa-home me-2"></i> Return to Home
            </router-link>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { useToast } from 'vue-toastification';

export default {
  name: 'PaymentSuccess',
  data() {
    return {
      confirmationMessage: 'Thank you for your payment! Your transaction was completed successfully.',
    };
  },
  created() {
    const toast = useToast();

    axios.get('https://localhost:7084/api/payment/payment-success')
      .then(response => {
        console.log('Payment success response:', response); 
        if (response.data && response.data.message) {
          this.confirmationMessage = response.data.message;
          toast.success(this.confirmationMessage);
        } else {
          this.confirmationMessage = 'Payment was successful, but no confirmation message was received.';
          toast.success(this.confirmationMessage);
        }
      })
      .catch(error => 
      { 
        console.log('Error fetching payment success message:', error); 
        this.confirmationMessage = 'Payment was successful.'; 
        toast.success('Payment was successful.'); 
      });
    } 
  };
</script>

<style scoped>
    .container {
        max-width: 500px;
        background-color: #e9f7ef;
        border-radius: 10px;
        border: 1px solid #c3e6cb;
    }
    h1 {
        font-size: 2rem;
    }
    p {
        font-size: 1.1rem;
        color: #555;
    }
</style>