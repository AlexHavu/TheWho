<template>
  <div class="hello">
    <team-members
    :teamName="'Contigos'"
    :teamLeader="'Yehiel Dahan'"
    :teamMembers="[{ name: 'Eli Broide'}, { name: 'Eylon Basirtman'},
    { name: 'Mor Cohen'}, { name: 'Mila Brooks'}, { name: 'Sharon Geller'}]"/>
    <quick-links :slack="'https://tipalti.slack.com/'" :jira="'https://tipalti.slack.com/'" :confluence="'https://tipalti.slack.com/'"/>
    <services :services="[{ title: 'Approval rules' },
    { title: 'Bills' },
    { title: 'Contigos' },
    { title: 'Approval rules 2' }, { title: 'Bills 2' }, { title: 'Contigos 2' },
    { title: 'Approval rules 3' }, { title: 'Bills 3' }, { title: 'Contigos 3' }]"/>
    <domains :domains="[{ title: 'Approval domain' }, { title: 'food domain' }]"/>
  </div>
</template>

<script>
import QuickLinks from '../components/Resource/QuickLinks.vue';
import TeamMembers from '../components/Resource/TeamMembers.vue';
import Services from '../components/Resource/Services.vue';
import Domains from '../components/Resource/Domains.vue';

export default {
  components: {
    QuickLinks, TeamMembers, Services, Domains,
  },
  name: 'Resource',
  props: {
    msg: String,
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
