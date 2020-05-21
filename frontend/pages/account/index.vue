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
								<b-card-body title="Janusz Nosacz">
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
					<b-card :class="$style.reservations" title="Reservation history">
						<b-table :items="reservations"></b-table>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class Account extends Vue {
	get authData() {
		return this.$store.state.auth;
	}

	get reservations() {
		return [
			{
				hotel: 'Hotel Name',
				room: 'Onion Standard 2x1',
				startDate: '2020-01-01',
				endDate: '2020-01-05',
			},
			{
				hotel: 'Hotel Name',
				room: 'Onion Standard 2x1',
				startDate: '2020-01-01',
				endDate: '2020-01-05',
			},
			{
				hotel: 'Hotel Name',
				room: 'Onion Standard 2x1',
				startDate: '2020-01-01',
				endDate: '2020-01-05',
			},
			{
				hotel: 'Hotel Name',
				room: 'Onion Standard 2x1',
				startDate: '2020-01-01',
				endDate: '2020-01-05',
			},
		];
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
