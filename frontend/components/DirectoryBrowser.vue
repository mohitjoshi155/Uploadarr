<template>
	<v-row justify="center">
		<v-dialog v-model="isDialogOpen" scrollable max-width="900px">
			<!-- The path box -->
			<template v-slot:activator="{ on }">
				<v-btn color="primary" dark v-on="on">{{ buttonText }}</v-btn>
			</template>

			<v-card style="position: absolute; top: 200px; max-width: 900px;">
				<v-card-title>Select path</v-card-title>
				<v-card-text>
					<v-row>
						<v-col cols="12">
							<v-text-field v-model="path" @input="setPath($event)" placeholder="Start typing or select a path below" />
						</v-col>
						<v-col>
							<v-data-table
								:headers="headers"
								:items="items"
								hide-default-footer
								fixed-header
								disable-sort
								disable-pagination
								disable-filtering
								@click:row="handleClick"
							>
								<template v-slot:item.type="{ item }">
									<v-icon>{{ getIcon(item.type) }}</v-icon>
								</template>
							</v-data-table>
						</v-col>
					</v-row>
				</v-card-text>
				<v-card-actions class="justify-end">
					<v-btn color="blue darken-1" text @click="cancel()">Cancel</v-btn>
					<v-btn color="blue darken-1" text @click="ok()">Ok</v-btn>
				</v-card-actions>
			</v-card>
		</v-dialog>
	</v-row>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator';
import { DataTableHeader } from 'vuetify';

interface IDataRow {
	type: string;
	name: string;
	path: string;
	size: number;
}

@Component
export default class DirectoryBrowser extends Vue {
	@Prop({ required: false, type: String, default: 'Add Folder' })
	readonly buttonText!: string;

	isDialogOpen: boolean = false;

	parentPath: string = '';
	path: string = '';

	headers: DataTableHeader[] = [
		{
			text: 'Type',
			value: 'type',
			width: 60,
		},
		{
			text: 'Name',
			value: 'name',
		},
	];

	items: IDataRow[] = [];

	getIcon(type: string): string {
		switch (type) {
			case 'drive':
				return 'mdi-folder';
			case 'folder':
				return 'mdi-folder';
			case 'file':
				return 'mdi-file';
			case 'return':
				return 'mdi-arrow-up-bold-outline';

			default:
				return 'crosshairs-question';
		}
	}

	setPath(path: string): void {
		console.log(path);
	}

	ok(): void {
		this.$emit('new-path', this.path);
		this.cancel();
	}

	cancel(): void {
		this.path = '';
		this.isDialogOpen = false;
		this.requestDirectories(this.path);
	}

	handleClick(dataRow: IDataRow): void {
		if (dataRow.path === '..') {
			this.requestDirectories(this.parentPath);
		} else {
			this.requestDirectories(dataRow.path);
		}
	}

	requestDirectories(path: string): void {
		this.$axios.get(`/filesystem/?path=${path}`).then((response) => {
			console.log(response.data);
			this.items = response.data.directories;

			// Don't add return row if in the root folder
			if (path !== '') {
				this.items.unshift({
					type: 'return',
					name: '...',
					path: '..',
				} as IDataRow);
			}
			this.parentPath = response.data.parent;
			this.path = path;
		});
	}

	mounted(): void {
		this.requestDirectories('');
	}
}
</script>
