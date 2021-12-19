<template>
  <v-card class="domains" elevation="1" outlined>
    <div class="domains-header">{{ header }}</div>
    <div class="domains-list">
      <div v-for="domainsItem in domains" :key="domainsItem" class="domain">
        <img class="domain-icon" src="@/assets/domain.png" />
        <div>{{ domainsItem }}</div>
      </div>
    </div>
  </v-card >
</template>

<script>
export default {
  name: 'Domains',
  props: {
    domains: Array,
    default() {
      return [];
    },
  },
  data() {
    return {
      header: 'Domains:',
    };
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.domains {
  margin-bottom: 50px;
  margin-left: 50px;
  margin-right: 50px;
  padding-bottom: 50px;
  padding-left: 15px;
  padding-right: 15px;
}

.domains-header {
  text-align: left;
  font-weight:bold;
  font-size: 20px;
    margin-bottom: 15px;
}

.domains-list {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr;
  gap: 15px 2em;
  align-items: left;
  flex-wrap: wrap;
}

.domain {
    display: flex;
    flex-direction: row;
    align-items: left;
    font-weight:bold;
    font-size: 14px;
    margin-bottom: 10px;
    margin-right: 30px;
    color: #1976d2;
}

.solid {
  border-top: 1px solid rgb(226, 224, 224);
  margin-bottom: 10px;
}

.domain-icon {
  width: 20px;
  height: 20px;
  margin-right: 3px;
  margin-top: -2px;
}
</style>
