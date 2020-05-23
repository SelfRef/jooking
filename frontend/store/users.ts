import { Commit, Dispatch } from 'vuex';
import { UsersClient, IUser, User } from '@/lib/Api';
type ActionContext = {
	commit: Commit;
	state: IState;
	dispatch: Dispatch;
	rootGetters;
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
	async pullUsers(
		{ commit, state, rootGetters }: ActionContext,
		force: boolean
	) {
		if (state.users === null || force) {
			const client = new UsersClient(
				undefined,
				rootGetters['auth/axiosInstance']
			);
			const users = await client.getUsers();
			commit('setUsers', users);
		}
	},
	async pullUser({ rootGetters }: ActionContext, id: number) {
		const client = new UsersClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		return await client.getUser(id);
	},
	async create({ dispatch, rootGetters }: ActionContext, user: IUser) {
		const client = new UsersClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		user.id = 0;
		try {
			await client.postUser2(new User(user));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullUsers', true);
	},
	async edit({ dispatch, rootGetters }: ActionContext, user: IUser) {
		const client = new UsersClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			await client.putUser(user.id || 0, new User(user));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullUsers', true);
	},
	async remove({ dispatch, rootGetters }: ActionContext, id: number) {
		const client = new UsersClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			await client.deleteUser(id);
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullUsers', true);
	},
};

export const getters = {};
