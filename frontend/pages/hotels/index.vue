<template>
	<div :class="$style.container">
		<b-container>
			<b-row cols="3">
				<b-col v-for="i in 6" :key="i" :class="$style.col">
					<b-card
						title="Hotel Name"
						:img-src="`https://picsum.photos/id/${1000 + i}/300?blur`"
					>
						<b-card-text>Lorem ipsum</b-card-text>
						<b-button variant="success">Show rooms</b-button>
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
