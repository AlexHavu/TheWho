<template>
  <div class="filter-selection">
      <h1 class="title">Filter By</h1>
      <v-btn class="card" elevation="2" outlined @click="handleClick(3)">
        <img class="icon" src="@/assets/team.png">Team</v-btn>
        <v-btn class="card" elevation="2" outlined @click="handleClick(4)">
        <img class="icon" src="@/assets/service.png">Service</v-btn>
        <v-btn class="card" elevation="2" outlined @click="handleClick(2)">
        <img class="icon" src="@/assets/jira.png">Jira</v-btn>
        <v-btn class="card" elevation="2" outlined @click="handleClick(1)">
        <img class="icon" src="@/assets/confluence.png">Confluence</v-btn>
        <v-btn class="card" elevation="2" outlined @click="handleClick(1)">
        <img class="icon" src="@/assets/kibana.png">Kibana</v-btn>
        <v-btn class="card" elevation="2" outlined @click="handleClick(1)">
        <img class="icon" src="@/assets/grafana.png">Grafana</v-btn>

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
    handleClick(selectedFilter) {
      this.$emit('filter', selectedFilter);
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
.icon{
  width: 25px;
  height: 25px;
  margin-right: 5px;
}
</style>
