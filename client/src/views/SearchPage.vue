<template>
  <div class="search-page">
      <div class="search-area">
        <search-field></search-field>
      </div>
      <div class="results-area">
        <div class="filter-area"></div>
        <div class="results">
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
          <result-card></result-card>
        </div>
      </div>
  </div>
</template>

<script>
import SearchField from '@/components/Search/SearchField.vue';
import ResultCard from '@/components/Search/ResultCard.vue';

export default {
  name: 'SearchPage',
  components: {
    SearchField,
    ResultCard,
  },
};
</script>
    ResultCard

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
  }
  .results-area{
    width: 100%;
    height: 85%;
    background-color: green;
    display: flex;
  }
  .filter-area{
    width: 20%;
    height: 100%;
    background-color: blue;
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
