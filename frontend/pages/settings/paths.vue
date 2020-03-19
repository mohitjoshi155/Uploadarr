<template>
	<v-row>
		<v-col>
			<v-row>
				<v-col>
					<h2>Movie import Paths</h2>
					<hr />
				</v-col>
			</v-row>
			<folder-table :items="moviePaths" />
			<directory-browser @new-path="addPath($event, pathTypes[0])" />
			<v-row>
				<v-col>
					<h2>Series import paths</h2>
					<hr />
				</v-col>
			</v-row>
			<folder-table :items="seriePaths" />
			<directory-browser @new-path="addPath($event, pathTypes[1])" />
		</v-col>
	</v-row>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import DirectoryBrowser from '@/components/DirectoryBrowser.vue';
import FolderTable from '@/components/FolderTable.vue';
import IPath from '@/types/interfaces/iPath.ts';
import IPathType from '@/types/interfaces/iPathType.ts';

@Component({
	components: {
		DirectoryBrowser,
		FolderTable,
	},
})
export default class SettingsPaths extends Vue {
	moduleName: string = 'Settings Paths';

	pathTypes: IPathType[] = [
		{ id: 1, type: 'Movies' },
		{ id: 2, type: 'Series' },
	];

	moviePaths: IPath[] = [];
	seriePaths: IPath[] = [];

	setPath(paths: IPath[], type: IPathType): void {
		switch (type.id) {
			case 1:
				this.moviePaths = paths;
				break;

			case 2:
				this.seriePaths = paths;
				break;
		}
	}

	addPath(path: string, type: IPathType): void {
		let tempPaths: IPath[] = [];
		switch (type.id) {
			case 1:
				tempPaths = this.moviePaths;
				break;

			case 2:
				tempPaths = this.seriePaths;
				break;
		}

		tempPaths.push({ path, type });

		this.$axios
			.post('/rootfolder/', {
				path,
				type,
			})
			.then((response) => {
				this.setPath(tempPaths, type);
				this.requestPaths(response.data.type);
			});
	}

	requestPaths(type: IPathType): void {
		this.$axios.get(`/rootfolder/?typeId=${type.id}`).then((response) => {
			const paths: IPath[] = response.data ? response.data : [];

			this.setPath(paths, type);
		});
	}

	mounted(): void {
		this.requestPaths(this.pathTypes[0]);
		this.requestPaths(this.pathTypes[1]);
	}
}
</script>
