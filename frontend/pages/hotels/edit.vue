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
						<b-table
							:items="hotels"
							:fields="fields"
							selectable
							select-mode="single"
							@row-selected="selectRow"
						>
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
			<b-row v-if="selectedHotel" :class="$style.roomsRow">
				<b-col>
					<b-card>
						<template v-slot:header>
							<h4 class="float-left">Rooms</h4>
							<b-button-group class="float-right">
								<b-button @click="refresh">
									Refresh
								</b-button>
								<b-button variant="success" @click="editModal">
									Add new room
								</b-button>
							</b-button-group>
						</template>
						<b-table :items="selectedHotel.rooms" :fields="roomFields">
							<template v-slot:cell(actions)="data">
								<b-button-group>
									<b-button
										variant="primary"
										@click="editRoomModal(data.item.id)"
										><b-icon icon="pencil"
									/></b-button>
									<b-button
										variant="danger"
										@click="removeRoomModal(data.item.id)"
										><b-icon icon="trash"
									/></b-button>
								</b-button-group>
							</template>
						</b-table>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
		<b-modal id="edit" title="Edit hotel" @ok="edit">
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
		<b-modal
			v-if="selectedHotel"
			id="editRoom"
			title="Edit room"
			@ok="editRoom"
		>
			<b-form>
				<b-form-group label="Number">
					<b-form-input
						v-model="selectedHotel.number"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Standard">
					<b-form-select
						v-model="selectedHotel.standard"
						:options="roomStandards"
						required
					></b-form-select>
				</b-form-group>
				<b-form-group label="Bed Count">
					<b-form-input
						v-model="selectedHotel.bedCount"
						type="number"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Bed Size">
					<b-form-input
						v-model="selectedHotel.bedSize"
						type="number"
						required
					></b-form-input>
				</b-form-group>
			</b-form>
			<b-alert variant="danger" :show="showInvalid"
				>Check all fields before submitting</b-alert
			>
		</b-modal>
		<b-modal id="removeRoom" title="Remove hotel" @ok="removeRoom">
			Do you want to remove this hotel: {{ hotelToRemove }}?
		</b-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { IHotel, IUser, RoomStandard, IRoom } from '@/lib/Api';

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

	roomFields = [
		{
			key: 'number',
			sortable: true,
		},
		{
			key: 'standard',
			sortable: true,
		},
		{
			key: 'bedCount',
			sortable: true,
		},
		{
			key: 'bedSize',
			sortable: false,
		},
		{
			key: 'actions',
			sortable: false,
		},
	];

	hotel: IHotel = this.emptyHotel;
	hotelId!: number;
	hotelToRemove: string = '';
	room: IRoom = this.emptyRoom;
	roomId!: number;
	roomToRemove: string = '';
	showInvalid: boolean = false;
	showRoomInvalid: boolean = false;
	selectedHotel: IHotel | null = null;

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

	get emptyRoom() {
		return (this.room = {
			id: 0,
			number: undefined,
			standard: undefined,
			bedCount: undefined,
			bedSize: undefined,
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

	get roomStandards() {
		return RoomStandard;
	}

	hotelById(id: number) {
		const hotel = this.hotels.find(h => h.id === id);
		return { ...hotel } || this.emptyHotel;
	}

	roomById(id: number) {
		const room = this.selectedHotel?.rooms?.find(r => r.id === id) ?? null;
		return { ...room } || this.emptyRoom;
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

	editRoomModal(id: number) {
		this.roomId = id;
		this.room = this.roomById(id);
		this.$bvModal.show('editRoom');
	}

	removeRoomModal(id: number) {
		this.roomId = id;
		const room = this.roomById(id);
		this.roomToRemove = `${room.standard} ${room.bedCount}x${room.bedSize} (${room.number})`;
		this.$bvModal.show('removeRoom');
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

	async editRoom($event) {
		if (
			this.room.number &&
			this.room.standard &&
			this.room.bedCount &&
			this.room.bedSize
		) {
			if (this.room.id) {
				await this.$store.dispatch('hotels/editRoom', this.room);
			} else {
				await this.$store.dispatch('rooms/createRoom', this.room);
			}
			this.showInvalid = false;
		} else {
			$event.preventDefault();
			this.showInvalid = true;
		}
	}

	async removeRoom() {
		await this.$store.dispatch('hotels/removeRoom', this.roomId);
	}

	selectRow(rows) {
		if (rows[0]) this.selectHotel(rows[0].id);
		else this.selectedHotel = null;
	}

	async selectHotel(id) {
		this.selectedHotel = await this.$store.dispatch('hotels/pullHotel', id);
	}
}
</script>

<style lang="scss" module>
.inputGroup {
	margin: 20px 0;
}
.reservations,
.roomsRow {
	margin-top: 50px;
}
</style>
