type State = {
	isLogged: boolean;
	name: string;
	email: string;
	phone: string;
};

export const state = (): State => ({
	isLogged: true,
	name: 'Janusz',
	email: 'janusz.nosacz@januszex.com',
	phone: '+48 123456789',
});

export const mutations = {
	setLogged(state: State, value: boolean) {
		state.isLogged = value;
	},
};

export const getters = {};
