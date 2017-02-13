<style>
	img {
		width: 100%;
	}
</style>

<template>
	<article class="card">
		<img v-bind:src="imgSrc">
		<footer>Step: {{currentStep}}</footer>
	</article>
</template>

<script>
	export default {
		name: 'image-step-by-step',
		props: {
			numberOfSteps: {
				type: Number,
				required: true
			},
			refresh: {
				type: Boolean,
				default: false,
				required: true
			}
		},
		created: function() {
			// Set the image
			this.imgSrc = "/GeneticImages/Step/" + this.currentStep + "?" + new Date().getTime();

			// Start the refresh loop
			this.refreshLoop(this.refreshInterval);
		},
		data() {
			return {
				imgSrc: null,
				currentStep: 1,
				refreshInterval: 100
			}
		},
		methods: {
			refreshLoop: function(refreshInterval) {
				var self = this;

				setTimeout(function() {
					if (self.refresh) {
						// Increment the current Step
						if (self.currentStep == self.numberOfSteps) {
							self.currentStep = 1;
						}
						else {
							self.currentStep++;
						}

						// Download the next image
						self.imgSrc = "/GeneticImages/Step/" + self.currentStep + "?" + new Date().getTime();

						// If we're on the last step
						if (self.currentStep == self.numberOfSteps) {
							// Pause for a bit longer on the final image before restarting
							self.refreshLoop(refreshInterval + 2000);
						}
						else {
							// Refresh again after the specified interval
							self.refreshLoop(self.refreshInterval);
						}
					}
					else {
						// Refresh again after the specified interval
						self.refreshLoop(self.refreshInterval);
					}
				}, refreshInterval);
			}
		}
	}
</script>