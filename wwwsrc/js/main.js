var Vue = require('../../node_modules/vue/dist/vue.js');
var VueResource = require('vue-resource');
var Nav = require('../vue/nav.vue');
var Home = require('../vue/home.vue');
var ImageCard = require('../vue/image-card.vue');
var RunConfigEditor = require('../vue/run-config-editor.vue');
var EngineStatus = require('../vue/engine-status.vue');
var StepByStepGif = require('../vue/step-by-step-gif.vue');

// Vue modules
Vue.use(VueResource);

// Declare the components
Vue.component('nav-component', Nav);
Vue.component('home', Home);
Vue.component('image-card', ImageCard);
Vue.component('run-config-editor', RunConfigEditor);
Vue.component('engine-status', EngineStatus);
Vue.component('step-by-step-gif', StepByStepGif);

// Root instance
new Vue({
	el: '#app'
})