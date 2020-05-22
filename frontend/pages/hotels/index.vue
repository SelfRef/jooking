<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col>
					<b-button v-if="canEdit" variant="primary" to="/hotels/edit"
						>Edit hotels</b-button
					>
				</b-col>
			</b-row>
			<b-row>
				<b-col
					v-for="hotel in hotels"
					:key="hotel.id"
					:class="$style.col"
					cols="12"
					sm="6"
					md="4"
				>
					<b-card
						:title="hotel.name"
						:img-src="`https://picsum.photos/id/${1000 + hotel.id}/300?blur`"
					>
						<b-card-text>{{ hotel.description }}</b-card-text>
						<b-button variant="success" @click="goToHotel(hotel.id)"
							>Book a room</b-button
						>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { IHotel } from '@/lib/Api';

@Component
export default class Hotels extends Vue {
	async mounted() {
		await this.$store.dispatch('hotels/pullHotels');
	}

	get hotels(): IHotel[] {
		return this.$store.state.hotels.hotels;
	}

	get canEdit() {
		return (
			this.$store.state.auth.isLogged &&
			this.$store.getters['auth/role'] !== 'Guest'
		);
	}

	goToHotel(id: number) {
		if (this.$store.state.auth.isLogged) this.$router.push('/hotels/' + id);
		else this.$bvModal.show('login');
	}
}
</script>

<style lang="scss" module>
.container {
}
.col {
	padding: 15px;
}
</style>
