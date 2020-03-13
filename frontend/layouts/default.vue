<template>
	<v-app dark>
		<v-content>
			<v-container>
				<nuxt />
			</v-container>
		</v-content>
		<v-footer app>
			<span>&copy; {{ new Date().getFullYear() }}</span>
		</v-footer>
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
