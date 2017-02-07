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
								message="Target Image"/>
						</div>
						<div>
							<image-card-component
								src="/GeneticImages/LatestImage"
								message="Latest Computed Image"
								:refresh-interval="1000"/>
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
			this.init();
		},
		data() {
			return {
				appInitialized: false,
				engineIsRunning: false,
				resultsAvailable: false,
				engineStatusMessage: "",
				targetImgSrc: null
			}
		},
		methods: {
			init: function() {
				this.getEngineStatus().then(response => {
					this.appInitialized = true;

					if (this.engineIsRunning) {
						this.getEngineStatusTimeout();
					}
				});
			},
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
					});
				}, 1000);
			},
			getEngineStatus: function() {
				return this.$http.get("/GeneticImages/EngineStatus").then(response => {
					this.engineIsRunning = response.body.engineIsRunning;
					this.resultsAvailable = response.body.resultsAvailable;
					this.engineStatusMessage = response.body.message;
					
					//this.targetImgSrc = "/GeneticImages/TargetImage?" + new Date().getTime();
					//this.bestGeneImgSrc = "/GeneticImages/BestImageFromGeneration/" + response.body.currentGeneration + "?" + new Date().getTime();

					return Promise.resolve();
				})
			},
			uploadFile: function() {
				// Get only one file (the first one)
				var file = this.$refs.uploadFileInput.files[0];

				// Set the status message
				this.engineStatusMessage = "Processing " + file.name;
				this.fileProcessingOrComplete = true;

				// Create FormData to hold the file for uploadFile
				var data = new FormData();
				data.append('file', file);

				this.$http.post("/GeneticImages/RunStatic", data).then(response => {
					this.engineIsRunning = response.body.engineIsRunning;
					this.resultsAvailable = response.body.resultsAvailable;
					this.engineStatusMessage = response.body.message;

					this.getEngineStatusTimeout();
				}, response => {
					this.engineStatusMessage = "Something went wrong...";
				});
			}
		}
	}
</script>