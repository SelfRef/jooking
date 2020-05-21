import { Commit } from 'vuex';
import { UsersClient, IUser } from '@/lib/Api';
type ActionContext = {
	commit: Commit;
	state: IState;
};

interface IState {
	users: IUser[] | null;
}

export const state = (): IState => ({
	users: null,
});

export const mutations = {
	setUsers(state: IState, users: IUser[]) {
		state.users = users;
	},
};

export const actions = {
	async pullUsers({ commit, state }: ActionContext, force: boolean) {
		if (state.users === null || force) {
			const client = new UsersClient();
			const users = await client.getUsers();
			commit('setUsers', users);
		}
	},
};

export const getters = {};
