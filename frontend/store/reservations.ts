import { Commit, Dispatch } from 'vuex';
import { ReservationsClient, IReservation, Reservation } from '@/lib/Api';
type ActionContext = {
	commit: Commit;
	state: IState;
	dispatch: Dispatch;
	rootGetters;
};

interface IState {}

export const state = (): IState => ({});

export const mutations = {};

export const actions = {
	async create({ rootGetters }: ActionContext, reservation: IReservation) {
		const client = new ReservationsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			reservation.id = 0;
			await client.postReservation(new Reservation(reservation));
		} catch (e) {
			console.log(e);
		}
	},
};

export const getters = {};
