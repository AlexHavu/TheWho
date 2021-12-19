<template>
  <div class="search-page">
      <div class="search-area">
        <search-field @search="handleSearch"></search-field>
      </div>
      <div class="results-area">
        <div class="filter-area">
          <filter-results @filter="handleFilter"/>
        </div>
        <div class="results">
          <result-card v-for="result in searchResult" :key="result.id" :data="result"></result-card>
        </div>
      </div>
  </div>
</template>

<script>
import { mapActions } from 'vuex';
import SearchField from '@/components/Search/SearchField.vue';
import ResultCard from '@/components/Search/ResultCard.vue';
import FilterResults from '@/components/Search/Filter.vue';
import search from '@/assets/search.json';

export default {
  name: 'SearchPage',
  components: {
    SearchField,
    ResultCard,
    FilterResults,
  },
  data() {
    return {
      allSearchResult: search,
      searchResult: [],
      documentTypes: [],
    };
  },
  methods: {
    handleFilter(type) {
      this.searchResult = this.allSearchResult.filter((x) => x.DocumentType === type);
    },
    async handleSearch({ searchValue }) {
      this.searchResult = await this.search(searchValue);
    },
    ...mapActions({
      search: 'search',
    }),
  },
};
</script>

<style scoped>
  .search-page{
    height: 100vh;
    width: 100%;
    display: flex;
    justify-content: space-around;
    flex-direction: column;
    overflow-y: hidden;
  }
  .search-area{
    display: flex;
    width: 100%;
    height: 15%;
    align-items: center;
    justify-content: center;
    border-bottom: 1px solid rgba(220, 220, 220, 0.5);
  }
  .results-area{
    width: 100%;
    height: 85%;
    display: flex;
  }
  .filter-area{
    width: 20%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    border-right: 1px solid rgba(220, 220, 220, 0.5);

  }
  .results{
    width: 80%;
    height: 100%;
    background-color: rgb(246, 246, 246);
    display: flex;
    flex-direction: column;
    align-items: center;
    overflow-y: auto;
  }
</style>
