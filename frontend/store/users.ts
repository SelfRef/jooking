import { Commit } from 'vuex';
import { UsersClient, IUser } from '@/lib/Api';
type ActionContext = {
	commit: Commit;
	state: IState;
};
type UserData = {
	name: string | null;
	surname: string | null;
	email: string | null;
	registered: string | null;
	role: string | null;
};

interface IState {
	users: UserData[] | null;
}

export const state = (): IState => ({
	users: null,
});

export const mutations = {
	setUsers(state: IState, users: IUser[]) {
		const userData: UserData[] = users.map(u => {
			return {
				name: u.name ?? null,
				surname: u.surname ?? null,
				email: u.email ?? null,
				registered: u.registered?.toLocaleString('pl') ?? null,
				role: u.role ?? null,
			};
		});
		state.users = userData;
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
