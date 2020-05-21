<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-card>
						<template v-slot:header>
							<h4 class="float-left">Hotels</h4>
							<b-button-group class="float-right">
								<b-button @click="refresh">
									Refresh
								</b-button>
								<b-button variant="success" @click="editModal">
									Add new hotel
								</b-button>
							</b-button-group>
						</template>
						<b-table :items="hotels" :fields="fields">
							<template v-slot:cell(userId)="data">
								{{ userNameById(data.item.userId) }}
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
					<b-form-input
						v-model="hotel.name"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Description">
					<b-form-input
						v-model="hotel.description"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Email">
					<b-form-input
						v-model="hotel.email"
						type="email"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Phone">
					<b-form-input
						v-model="hotel.phone"
						type="phone"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Owner">
					<b-form-select
						v-model="hotel.userId"
						:options="userOptions"
						required
					></b-form-select>
				</b-form-group>
			</b-form>
			<b-alert variant="danger" :show="showInvalid"
				>Check all fields before submitting</b-alert
			>
		</b-modal>
		<b-modal id="remove" title="Remove hotel" @ok="remove">
			Do you want to remove this hotel: {{ hotelToRemove }}?
		</b-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { IHotel, IUser } from '@/lib/Api';

@Component
export default class Users extends Vue {
	fields = [
		{
			key: 'name',
			sortable: true,
		},
		{
			key: 'description',
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
			key: 'userId',
			label: 'User',
			sortable: true,
		},
		{
			key: 'actions',
			sortable: false,
		},
	];

	hotel: IHotel = this.emptyHotel;
	hotelId: number;
	hotelToRemove: string = '';
	showInvalid: boolean = false;

	async mounted() {
		await this.$store.dispatch('hotels/pullHotels');
		await this.$store.dispatch('users/pullUsers');
	}

	async refresh() {
		await this.$store.dispatch('hotels/pullHotels', true);
	}

	get users(): IUser[] {
		return this.$store.state.users.users;
	}

	get hotels(): IHotel[] {
		return this.$store.state.hotels.hotels;
	}

	get emptyHotel() {
		return (this.hotel = {
			id: 0,
			name: '',
			description: '',
			email: '',
			phone: '',
			userId: 0,
		});
	}

	get userOptions() {
		if (!this.users) return [];
		return this.users.map(u => {
			return {
				value: u.id,
				text: this.userNameById(u.id),
			};
		});
	}

	hotelById(id: number) {
		const hotel = this.hotels.find(h => h.id === id);
		return { ...hotel } || this.emptyHotel;
	}

	userById(id: number) {
		const user = this.users?.find(u => u.id === id) ?? {};
		return { ...user };
	}

	userNameById(id: number) {
		const user = this.userById(id);
		return `${user.name} ${user.surname}`;
	}

	localDate(date: Date) {
		return date.toLocaleString('pl');
	}

	editModal(id: number) {
		this.hotelId = id;
		this.hotel = this.hotelById(id);
		this.$bvModal.show('edit');
	}

	removeModal(id: number) {
		this.hotelId = id;
		const hotel = this.hotelById(id);
		this.hotelToRemove = `${hotel.name}`;
		this.$bvModal.show('remove');
	}

	async edit($event) {
		if (
			this.hotel.name &&
			this.hotel.description &&
			this.hotel.email &&
			this.hotel.phone &&
			this.hotel.userId
		) {
			if (this.hotel.id) {
				await this.$store.dispatch('hotels/edit', this.hotel);
			} else {
				await this.$store.dispatch('hotels/create', this.hotel);
			}
			this.showInvalid = false;
		} else {
			$event.preventDefault();
			this.showInvalid = true;
		}
	}

	async remove() {
		await this.$store.dispatch('hotels/remove', this.hotelId);
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
