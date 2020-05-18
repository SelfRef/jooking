<template>
	<div :class="$style.container">
		<b-jumbotron header="Jooking" lead="Jooking ya Booking">
			<b-button variant="primary" to="/hotels">Hotels</b-button>
		</b-jumbotron>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class index extends Vue {}
</script>

<style lang="scss" module>
.container {
}
</style>
