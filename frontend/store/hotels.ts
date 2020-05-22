import { Commit, Dispatch } from 'vuex';
import {
	HotelsClient,
	IHotel,
	Hotel,
	RoomsClient,
	IRoom,
	Room,
} from '@/lib/Api';
type ActionContext = {
	commit: Commit;
	state: IState;
	dispatch: Dispatch;
	rootGetters;
};

interface IState {
	hotels: IHotel[] | null;
}

export const state = (): IState => ({
	hotels: null,
});

export const mutations = {
	setHotels(state: IState, hotels: IHotel[]) {
		state.hotels = hotels;
	},
};

export const actions = {
	async pullHotels(
		{ commit, state, rootGetters }: ActionContext,
		force: boolean
	) {
		if (state.hotels === null || force) {
			const client = new HotelsClient(
				undefined,
				rootGetters['auth/axiosInstance']
			);
			const hotels = await client.getHotels();
			commit('setHotels', hotels);
		}
	},
	async pullHotel({ rootGetters }: ActionContext, id: string) {
		const client = new HotelsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		return await client.getHotel(Number(id));
	},
	async create({ dispatch, rootGetters }: ActionContext, hotel: IHotel) {
		const client = new HotelsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		hotel.id = 0;
		try {
			await client.postHotel(new Hotel(hotel));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async edit({ dispatch, rootGetters }: ActionContext, hotel: IHotel) {
		const client = new HotelsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			await client.putHotel(hotel.id || 0, new Hotel(hotel));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async remove({ dispatch, rootGetters }: ActionContext, id: number) {
		const client = new HotelsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			await client.deleteHotel(id);
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},

	async pullRoom({ rootGetters }: ActionContext, id: string) {
		const client = new RoomsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		return await client.getRoom(Number(id));
	},
	async createRoom({ dispatch, rootGetters }: ActionContext, room: IRoom) {
		const client = new RoomsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		room.id = 0;
		try {
			await client.postRoom(new Room(room));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async editRoom({ dispatch, rootGetters }: ActionContext, room: IRoom) {
		const client = new RoomsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			await client.putRoom(room.id || 0, new Room(room));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async removeRoom({ dispatch, rootGetters }: ActionContext, id: number) {
		const client = new RoomsClient(
			undefined,
			rootGetters['auth/axiosInstance']
		);
		try {
			await client.deleteRoom(id);
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
};

export const getters = {};
