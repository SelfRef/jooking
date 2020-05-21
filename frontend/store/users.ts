import { Commit, Dispatch } from 'vuex';
import { UsersClient, IUser, User } from '@/lib/Api';
type ActionContext = {
	commit: Commit;
	state: IState;
	dispatch: Dispatch;
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
	async create({ dispatch }: ActionContext, user: IUser) {
		const client = new UsersClient();
		user.id = 0;
		try {
			await client.postUser(new User(user));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullUsers', true);
	},
	async edit({ dispatch }: ActionContext, user: IUser) {
		const client = new UsersClient();
		try {
			await client.putUser(user.id || 0, new User(user));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullUsers', true);
	},
	async remove({ dispatch }: ActionContext, id: number) {
		const client = new UsersClient();
		try {
			await client.deleteUser(id);
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullUsers', true);
	},
};

export const getters = {};
