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
	async pullHotels({ commit, state }: ActionContext, force: boolean) {
		if (state.hotels === null || force) {
			const client = new HotelsClient();
			const hotels = await client.getHotels();
			commit('setHotels', hotels);
		}
	},
	async pullHotel(_context: ActionContext, id: string) {
		const client = new HotelsClient();
		return await client.getHotel(Number(id));
	},
	async create({ dispatch }: ActionContext, hotel: IHotel) {
		const client = new HotelsClient();
		hotel.id = 0;
		try {
			await client.postHotel(new Hotel(hotel));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async edit({ dispatch }: ActionContext, hotel: IHotel) {
		const client = new HotelsClient();
		try {
			await client.putHotel(hotel.id || 0, new Hotel(hotel));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async remove({ dispatch }: ActionContext, id: number) {
		const client = new HotelsClient();
		try {
			await client.deleteHotel(id);
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},

	async pullRoom(_context: ActionContext, id: string) {
		const client = new RoomsClient();
		return await client.getRoom(Number(id));
	},
	async createRoom({ dispatch }: ActionContext, room: IRoom) {
		const client = new RoomsClient();
		room.id = 0;
		try {
			await client.postRoom(new Room(room));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async editRoom({ dispatch }: ActionContext, room: IRoom) {
		const client = new RoomsClient();
		try {
			await client.putRoom(room.id || 0, new Room(room));
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
	async removeRoom({ dispatch }: ActionContext, id: number) {
		const client = new RoomsClient();
		try {
			await client.deleteRoom(id);
		} catch (e) {
			console.log(e);
		}
		await dispatch('pullHotels', true);
	},
};

export const getters = {};
