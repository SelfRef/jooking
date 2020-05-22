<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-card :class="$style.users">
						<template v-slot:header>
							<h4 class="float-left">Users</h4>
							<b-button-group class="float-right">
								<b-button @click="refresh">
									Refresh
								</b-button>
								<b-button variant="success" @click="editModal">
									Add new user
								</b-button>
							</b-button-group>
						</template>
						<b-table :items="users" :fields="fields">
							<template v-slot:cell(registered)="data">
								{{ localDate(data.item.registered) }}
							</template>
							<template v-slot:cell(actions)="data">
								<b-button-group>
									<b-button variant="primary" @click="editModal(data.item.id)"
										><b-icon icon="pencil"
									/></b-button>
									<b-button variant="danger" @click="removeModal(data.item.id)"
										><b-icon icon="trash"
									/></b-button>
								</b-button-group>
							</template>
						</b-table>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
		<b-modal id="edit" title="Edit user" @ok="edit">
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
				<b-form-group label="Email">
					<b-form-input v-model="user.phone" type="phone"></b-form-input>
				</b-form-group>
				<b-form-group label="Role">
					<b-form-select
						v-model="user.role"
						:options="roles"
						required
					></b-form-select>
				</b-form-group>
			</b-form>
			<b-alert variant="danger" :show="showInvalid"
				>Check all fields before submitting</b-alert
			>
		</b-modal>
		<b-modal id="remove" title="Remove user" @ok="remove">
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
			key: 'phone',
			sortable: false,
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
			sortable: false,
		},
	];

	roles: string[] = ['Guest', 'Moderator', 'Admin'];
	user: IUser = this.emptyUser;
	userId: number;
	userToRemove: string = '';
	showInvalid: boolean = false;

	async mounted() {
		await this.$store.dispatch('users/pullUsers');
	}

	async refresh() {
		await this.$store.dispatch('users/pullUsers', true);
	}

	get users(): IUser[] {
		return this.$store.state.users.users;
	}

	get emptyUser() {
		return (this.user = {
			id: 0,
			name: '',
			surname: '',
			email: '',
			phone: '',
			role: '',
		});
	}

	userById(id: number) {
		const user = this.users.find(u => u.id === id);
		return { ...user } || this.emptyUser;
	}

	localDate(date: Date) {
		return date.toLocaleString('pl');
	}

	editModal(id: number) {
		this.userId = id;
		this.user = this.userById(id);
		this.$bvModal.show('edit');
	}

	removeModal(id: number) {
		this.userId = id;
		const user = this.userById(id);
		this.userToRemove = `${user.name} ${user.surname}`;
		this.$bvModal.show('remove');
	}

	async edit($event) {
		if (
			this.user.name &&
			this.user.surname &&
			this.user.email &&
			this.user.role
		) {
			if (this.user.id) {
				await this.$store.dispatch('users/edit', this.user);
			} else {
				await this.$store.dispatch('users/create', this.user);
			}
			this.showInvalid = false;
		} else {
			$event.preventDefault();
			this.showInvalid = true;
		}
	}

	async remove() {
		await this.$store.dispatch('users/remove', this.userId);
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
