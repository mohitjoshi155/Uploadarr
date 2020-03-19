<template>
	<v-app dark>
		<v-content>
			<v-container fluid class="p-0">
				<v-row>
					<v-col cols="2">
						<the-menu />
					</v-col>
					<v-col>
						<nuxt />
					</v-col>
				</v-row>
			</v-container>
		</v-content>
		<v-footer app>
			<span>&copy; {{ new Date().getFullYear() }}</span>
		</v-footer>
	</v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import TheMenu from '@/components/TheMenu/TheMenu.vue';

@Component({
	components: { TheMenu },
})
export default class Default extends Vue {
	get currentYear(): number {
		return new Date().getFullYear();
	}
}
</script>
