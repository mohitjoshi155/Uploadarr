<template>
	<v-row>
		<v-col cols="12">
			<v-data-table :headers="headers" :items="items" hide-default-footer fixed-header disable-sort disable-pagination disable-filtering>
				<template v-slot:item.type="{ item }">
					<v-icon>{{ getIcon(item.type) }}</v-icon>
				</template>
			</v-data-table>
		</v-col>
	</v-row>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';
import { DataTableHeader } from 'vuetify';
import IPath from '@/types/interfaces/iPath.ts';

@Component
export default class FolderTable extends Vue {
	@Prop({ required: true, type: Array as () => IPath[], default: [] })
	readonly items!: IPath[];

	headers: DataTableHeader[] = [
		{
			text: 'Path',
			value: 'path',
			width: 60,
		},
		{
			text: 'Free Space',
			value: 'freespace',
		},
		{
			text: 'Unmapped Folders',
			value: 'unmappedfolders',
		},
	];
}
</script>
