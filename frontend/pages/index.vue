<template>
	<div :class="$style.container">
		<b-jumbotron header="Jooking" lead="Jooking ya Booking">
			<p>
				Lorem Ipsum is simply dummy text of the printing and typesetting
				industry. Lorem Ipsum has been the industry's standard dummy text ever
				since the 1500s, when an unknown printer took a galley of type and
				scrambled it to make a type specimen book. It has survived not only five
				centuries, but also the leap into electronic typesetting, remaining
				essentially unchanged. It was popularised in the 1960s with the release
				of Letraset sheets containing Lorem Ipsum passages, and more recently
				with desktop publishing software like Aldus PageMaker including versions
				of Lorem Ipsum.
			</p>
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
