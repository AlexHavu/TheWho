<template>
  <v-card class="services" elevation="1" outlined>
    <div class="services-header">{{ header }}</div>
    <div class="services-list">
      <div v-for="serviceItem in services" :key="serviceItem" class="service">
        <img class="service-icon" src="@/assets/service.png" />
        <div>{{ serviceItem }}</div>
      </div>
    </div>
  </v-card>
</template>

<script>
export default {
  name: 'Services',
  props: {
    services: Array,
    default() {
      return [];
    },
  },
  data() {
    return {
      header: 'Services:',
    };
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.services {
  margin-bottom: 50px;
  margin-left: 50px;
  margin-right: 50px;
  padding-bottom: 50px;
  padding-left: 15px;
  padding-right: 15px;
}

.services-header {
  text-align: left;
  font-weight:bold;
  font-size: 20px;
    margin-bottom: 15px;
}

.services-list {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr;
  gap: 15px 2em;
  align-items: left;
  flex-wrap: wrap;
}

.service {
    display: flex;
    flex-direction: row;
    align-items: left;
    font-weight:bold;
    font-size: 14px;
    margin-right: 50px;
    color: #1976d2;
}

.solid {
  border-top: 1px solid rgb(226, 224, 224);
  margin-bottom: 10px;
}

.service-icon {
  width: 15px;
  height: 15px;
  margin-right: 5px;
}
</style>
