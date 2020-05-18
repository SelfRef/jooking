<template>
	<div :class="$style.container">
		<header>
			<b-navbar variant="dark" type="dark">
				<b-navbar-brand>Jooking</b-navbar-brand>
				<b-navbar-nav>
					<b-nav-item to="/">Home</b-nav-item>
					<b-nav-item to="/hotels">Hotels</b-nav-item>
				</b-navbar-nav>
				<b-navbar-nav :class="$style.right">
					<b-nav-item-dropdown v-if="isLogged">
						<b-dropdown-item>Settings</b-dropdown-item>
						<hr />
						<b-dropdown-item>Logout</b-dropdown-item>
					</b-nav-item-dropdown>
					<b-nav-form v-else>
						<b-button variant="outline-primary" to="/logout">Login</b-button>
					</b-nav-form>
				</b-navbar-nav>
			</b-navbar>
		</header>
		<main>
			<nuxt />
		</main>
		<footer></footer>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';

@Component
export default class Laout extends Vue {
	get isLogged() {
		return this.$store.state.auth.isLogged;
	}
}
</script>

<style lang="scss" module>
.container {
	width: 100%;
	max-width: 1000px;
	height: 100%;
	display: flex;
	flex-direction: column;
	margin: auto;

	header {
		margin: 50px 0;
		border-radius: 5px;
		overflow: hidden;

		.right {
			flex: 1;
			justify-content: flex-end;
		}
	}
}
</style>

<style lang="scss">
html,
body {
	width: 100%;
	height: 100%;
	box-sizing: border-box;
}
</style>
