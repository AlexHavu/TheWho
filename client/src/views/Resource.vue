<template>
  <div class="hello">
    <team-members
    :teamName="resource.Name"
    :teamLeader="resource.TeamLeader.Name"
    :teamMembers="resource.TeamMembers"/>
    <quick-links :slack="resource.Slack" :jira="resource.Jira" :confluence="resource.Confluence"/>
    <services :services="resource.Services"/>
    <domains :domains="resource.Domains"/>
  </div>
</template>

<script>
import QuickLinks from '../components/Resource/QuickLinks.vue';
import TeamMembers from '../components/Resource/TeamMembers.vue';
import Services from '../components/Resource/Services.vue';
import Domains from '../components/Resource/Domains.vue';
import resourceJson from '@/assets/resource.json';

export default {
  components: {
    QuickLinks, TeamMembers, Services, Domains,
  },
  name: 'Resource',
  props: {
    msg: String,
  },
  data() {
    return {
      resource: resourceJson,
    };
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
