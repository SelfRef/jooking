type State = {
	isLogged: boolean;
};

export const state = (): State => ({
	isLogged: false,
});

export const mutations = {
	setLogged(state: State, value: boolean) {
		state.isLogged = value;
	},
};

export const getters = {};
