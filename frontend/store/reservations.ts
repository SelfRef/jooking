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
		reservation.id = 0;
		await client.postReservation(new Reservation(reservation));
	},
	async remove({ rootGetters }: ActionContext, id: number) {
		const client = new ReservationsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		await client.deleteReservation(id);
	},
	async pullMyReservations({ rootGetters }: ActionContext) {
		const client = new ReservationsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		return await client.getReservationUser();
	},
};

export const getters = {};
