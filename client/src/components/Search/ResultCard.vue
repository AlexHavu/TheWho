<template>
  <v-card class="card" elevation="2" outlined>
      <acount-icon class="icon" v-if="false"/>
      <folder-icon class="icon" v-if="false"/>
      <acount-group-icon class="icon" v-if="true"/>
      <div class="team-user-info">
            <h2 class="title">Sharon Geller</h2>
            <p>{{getBottomText}}</p>
      </div>
      <div class="button-panel">
          <v-btn
          icon
          >
          <arrow-right-icon/>
          </v-btn>
      </div>
  </v-card>
</template>

<script>
import AcountGroupIcon from 'vue-material-design-icons/AccountGroup.vue';
import AcountIcon from 'vue-material-design-icons/Account.vue';
import FolderIcon from 'vue-material-design-icons/Folder.vue';
import ArrowRightIcon from 'vue-material-design-icons/ArrowRight.vue';

export default {
  name: 'ResultCard',
  components: {
    AcountIcon,
    FolderIcon,
    AcountGroupIcon,
    ArrowRightIcon,
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
    handleClick() {
      this.$emit('search', { searchValue: this.searchValue });
    },
  },
};
</script>

<style scoped>
.card{
    display: flex;
    align-items: center;
    width: 80%;
    height: 15%;
    min-width:80%;
    min-height:15%;
    background-color: white;
    margin-top: 1%;
}
.icon{
    margin-left: 1%;
}
.team-user-info{
    margin-left: 10%;
    display: flex;
    flex-direction: column;
    margin-left: 1%;
    width: 30%;
    height: 100%;
}
.title{
    margin-top:10%;
    color: rgb(69, 94, 163);
}
.button-panel{
    display: flex;
    flex-direction: row-reverse;
    flex-grow : 1;
    margin-right: 1%;
}
</style>
