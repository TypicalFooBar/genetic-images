<style>
	button {
		width: 100%;
	}
</style>

<template>
	<div class="half">
		<h3>Configuration</h3>
		<h4>Image</h4>
		<div>
			<label ref="fileInputLabel" class="dropimage">
				<input ref="fileInput" v-on:change="fileSelected" title="Drop image or click me" type="file">
			</label>
		</div>
		<h4>Generations</h4>
		<input type="number" v-model="runConfig.generations">
		<h4>Genes per Generation</h4>
		<input type="number" v-model="runConfig.genesPerGeneration">
		<h4>Reproduction Pool</h4>
		<input type="number" v-model="runConfig.genesToReproduce">
		<h4>Gene Type</h4>
		<select v-model="runConfig.geneType">
			<option v-for="geneType in geneTypes" v-bind:value="geneType.id">{{geneType.name}}</option>
		</select>
		<hr>
		<h3>Gene Settings</h3>
		<h4>Mutation Range Max (0-X)</h4>
		<input type="number" v-model="runConfig.mutationRangeMax">
		<div v-if="runConfig.geneType == 2">
			<h4># of Strokes</h4>
			<input type="number" v-model="runConfig.numberOfStrokes">
		</div>
		<button v-on:click="runClicked">Run</button>
	</div>
</template>

<script>
	export default {
		name: 'run-config-editor',
		created: function() {
			
		},
		data() {
			return {
				geneTypes: [
					{ id: 1, name: 'Pixel Gene'},
					{ id: 2, name: 'Line Gene'}
				],
				runConfig: {
					file: null,
					generations: 100,
					genesPerGeneration: 100,
					genesToReproduce: 10,
					geneType: 2,
					mutationRangeMax: 1000,
					numberOfStrokes: 250
				}
			}
		},
		methods: {
			fileSelected: function() {
				// Get only one file (the first one)
				this.runConfig.file = this.$refs.fileInput.files[0];

				// Show this file as the background
				var fileInputLabel = this.$refs.fileInputLabel;
				var reader = new FileReader();
				reader.onloadend = function() {
					fileInputLabel.style['background-image'] = 'url('+ reader.result + ')';
				};
				reader.readAsDataURL(this.runConfig.file);
			},
			runClicked: function() {
				window.scrollTo(0, 0);
				this.$emit('run', this.runConfig);
			}
		}
	}
</script>