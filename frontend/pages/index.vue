<template>
	<div :class="$style.container">
		<b-jumbotron header="Hotelix" lead="Jakies info o stronie">
			<b-button variant="primary" to="/hotels">Go to hotel list</b-button>
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
