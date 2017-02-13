<style>
	button {
		width: 100%;
	}
	.status {
		text-align: right;
	}
</style>

<template>
	<div class="off-half status">
		<h3>Engine Status</h3>
		<h4>Is Running</h4>
		{{isRunning}}
		<h4>Results Available</h4>
		{{resultsAvailable}}
		<h4>Current Generation</h4>
		{{currentGeneration}}
		<h4>Message</h4>
		{{message}}
		<button v-if="isRunning" class="error" v-on:click="cancelClicked">Cancel</button>
	</div>
</template>

<script>
	export default {
		name: 'engine-status',
		props: {
			isRunning: {
				type: Boolean,
				required: true
			},
			resultsAvailable: {
				type: Boolean,
				required: true
			},
			currentGeneration: {
				type: Number,
				required: true
			},
			message: {
				type: String,
				required: true
			}
		},
		data() {
			return {
				
			}
		},
		methods: {
			cancelClicked: function() {
				this.$emit('cancel');
			}
		}
	}
</script>