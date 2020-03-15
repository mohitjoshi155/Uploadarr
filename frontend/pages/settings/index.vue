<template>
	<div>
		<span>{{ moduleName }}</span>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class SettingsIndex extends Vue {
	moduleName: string = 'settings';
}
</script>
