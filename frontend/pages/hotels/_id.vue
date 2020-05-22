<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-card no-body>
						<b-row no-gutters>
							<b-col md="4">
								<b-card-img
									:src="`https://picsum.photos/id/${1000 + id}/300?blur`"
								></b-card-img>
							</b-col>
							<b-col md="8">
								<b-card-body :title="hotel.name">
									<b-card-text>{{ hotel.description }} </b-card-text>
									<b-card-text
										>Phone number:
										<b-link>{{ hotel.phone }}</b-link></b-card-text
									>
									<b-card-text
										>Email address:
										<b-link>{{ hotel.email }}</b-link></b-card-text
									>
								</b-card-body>
							</b-col>
						</b-row>
					</b-card>
				</b-col>
			</b-row>
			<b-row>
				<b-col>
					<b-card :class="$style.form" bg-variant="light">
						<b-alert v-if="submitted" :show="submitted" variant="success"
							>Thank you for your reservation!</b-alert
						>
						<b-form-group
							v-else
							label="Make a reservation"
							description="Note: Fields with * are required"
							label-size="lg"
						>
							<b-form @submit.prevent="submit">
								<b-form-group
									label="Stay beginning*"
									description="When your stay shoud begin"
									label-cols-md="4"
									label-for="startDate"
								>
									<b-form-datepicker
										id="startDate"
										v-model="formData.startDate"
										value-as-date
										:min="minDateStart"
										:state="startDateState"
										required
									></b-form-datepicker>
								</b-form-group>
								<b-form-group
									label="Stay ending*"
									description="When your stay should end"
									label-cols-md="4"
									label-for="endDate"
								>
									<b-form-datepicker
										id="endDate"
										v-model="formData.endDate"
										value-as-date
										:min="minDateEnd"
										:disabled="!formData.startDate"
										:state="endDateState"
										required
									></b-form-datepicker>
								</b-form-group>
								<b-form-group
									v-if="daysDiff"
									label="Your selection"
									label-cols-md="4"
								>
									<b-alert :show="daysDiff > 0"
										>You've selected {{ daysDiff }} days.</b-alert
									>
									<b-alert :show="daysDiff < 0" variant="danger"
										>Ending date have to be at least one day after beginning
										date.</b-alert
									>
								</b-form-group>
								<b-form-group
									label="Room*"
									description="Choose room standard"
									label-cols-md="4"
									label-for="room"
								>
									<b-form-select
										id="room"
										v-model="formData.roomId"
										required
										:options="roomOptions"
									></b-form-select>
								</b-form-group>
								<b-form-group
									label="Phone number*"
									description="Enter contact phone number"
									label-cols-md="4"
									required
								>
									<b-form-input v-model="formData.phone"></b-form-input>
								</b-form-group>
								<b-form-group
									label="Email"
									description="Enter contact email address"
									label-cols-md="4"
								>
									<b-form-input v-model="formData.email"></b-form-input>
								</b-form-group>
								<b-button
									class="float-right"
									type="submit"
									variant="primary"
									:disabled="!formValid"
								>
									Submit
								</b-button>
							</b-form>
						</b-form-group>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { IHotelResponse, IRoom } from '@/lib/Api';

@Component
export default class HotelDetails extends Vue {
	formData = {
		startDate: '',
		endDate: '',
		roomId: null,
		phone: '',
		email: '',
		userId: 3,
	};

	hotel: IHotelResponse = {};
	submitted = false;

	async mounted() {
		this.hotel = await this.$store.dispatch(
			'hotels/pullHotel',
			this.$route.params.id
		);
	}

	get id() {
		return Number(this.$route.params.id);
	}

	get minDateStart() {
		const date = new Date();
		date.setDate(date.getDate() + 1);
		return date;
	}

	get minDateEnd() {
		const date = new Date(this.minDateStart);
		date.setDate(date.getDate() + 1);
		return date;
	}

	get startDateState() {
		if (!this.formData.startDate) return null;
		if (this.formData.endDate && this.daysDiff < 1) return false;
		return true;
	}

	get endDateState() {
		if (!this.formData.endDate) return null;
		if (this.formData.startDate && this.daysDiff < 1) return false;
		return true;
	}

	get daysDiff() {
		if (!this.formData.startDate || !this.formData.endDate) return 0;
		const startDate = Number(new Date(this.formData.startDate));
		const endDate = Number(new Date(this.formData.endDate));
		const oneDay = 24 * 60 * 60 * 1000;
		const diffDays = Math.round((endDate - startDate) / oneDay);
		return diffDays;
	}

	get formValid() {
		if (!this.startDateState) return false;
		if (!this.endDateState) return false;
		if (!this.formData.roomId) return false;
		if (!this.formData.phone) return false;
		return true;
	}

	get roomOptions() {
		const rooms: IRoom[] = this.hotel?.rooms ?? [];
		return rooms.map(r => {
			return {
				value: r.id,
				text: `${r.standard} ${r.bedCount}x${r.bedSize} (no ${r.number})`,
			};
		});
	}

	async submit() {
		await this.$store.dispatch('reservations/create', this.formData);
		this.submitted = true;
	}
}
</script>

<style lang="scss" module>
.form {
	margin: 50px 0;
}
.right {
	justify-self: flex-end;
}
</style>
