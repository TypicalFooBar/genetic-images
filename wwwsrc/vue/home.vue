<style>
</style>

<template>
	<div v-if="appInitialized" v-cloak>
		<div class="flex">
			<div></div>
			<div class="text-center">
				<div v-if="engineIsRunning || resultsAvailable">
					<p>{{engineStatusMessage}}</p>
					<div class="flex two">
						<div>
							<image-card-component
								src="/GeneticImages/TargetImage"
								message="Target Image"
								:refresh="refreshImages"/>
						</div>
						<div>
							<image-card-component
								src="/GeneticImages/LatestImage"
								message="Latest Computed Image"
								:refresh="engineIsRunning || refreshImages"/>
						</div>
					</div>
					<div v-if="!resultsAvailable">
						<i class="fa fa-cog fa-spin fa-5x fa-fw"></i>
					</div>
					<div v-if="resultsAvailable">

					</div>
				</div>
				<div v-if="!engineIsRunning">
					<h1>Upload Image</h1>
					<div>
						<label class="dropimage">
							<input ref="uploadFileInput" v-on:change="uploadFile" title="Drop image or click me" type="file">
						</label>
					</div>
				</div>
			</div>
			<div></div>
		</div>
	</div>
</template>

<script>
	export default {
		name: 'home',
		created: function() {
			this.getEngineStatus().then(response => {
				this.appInitialized = true;

				if (this.engineIsRunning) {
					// Start the status loop
					this.getEngineStatusTimeout();
				}
			});
		},
		data() {
			return {
				appInitialized: false,
				engineIsRunning: false,
				resultsAvailable: false,
				engineStatusMessage: "",
				refreshImages: false,
				runRequestConfig: {
					//file: null,
					generations: 100,
					genesPerGeneration: 100,
					genesToReproduce: 10
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
						if (self.engineIsRunning) {
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
					this.engineIsRunning = response.body.isRunning;
					this.resultsAvailable = response.body.resultsAvailable;
					this.engineStatusMessage = response.body.message;

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
			uploadFile: function() {
				// Get only one file (the first one)
				var file = this.$refs.uploadFileInput.files[0];
				//this.runConfig.file = file;

				// Set the status message
				this.engineStatusMessage = "Processing " + file.name;
				this.fileProcessingOrComplete = true;

				// Create FormData for this run request
				var data = new FormData();
				data.append('file', file);
				for (var key in this.runRequestConfig) {
					data.append(key, this.runRequestConfig[key])
				}

				// Post the file and run config to the server to start the engine
				this.$http.post("/GeneticImages/RunDrawLines", data).then(response => {
					this.engineIsRunning = response.body.isRunning;
					this.resultsAvailable = response.body.resultsAvailable;
					this.engineStatusMessage = response.body.message;

					// Refresh the images to make sure we get the new target image
					this.imageRefresh();

					this.getEngineStatusTimeout();
				}, response => {
					this.engineStatusMessage = "Something went wrong...";
				});
			},
			runDrawLines: function() {

			}
		}
	}
</script>