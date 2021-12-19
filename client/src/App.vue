<template>
  <v-app class="app-main">
    <v-app-bar
      app
      color="primary"
      dark
    >
      <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="@/assets/TheWhoLogo.png"
          transition="scale-transition"
          width="40"
          @click="handleClick"
        />
        <h1>The-Who</h1>
      </div>
      <v-spacer></v-spacer>
        <h6>ALPHA</h6>
    </v-app-bar>

    <v-main>
      <router-view/>
    </v-main>
  </v-app>
</template>

<script>

export default {
  name: 'App',
  methods: {
    handleClick() {
      this.$router.push({ path: '/' });
    },
  },
};
</script>

<style scoped>
.app-main{
    overflow-y: hidden;
}
</style>
