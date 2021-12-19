<template>
  <div class="filter-selection">
      <h1 class="title">Filter By</h1>
      <v-btn class="card" elevation="2" outlined @click="handleClick({})">test(1)</v-btn>
      <v-btn class="card" elevation="2" outlined>test</v-btn>
      <v-btn class="card" elevation="2" outlined>test</v-btn>

  </div>
</template>

<script>

export default {
  name: 'ResultCard',
  components: {
  },
  props: {
    data: Object,
  },
  data() {
    return {
      searchValue: '',
    };
  },
  computed: {
    getBottomText() {
      return 'Manger: placeholder';
    },
  },
  methods: {
    handleClick(filterBy) {
      this.$emit('filter', { filterBy });
    },
  },
};
</script>

<style scoped>
.filter-selection{
    height: 100%;
    width: 80%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start;
}
.title{
    color: rgb(69, 94, 163);
}
.card{
    width: 60%;
    height: 5%;
    margin-top: 2%;
}
</style>
