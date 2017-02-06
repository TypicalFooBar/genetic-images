import Vue from 'vue'
import VueResource from 'vue-resource'
import Nav from '../vue/nav.vue'
import Home from '../vue/home.vue'

// Vue modules
Vue.use(VueResource);

// Declare the components
Vue.component('nav-component', Nav);
Vue.component('home-component', Home);

// Root instance
new Vue({
	el: '#app'
})