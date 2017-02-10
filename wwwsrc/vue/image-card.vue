<style>
	img {
		width: 100%;
	}
</style>

<template>
	<article class="card">
		<img v-bind:src="imgSrc">
		<footer>{{message}}</footer>
	</article>
</template>

<script>
	export default {
		name: 'image-card',
		props: {
			src: {
				type: String,
				required: true
			},
			message: {
				type: String,
				default: null
			},
			refresh: {
				type: Boolean,
				default: false
			}
		},
		created: function() {
			// Set the image
			this.imgSrc = this.src;

			// Start the refresh loop
			this.refreshLoop();

			var self = this;
		},
		data() {
			return {
				imgSrc: null,
				stopRefresh: false,
				refreshInterval: 1000
			}
		},
		methods: {
			refreshLoop: function() {
				var self = this;

				setTimeout(function() {
					if (self.refresh) {
						// Download the image again by adding a timestamp to the end
						self.imgSrc = self.src + "?" + new Date().getTime();
					}

					// Refresh again after the specified interval
					self.refreshLoop();
				}, this.refreshInterval);
			}
		}
	}
</script>