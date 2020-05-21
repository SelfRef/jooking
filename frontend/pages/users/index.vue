<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-card :class="$style.users">
						<template v-slot:header>
							<h4 class="float-left">Users</h4>
							<b-button variant="success" class="float-right">
								Add new user
							</b-button>
						</template>
						<b-table :items="users" :fields="fields">
							<template v-slot:cell(actions)>
								<b-button variant="primary"><b-icon icon="pencil"/></b-button>
								<b-button variant="danger"><b-icon icon="trash"/></b-button>
							</template>
						</b-table>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class Users extends Vue {
	fields = [
		{
			key: 'name',
			sortable: true,
		},
		{
			key: 'surname',
			sortable: true,
		},
		{
			key: 'email',
			sortable: true,
		},
		{
			key: 'registered',
			sortable: true,
		},
		{
			key: 'role',
			sortable: true,
		},
		{
			key: 'actions',
		},
	];

	async mounted() {
		await this.$store.dispatch('users/pullUsers');
	}

	get users() {
		return this.$store.state.users.users || [];
	}
}
</script>

<style lang="scss" module>
.inputGroup {
	margin: 20px 0;
}
.reservations {
	margin-top: 50px;
}
</style>
