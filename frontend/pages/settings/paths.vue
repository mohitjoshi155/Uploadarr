<template>
	<div>
		<span>{{ moduleName }}</span>
		<directory-browser @new-path="setMoviePath($event)" />
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import DirectoryBrowser from '@/components/DirectoryBrowser.vue';

@Component({
	components: {
		DirectoryBrowser,
	},
})
export default class SettingsPaths extends Vue {
	moduleName: string = 'Settings Paths';

	setMoviePath(path: string): void {
		console.log(path);

		// alert(path);
	}
}
</script>
