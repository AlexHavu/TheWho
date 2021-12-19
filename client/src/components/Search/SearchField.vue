<template>
  <div class="search-field">
      <v-text-field class="field" v-model="searchValue" outlined></v-text-field>
      <v-btn
      class="btn"
      :disabled="isButtonDisabled"
      @click="this.handleClick"
      outlined
      >
      Search
      </v-btn>
  </div>
</template>

<script>
export default {
  name: 'SearchField',
  props: {
    msg: String,
  },
  data() {
    return {
      searchValue: '',
    };
  },
  computed: {
    isButtonDisabled() {
      return this.searchValue.length <= 0;
    },
  },
  methods: {
    handleClick() {
      this.$emit('search', { searchValue: this.searchValue });
    },
  },
};
</script>

<style scoped>
.search-field{
    display: flex;
    width: 30%;
}
.field {
  width: 70%;
  margin-top: 50%;
}
.btn{
  width: 30%;
  margin-top: 2%;
  margin-inline-start: 2%;
}
</style>
