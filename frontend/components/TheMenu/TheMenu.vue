<template>
	<v-card max-width="500" class="mx-auto">
		<v-list>
			<template v-for="item in getLinks">
				<template v-if="item.items.length > 0">
					<v-list-group :key="item.title" v-model="item.active" :prepend-icon="item.icon" no-action @click="navigateTo(item.path)">
						<template v-slot:activator>
							<v-list-item-content>
								<v-list-item-title v-text="item.title" />
							</v-list-item-content>
						</template>

						<v-list-item v-for="subItem in item.items" :prepend-icon="item.icon" :key="subItem.title" @click="navigateTo(subItem.path)">
							<v-list-item-content>
								<v-list-item-title v-text="subItem.title" />
							</v-list-item-content>
						</v-list-item>
					</v-list-group>
				</template>
				<template v-else>
					<v-list-item :key="item.title" @click="navigateTo(item.path)">
						<v-icon class="mr-8">{{ item.icon }}</v-icon>
						<v-list-item-content>
							<v-list-item-title v-text="item.title" />
						</v-list-item-content>
					</v-list-item>
				</template>
			</template>
		</v-list>
	</v-card>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

interface INavModule {
	title: string;
	path: string;
	icon?: string;
	active?: boolean;
	items: INavModule[];
}

@Component
export default class TheMenu extends Vue {
	get getLinks(): INavModule[] {
		return [
			{
				icon: 'mdi-filmstrip',
				title: 'Movies',
				path: '/movies',
				active: true,
				items: [{ title: 'Import', path: '/movies/import', items: [] }],
			},
			{
				icon: 'mdi-television',
				title: 'Series',
				path: '/series',
				items: [{ title: 'Import', path: '/series/import', items: [] }],
			},
			{
				icon: 'mdi-clock-outline',
				title: 'Activity',
				path: '/activity',
				items: [],
			},
			{
				icon: 'directions_run',
				title: 'Settings',
				path: '/settings',
				items: [],
			},
		];
	}

	navigateTo(path: string): void {
		this.$router.push({
			path,
		});
	}
}
</script>

<style></style>
