<template>
  <v-card class="card" elevation="2" outlined>
      <lan-icon class="icon" v-if="isService"/>
      <folder-icon class="icon" v-if="!isTeam && !isService"/>
      <acount-group-icon class="icon" v-if="isTeam"/>
      <div class="team-user-info">
            <h2 class="title">{{getTitle}}</h2>
            <p>{{getOwnerText}}</p>
      </div>
      <div class="disciption-panel">
          {{getDescription}}
      </div>
      <div class="button-panel">
          <v-btn
          v-if="!isService"
          icon
          @click="handleClick"
          >
          <arrow-right-icon/>
          </v-btn>
      </div>
  </v-card>
</template>

<script>
import AcountGroupIcon from 'vue-material-design-icons/AccountGroup.vue';
import LanIcon from 'vue-material-design-icons/Lan.vue';
import FolderIcon from 'vue-material-design-icons/Folder.vue';
import ArrowRightIcon from 'vue-material-design-icons/ArrowRight.vue';

export default {
  name: 'ResultCard',
  components: {
    LanIcon,
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
    getTitle() {
      if (this.data.documentType === 1 || this.data.documentType === 2) {
        return this.data.title;
      }
      return this.data.name;
    },
    getOwnerText() {
      if (this.data.documentType === 3) {
        return `Manger: ${this.data.teamLeader?.name}`;
      }
      if (this.data.documentType === 4) {
        return `Owner: ${this.data.owner?.name}`;
      }
      return '';
    },
    getDescription() {
      if (this.data.description) {
        return this.data.description;
      }
      return '';
    },
    isTeam() {
      return this.data.documentType === 3;
    },
    isService() {
      return this.data.documentType === 4;
    },
  },
  methods: {
    handleClick() {
      if (this.isTeam) {
        this.$router.push({ path: `/resource/${this.data.id}` });
      } else {
        window.open(this.data.link);
      }
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
.disciption-panel{
    display: flex;
    flex-direction: row;
    flex-grow : 1;
    margin-right: 1%;
    height: 60%;
    display: flex;
    align-items: center;
}
</style>
