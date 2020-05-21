<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-card :class="$style.users">
						<template v-slot:header>
							<h4 class="float-left">Users</h4>
							<b-button variant="success" class="float-right" @click="edit">
								Add new user
							</b-button>
						</template>
						<b-table :items="users" :fields="fields">
							<template v-slot:cell(registered)="data">
								{{ localDate(data.item.registered) }}
							</template>
							<template v-slot:cell(actions)="data">
								<b-button variant="primary" @click="edit(data.item.id)"
									><b-icon icon="pencil"
								/></b-button>
								<b-button variant="danger" @click="remove(data.item.id)"
									><b-icon icon="trash"
								/></b-button>
							</template>
						</b-table>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
		<b-modal id="edit" title="Edit user">
			<b-form>
				<b-form-group label="Name">
					<b-form-input v-model="user.name" type="text" required></b-form-input>
				</b-form-group>
				<b-form-group label="Surname">
					<b-form-input
						v-model="user.surname"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Email">
					<b-form-input
						v-model="user.email"
						type="email"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Role">
					<b-form-select
						v-model="user.role"
						:options="roles"
						required
					></b-form-select>
				</b-form-group>
			</b-form>
		</b-modal>
		<b-modal id="remove" title="Remove user">
			Do you want to remove this user: {{ userToRemove }}?
		</b-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { IUser } from '@/lib/Api';

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

	roles: string[] = ['Guest', 'Moderator', 'Admin'];
	user: IUser = this.emptyUser;
	userToRemove: string = '';

	async mounted() {
		await this.$store.dispatch('users/pullUsers');
	}

	get users(): IUser[] {
		return this.$store.state.users.users || [];
	}

	get emptyUser() {
		return (this.user = {
			name: '',
			surname: '',
			email: '',
			role: '',
		});
	}

	localDate(date: Date) {
		return date.toLocaleString('pl');
	}

	edit(id: number) {
		this.user = this.users.find(u => u.id === id) || this.emptyUser;
		this.$bvModal.show('edit');
	}

	remove(id: number) {
		const user = this.users.find(u => u.id === id);
		this.userToRemove = `${user.name} ${user.surname}`;
		this.$bvModal.show('remove');
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
