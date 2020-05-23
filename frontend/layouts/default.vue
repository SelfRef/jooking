<template>
	<div :class="$style.container">
		<header>
			<b-navbar variant="dark" type="dark" :class="$style.navbar">
				<b-navbar-brand>{{ $store.state.app.name }}</b-navbar-brand>
				<b-navbar-nav>
					<b-nav-item to="/">Home</b-nav-item>
					<b-nav-item to="/hotels">Hotels</b-nav-item>
					<b-nav-item v-if="$store.getters['auth/role'] === 'Admin'" to="/users"
						>Users</b-nav-item
					>
				</b-navbar-nav>
				<b-navbar-nav :class="$style.right">
					<b-nav-item-dropdown v-if="isLogged" :text="userName">
						<b-dropdown-item to="/account">Account</b-dropdown-item>
						<hr />
						<b-dropdown-item @click="logout">Logout</b-dropdown-item>
					</b-nav-item-dropdown>
					<b-nav-form v-else>
						<b-button-group>
							<b-button v-b-modal.register variant="success">Register</b-button>
							<b-button v-b-modal.login variant="primary">Login</b-button>
						</b-button-group>
					</b-nav-form>
				</b-navbar-nav>
			</b-navbar>
		</header>
		<main>
			<nuxt />
		</main>
		<footer></footer>
		<b-modal id="login" title="Login" @ok="login">
			<b-form @submit.prevent>
				<b-form-group label="Email">
					<b-form-input
						v-model="loginForm.email"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Password">
					<b-form-input
						v-model="loginForm.password"
						type="password"
						required
					></b-form-input>
				</b-form-group>
			</b-form>
			<b-alert :show="Boolean(loginError)" variant="danger">{{
				loginError
			}}</b-alert>
		</b-modal>
		<b-modal id="register" title="Register" @ok="register">
			<b-form @submit.prevent>
				<b-form-group label="Name">
					<b-form-input
						v-model="registerForm.name"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Surname">
					<b-form-input
						v-model="registerForm.surname"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Email">
					<b-form-input
						v-model="registerForm.email"
						type="text"
						required
					></b-form-input>
				</b-form-group>
				<b-form-group label="Phone">
					<b-form-input
						v-model="registerForm.phone"
						required
						type="text"
					></b-form-input>
				</b-form-group>
				<b-form-group label="Password">
					<b-form-input
						v-model="registerForm.password"
						type="password"
						required
					></b-form-input>
				</b-form-group>
			</b-form>
			<b-alert :show="Boolean(registerError)" variant="danger">{{
				registerError
			}}</b-alert>
		</b-modal>
	</div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator';
import { Login, UserRegisterRequest } from '@/lib/Api';

@Component
export default class Layout extends Vue {
	get isLogged() {
		return this.$store.state.auth.isLogged;
	}

	get userName() {
		return `${this.$store.state.auth.user.name} ${this.$store.state.auth.user.surname}`;
	}

	get formValid() {
		return this.loginForm.email && this.loginForm.password;
	}

	get registerFormValid() {
		return (
			this.registerForm.email &&
			this.registerForm.password &&
			this.registerForm.name &&
			this.registerForm.surname &&
			this.registerForm.phone
		);
	}

	loginForm: Login = new Login();
	registerForm: UserRegisterRequest = new UserRegisterRequest();
	loginError = '';
	registerError = '';

	async login($event) {
		$event.preventDefault();
		if (!this.formValid) {
			this.loginError = 'Fill all fields';
			return;
		}
		try {
			await this.$store.dispatch('auth/login', this.loginForm);
			this.loginError = '';
			this.$bvModal.hide('login');
			this.loginForm.email = '';
		} catch (e) {
			const response = e.response.message;
			this.loginError = response;
		} finally {
			this.loginForm.password = '';
		}
	}

	async register($event) {
		$event.preventDefault();
		if (!this.registerFormValid) {
			this.registerError = 'Fill all fields';
			return;
		}
		try {
			await this.$store.dispatch('auth/register', this.registerForm);
			this.registerError = '';
			this.$bvModal.hide('register');
			this.registerForm = new UserRegisterRequest();
		} catch (e) {
			const response = e.response.message;
			this.registerError = response;
		} finally {
			this.loginForm.password = '';
		}
	}

	logout() {
		this.$router.push('/');
		this.$store.dispatch('auth/logout');
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

	.navbar {
		margin: 50px 0;
		border-radius: 0.25rem;
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
