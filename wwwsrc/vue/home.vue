<style>
</style>

<template>
	<div v-if="appInitialized" v-cloak>
		<div class="flex">
			<div>
				<run-config-editor v-on:run="run"></run-config-editor>
			</div>
			<div class="text-center">
				<div v-if="engineStatus.isRunning || engineStatus.resultsAvailable">
					<h3>Results</h3>
					<div class="flex two">
						<div>
							<image-card
								src="/GeneticImages/TargetImage"
								message="Target Image"
								:refresh="refreshImages"/>
						</div>
						<div>
							<image-card
								src="/GeneticImages/LatestImage"
								message="Best Image"
								:refresh="engineStatus.isRunning || refreshImages"/>
						</div>
					</div>
					<div v-if="!engineStatus.resultsAvailable">
						<i class="fa fa-cog fa-spin fa-5x fa-fw"></i>
					</div>
					<div v-if="resultsAvailable">
					</div>
				</div>
			</div>
			<div>
				<engine-status
					:isRunning="engineStatus.isRunning"
					:resultsAvailable="engineStatus.resultsAvailable"
					:currentGeneration="engineStatus.currentGeneration"
					:message="engineStatus.message"
					v-on:cancel="cancel">
				</engine-status>
			</div>
		</div>

		<div class="modal">
			<input ref="modalAlert" id="modalAlert" type="checkbox" />
			<label for="modalAlert" class="overlay"></label>
			<article>
				<header>
					<h3>{{modalAlert.title}}</h3>
					<label for="modalAlert" class="close">&times;</label>
				</header>
				<section class="content">
					{{modalAlert.message}}
				</section>
				<footer>
					<label for="modalAlert" class="button">
						Close
					</label>
				</footer>
			</article>
		</div>

	</div>
</template>

<script>
	export default {
		name: 'home',
		created: function() {
			this.getEngineStatus().then(response => {
				this.appInitialized = true;

				if (this.engineStatus.isRunning) {
					// Start the status loop
					this.getEngineStatusTimeout();
				}
			});
		},
		data() {
			return {
				appInitialized: false,
				engineStatus: null,
				refreshImages: false,
				modalAlert: {
					title: null,
					message: null
				}
			}
		},
		methods: {
			getEngineStatusTimeout: function() {
				var self = this;
				setTimeout(function() {
					// Get the engine status
					self.getEngineStatus().then(response => {
						// If the engine is running
						if (self.engineStatus.isRunning) {
							// Do the timeout again until the engine is not running
							self.getEngineStatusTimeout();
						}
						else {
							// The engine has stopped

							// Refresh the images to make sure we get the final result
							self.imageRefresh();
						}
					});
				}, 1000);
			},
			getEngineStatus: function() {
				return this.$http.get("/GeneticImages/EngineStatus").then(response => {
					this.engineStatus = response.body;

					return Promise.resolve();
				})
			},
			imageRefresh: function() {
				this.refreshImages = true;
				var self = this;

				// Turn the refresh flag off after 2 seconds.
				// This is more than enough time to allow the image cards to refresh.
				setTimeout(function() {
					self.refreshImages = false;
				}, 2000);
			},
			run: function(runConfig) {
				// Create FormData for this run request
				var data = new FormData();
				data.append('file', runConfig.file);
				for (var key in runConfig) {
					data.append(key, runConfig[key]);
				}

				// Start the engine
				this.$http.post("/GeneticImages/Run", data).then(response => {
					this.engineStatus = response.body;

					// Refresh the images to make sure we get the new target image
					this.imageRefresh();

					this.getEngineStatusTimeout();
				}, response => {
					// Show the modal alert
					this.modalAlert.title = "Could not start instance";
					this.modalAlert.message = response.body.message;
					this.$refs.modalAlert.checked = true;
				});
			},
			cancel: function() {
				this.$http.get("/GeneticImages/Cancel").then(response => {
					this.engineStatus = response.body;
				}, response => {
					// Show the modal alert
					this.modalAlert.title = "Error canceling engine";
					this.modalAlert.message = "hmm...not sure what happened...";
					this.$refs.modalAlert.checked = true;
				});
			}
		}
	}
</script>