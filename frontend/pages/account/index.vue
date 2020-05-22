<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-card no-body>
						<b-row no-gutters>
							<b-col md="4">
								<b-card-img
									src="https://1.bp.blogspot.com/-4D9PFKCx9XE/T5lkT6VG82I/AAAAAAAAFcA/YWZHV_qBa3k/s1600/proboscis_monkey.jpg"
								></b-card-img>
							</b-col>
							<b-col md="8">
								<b-card-body :title="`${authData.name} ${authData.surname}`">
									<b-input-group prepend="Email" :class="$style.inputGroup">
										<b-form-input
											readonly
											:value="authData.email"
										></b-form-input>
									</b-input-group>
									<b-input-group prepend="Phone">
										<b-form-input
											readonly
											:value="authData.phone"
										></b-form-input>
									</b-input-group>
								</b-card-body>
							</b-col>
						</b-row>
					</b-card>
				</b-col>
			</b-row>
			<b-row>
				<b-col>
					<b-card :class="$style.reservations">
						<h4 class="float-left">Reservation history</h4>
						<b-button-group class="float-right" :class="$style.header">
							<b-button @click="generatePdf()">Generate PDF from all</b-button>
						</b-button-group>
						<b-table
							v-if="reservations.length > 0"
							:items="reservations"
							:fields="fields"
						>
							<template v-slot:cell(startDate)="data">
								{{ localDate(data.item.startDate) }}
							</template>
							<template v-slot:cell(endDate)="data">
								{{ localDate(data.item.endDate) }}
							</template>
							<template v-slot:cell(hotel)="data">
								{{ data.item.room.hotel.name }}
							</template>
							<template v-slot:cell(room)="data">
								{{ roomName(data.item.room) }}
							</template>
							<template v-slot:cell(actions)="data">
								<b-button-group>
									<b-button variant="primary" @click="generatePdf(data.item)"
										><b-icon icon="download"
									/></b-button>
								</b-button-group>
							</template>
						</b-table>
						<b-alert v-else>No reservations to show</b-alert>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import Jspdf from 'jspdf';
import { IReservation, IRoom } from '@/lib/Api';

@Component
export default class Account extends Vue {
	reservations: IReservation[] = [];
	fields = [
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

	async mounted() {
		this.reservations = await this.$store.dispatch(
			'reservations/pullMyReservations'
		);
	}

	get authData() {
		return this.$store.state.auth.user || {};
	}

	roomName(room: IRoom) {
		return `${room.standard} ${room.bedCount}x${room.bedSize} (no ${room.number})`;
	}

	localDate(date: Date) {
		return date.toLocaleDateString('pl');
	}

	addRow(item: IReservation) {
		let data = '';
		data += '<tr>';
		data += `<td>${item.room?.hotel?.name ?? '[none]'}</td>`;
		data += `<td>${item.room ? this.roomName(item.room) : '[none]'}</td>`;
		data += `<td>${
			item.startDate ? this.localDate(item.startDate) : '[none]'
		}</td>`;
		data += `<td>${
			item.endDate ? this.localDate(item.endDate) : '[none]'
		}</td>`;
		data += `<td>${item.phone ?? '[none]'}</td>`;
		data += `<td>${item.email ?? '[none]'}</td>`;
		data += '</tr>';
		return data;
	}

	generateTable(item?: IReservation) {
		let data = '<table style="width: 100%;"><tr>';
		const headers = [
			'Hotel',
			'Room',
			'Start Date',
			'End Date',
			'Phone',
			'Email',
		];
		headers.forEach(h => {
			data += `<th>${h}</th>`;
		});
		data += '</tr>';
		if (item) data += this.addRow(item);
		else
			this.reservations.forEach(r => {
				data += this.addRow(r);
			});
		data += '</table>';
		return data;
	}

	generatePdf(item?: IReservation) {
		const doc = new Jspdf();
		doc.setFontSize(16);
		doc.text(20, 20, 'Reservation list');
		doc.fromHTML(this.generateTable(item), 15, 15, {
			width: 200,
		});
		doc.save();
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
.header {
	margin-bottom: 10px;
}
</style>
