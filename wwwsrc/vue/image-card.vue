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
			refreshInterval: {
				type: Number,
				default: 0
			}
		},
		created: function() {
			// If a refresh interval is specified
			if (this.refreshInterval > 0) {
				// Set the image and start the refresh loop
				this.imgSrc = this.src;
				this.refreshImage(this.refreshInterval);
			}
			// Otherwise just set the src one time
			else {
				this.imgSrc = this.src;
			}
		},
		data() {
			return {
				imgSrc: null
			}
		},
		methods: {
			refreshImage: function() {
				var self = this;
				setTimeout(function() {
					// Download the image again by adding a timestamp to the end
					self.imgSrc = self.src + "?" + new Date().getTime();

					// Refresh again after the specified interval
					self.refreshImage();
				}, this.refreshInterval);
			}
		}
	}
</script>