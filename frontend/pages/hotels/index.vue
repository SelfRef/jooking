<template>
	<div :class="$style.container">
		<b-container>
			<b-row>
				<b-col
					v-for="i in 5"
					:key="i"
					:class="$style.col"
					cols="12"
					sm="6"
					md="4"
				>
					<b-card
						title="Hotel Name"
						:img-src="`https://picsum.photos/id/${1000 + i}/300?blur`"
					>
						<b-card-text>Lorem ipsum</b-card-text>
						<b-button variant="success" :to="`/hotels/${i}`"
							>Book a room</b-button
						>
					</b-card>
				</b-col>
				<b-col :class="$style.col" cols="12" sm="6" md="4">
					<b-card
						title="Hotel Name"
						:img-src="`https://picsum.photos/300?blur`"
					>
						<b-card-text>Lorem ipsum</b-card-text>
						<b-button variant="danger" :to="`/hotels/1`">No vacancy</b-button>
					</b-card>
				</b-col>
			</b-row>
		</b-container>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class Hotels extends Vue {}
</script>

<style lang="scss" module>
.container {
}
.col {
	padding: 15px;
}
</style>
