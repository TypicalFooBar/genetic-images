var app = new Vue({
    el: "#app",
    data: {
        appInitialized: false,
        engineIsRunning: false,
        resultsAvailable: false,
        engineStatusMessage: "",
        targetImgSrc: null
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
                
                this.targetImgSrc = "/GeneticImages/TargetImage?" + new Date().getTime();
                this.bestGeneImgSrc = "/GeneticImages/BestImageFromGeneration/" + response.body.currentGeneration + "?" + new Date().getTime();

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
});

app.init();