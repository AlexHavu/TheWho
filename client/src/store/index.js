/* eslint-disable consistent-return */
import Vue from 'vue';
import Vuex from 'vuex';
// import axios from 'axios';
// eslint-disable-next-line import/no-unresolved
import searchJson from '@/assets/search.json';

const SET_SERACH_RESULT = 'SET_SERACH_RESULT';

Vue.use(Vuex);

export default new Vuex.Store({
  state() {
    return {
      searchResult: [],
    };
  },
  mutations: {
    [SET_SERACH_RESULT](state, payload) {
      state.searchResult = payload;
    },
  },
  actions: {
    async search({ commit }, searchWord) {
      // const apiPayload = await axios.get(`localhost/${searchWord}`, {
      // cache: false,
      //  dataFetch: true,
      //  headers: { 'X-Requested-With': 'XMLHttpRequest' },
      // });
      console.log(searchWord);
      const apiPayload = searchJson;
      commit(SET_SERACH_RESULT, apiPayload);
      return apiPayload;
    },
    dataById(state, id) {
      return state.state.searchResult.filter((x) => x.Id.toString() === id)[0];
    },
  },
  modules: {
  },
});
