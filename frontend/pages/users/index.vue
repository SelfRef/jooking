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
						<b-table
							:items="users"
							:fields="fields"
							selectable
							select-mode="single"
							@row-selected="selectRow"
						>
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
			<b-row :class="$style.reservationsRow">
				<b-col>
					<b-alert v-if="!selectedUser" show
						>Select user to show reservation list</b-alert
					>
					<b-card v-else>
						<template v-slot:header>
							<h4 class="float-left">Reservations</h4>
						</template>
						<b-table
							v-if="
								selectedUser &&
									selectedUser.reservations &&
									selectedUser.reservations.length > 0
							"
							:items="selectedUser.reservations"
							:fields="fieldsRoom"
						>
							<template v-slot:cell(hotel)="data">
								{{ data.item.room.hotel.name }}
							</template>
							<template v-slot:cell(room)="data">
								{{ roomName(data.item.room) }}
							</template>
							<template v-slot:cell(startDate)="data">
								{{ localDate(data.item.startDate) }}
							</template>
							<template v-slot:cell(endDate)="data">
								{{ localDate(data.item.endDate) }}
							</template>
							<template v-slot:cell(actions)="data">
								<b-button-group>
									<b-button
										variant="danger"
										@click="removeReservationModal(data.item.id)"
										><b-icon icon="trash"
									/></b-button>
								</b-button-group>
							</template>
						</b-table>
						<b-alert v-else show
							>This user does not have any reservations</b-alert
						>
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
				<b-form-group label="Phone">
					<b-form-input v-model="user.phone" type="tel"></b-form-input>
				</b-form-group>
				<b-form-group label="Role">
					<b-form-select
						v-model="user.role"
						:options="roles"
						required
					></b-form-select>
				</b-form-group>
				<b-form-group label="Password">
					<b-form-input
						v-model="user.password"
						placeholder="Enter to change"
						type="password"
					></b-form-input>
				</b-form-group>
			</b-form>
			<b-alert variant="danger" :show="showInvalid"
				>Check all fields before submitting</b-alert
			>
		</b-modal>
		<b-modal id="remove" title="Remove user" @ok="remove">
			Do you want to remove this user: {{ userToRemove }}?
		</b-modal>
		<b-modal
			id="removeReservation"
			title="Remove reservation"
			@ok="removeReservation"
		>
			Do you want to remove this reservation?
		</b-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { IUser, IRoom, IReservation } from '@/lib/Api';

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

	fieldsRoom = [
		{
			key: 'hotel',
			sortable: true,
		},
		{
			key: 'room',
			sortable: true,
		},
		{
			key: 'startDate',
			sortable: true,
		},
		{
			key: 'endDate',
			sortable: true,
		},
		{
			key: 'phone',
			sortable: false,
		},
		{
			key: 'email',
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
	reservation: IReservation = {};
	reservationId: number;
	showInvalid: boolean = false;
	selectedUser: IUser | null = null;

	async mounted() {
		if (this.$store.getters['auth/role'] !== 'Admin') {
			this.$router.push('/');
		} else {
			await this.$store.dispatch('users/pullUsers');
		}
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
			password: '',
			role: '',
		});
	}

	userById(id: number) {
		const user = this.users.find(u => u.id === id);
		return { ...user } || this.emptyUser;
	}

	reservationById(id: number) {
		const reservation = this.selectedUser?.reservations?.find(r => r.id === id);
		return { ...reservation } || {};
	}

	roomName(room: IRoom) {
		return `${room.standard} ${room.bedCount}x${room.bedSize} (no ${room.number})`;
	}

	localDate(date: Date) {
		if (typeof date === 'string') date = new Date(date);
		return date.toLocaleDateString('pl');
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

	editReservationModal(id: number) {
		this.reservationId = id;
		this.reservation = this.reservationById(id);
		this.$bvModal.show('editReservation');
	}

	removeReservationModal(id: number) {
		this.reservationId = id;
		this.$bvModal.show('removeReservation');
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

	async removeReservation() {
		await this.$store.dispatch('reservations/remove', this.reservationId);
		await this.selectUser(this.selectedUser?.id);
	}

	selectRow(rows) {
		if (rows[0]) this.selectUser(rows[0].id);
		else this.selectedUser = null;
	}

	async selectUser(id) {
		this.selectedUser = await this.$store.dispatch('users/pullUser', id);
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
.reservationsRow {
	margin-top: 50px;
}
</style>
