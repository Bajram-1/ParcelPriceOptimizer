import { createStore } from 'vuex'; 
import axios from 'axios'; 

const store = createStore({ state: 
{ 
    token: localStorage.getItem('token') || null }, 
    mutations: 
    { 
        setToken(state, token) 
        { 
            state.token = token; 
        }, logout(state) { state.token = null; } 
    }, 
    actions: 
    { 
        login({ commit }, token) 
        { 
            commit('setToken', token); 
            localStorage.setItem('token', token); 
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`; 
        }, 
        logout({ commit }) 
        { 
            commit('logout'); 
            localStorage.removeItem('token'); 
            delete axios.defaults.headers.common['Authorization']; 
        } 
    } 
}); 

export default store;