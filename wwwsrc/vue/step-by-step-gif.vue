<style>
	img {
		width: 100%;
	}
</style>

<template>
	<div>
		<article class="card">
			<img v-bind:src="imgSrc">
			<footer>{{message}}</footer>
		</article>
	</div>
</template>

<script>
	var gifshot = require('gifshot');

	export default {
		name: 'step-by-step-gif',
		props: {
			numberOfSteps: {
				type: Number,
				required: true
			},
			width: {
				type: Number,
				required: true
			},
			height: {
				type: Number,
				required: true
			}
		},
		created: function() {
			// Get all image URLs for gifshot
			var imageUrlArray = [];
			for (var i = 1; i <= this.numberOfSteps; i++) {
				imageUrlArray.push('/GeneticImages/Step/' + i);
			}

			// Add the last gif a few times to make it longer at the end
			for (var i = 0; i < 50; i++) {
				imageUrlArray.push('/GeneticImages/Step/' + this.numberOfSteps);
			}

			// Create the gif
			var self = this;
			gifshot.createGIF({
				images: imageUrlArray,
				sampleInterval: 1,
				numWorkers: 8,
				gifWidth: this.width,
				gifHeight: this.height,
				progressCallback: function(captureProgress) {
					self.message = "Creating GIF... " + Math.round(captureProgress * 100) + "%";
				}
			},function(obj) {
				if(!obj.error) {
					// Set the image source
					var image = obj.image;
					self.imgSrc = image;
					self.message = "Step-by-step GIF"
				}
				else {
					self.message = "Error creating GIF"
				}
			});
		},
		data() {
			return {
				imgSrc: null,
				message: "Creating GIF... 0%"
			}
		},
		methods: {
		}
	}
</script>