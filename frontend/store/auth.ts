import { Commit, Dispatch } from 'vuex';
import { IUser, UsersClient, Login } from '@/lib/Api';

type ActionContext = {
	commit: Commit;
	state: IState;
	dispatch: Dispatch;
};
type IState = {
	isLogged: boolean;
	user: IUser | null;
};

export const state = (): IState => ({
	isLogged: false,
	user: null,
});

export const mutations = {
	setLogged(state: IState, value: boolean) {
		state.isLogged = value;
	},
	setUser(state: IState, user: IUser) {
		state.user = user;
	},
};

export const actions = {
	async login({ commit }: ActionContext, login: Login) {
		const client = new UsersClient();
		try {
			const user = await client.authenticate(login);
			commit('setUser', user);
			commit('setLogged', true);
		} catch (e) {
			commit('setLogged', false);
			throw e;
		}
	},
	logout({ commit }: ActionContext) {
		commit('setUser', null);
		commit('setLogged', false);
	},
};
export const getters = {
	token(state: IState): string {
		return state.user?.token ?? '';
	},
};
