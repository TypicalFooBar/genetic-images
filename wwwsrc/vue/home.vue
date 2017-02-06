<style>
	h1 {
		color: green;
	}
</style>

<template>
	<h1>Hey...{{message}}</h1>
</template>

<script>
	export default {
		name: 'home',
		data() {
			return {
				message: 'Hello, component!'
			}
		}
	}
</script>