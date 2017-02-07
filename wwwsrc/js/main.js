// import Vue from 'vue'
// import VueResource from 'vue-resource'
// import Nav from '../vue/nav.vue'
// import Home from '../vue/home.vue'

var Vue = require('../../node_modules/vue/dist/vue.js');
var VueResource = require('vue-resource');
var Nav = require('../vue/nav.vue');
var Home = require('../vue/home.vue');
var ImageCard = require('../vue/image-card.vue');

// Vue modules
Vue.use(VueResource);

// Declare the components
Vue.component('nav-component', Nav);
Vue.component('home-component', Home);
Vue.component('image-card-component', ImageCard);

// Root instance
new Vue({
	el: '#app'
})