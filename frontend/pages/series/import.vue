<template>
	<div>
		<span>{{ moduleName }}</span>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class MoviesIndex extends Vue {
	moduleName: string = 'series/import';
}
</script>
