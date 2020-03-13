<template>
	<v-app>
		<v-content>
			<v-container fluid>
				<nuxt />
			</v-container>
		</v-content>
	</v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class Default extends Vue {
	get currentYear(): number {
		return new Date().getFullYear();
	}
}
</script>
