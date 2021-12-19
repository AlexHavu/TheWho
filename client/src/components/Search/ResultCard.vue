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
      if (this.data.DocumentType === 1) {
        return this.data.Title;
      }
      return this.data.Name;
    },
    getOwnerText() {
      if (this.data.DocumentType === 3) {
        return `Manger: ${this.data.TeamLeader.Name}`;
      }
      if (this.data.DocumentType === 4) {
        return `Owner: ${this.data.Owner.Name}`;
      }
      return '';
    },
    getDescription() {
      if (this.data.Description) {
        return this.data.Description;
      }
      return '';
    },
    isTeam() {
      return this.data.DocumentType === 3;
    },
    isService() {
      return this.data.DocumentType === 4;
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
