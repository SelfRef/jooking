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
								<b-card-body title="Hotel Name">
									<b-card-text
										>Lorem Ipsum is simply dummy text of the printing and
										typesetting industry.
									</b-card-text>
									<b-card-text>
										Reception open:
										<ul>
											<li>Mon-Fri: 06:00 - 23:00</li>
											<li>Sat: 07:00 - 22:00</li>
											<li>Sun: 08:00 - 21:00</li>
										</ul>
									</b-card-text>
									<b-card-text>Phone number: 123456789</b-card-text>
									<b-link>Website</b-link>
								</b-card-body>
							</b-col>
						</b-row>
					</b-card>
				</b-col>
			</b-row>
			<b-row>
				<b-col>
					<b-card :class="$style.form" bg-variant="light">
						<b-form-group
							label="Make a reservation"
							description="Note: Fields with * are required"
							label-size="lg"
						>
							<b-form>
								<b-form-group
									label="Stay beginning*"
									description="When your stay shoud begin"
									label-cols-md="4"
									label-for="startDate"
								>
									<b-form-datepicker
										id="startDate"
										v-model="formData.start"
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
										v-model="formData.end"
										:min="minDateEnd"
										:disabled="!formData.start"
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
										v-model="formData.room"
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

@Component
export default class HotelDetails extends Vue {
	formData = {
		start: '',
		end: '',
		room: null,
		phone: '',
		email: '',
	};

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
		if (!this.formData.start) return null;
		if (this.formData.end && this.daysDiff < 1) return false;
		return true;
	}

	get endDateState() {
		if (!this.formData.end) return null;
		if (this.formData.start && this.daysDiff < 1) return false;
		return true;
	}

	get daysDiff() {
		if (!this.formData.start || !this.formData.end) return 0;
		const startDate = Number(new Date(this.formData.start));
		const endDate = Number(new Date(this.formData.end));
		const oneDay = 24 * 60 * 60 * 1000;
		const diffDays = Math.round((endDate - startDate) / oneDay);
		return diffDays;
	}

	get formValid() {
		if (!this.startDateState) return false;
		if (!this.endDateState) return false;
		if (!this.formData.room) return false;
		if (!this.formData.phone) return false;
		return true;
	}

	get roomOptions() {
		return [
			{ value: null, text: 'Please select room' },
			{ value: 1, text: 'Onion Standard 2x1' },
			{ value: 2, text: 'Comfort Standard 1x2' },
			{ value: 3, text: 'Luxury Standard 1x2' },
		];
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
