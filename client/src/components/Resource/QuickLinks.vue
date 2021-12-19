<template>
  <v-card class="links" elevation="1" outlined>
    <div class="links-header">{{ header }}</div>
    <div class="links-list">
      <div class="link">
        <img class="link-icon" src="@/assets/confluence.png" />
        <a v-bind:href="confluence" target="_blank">{{ confluenceText }}</a>
      </div>
      <div class="link">
        <img class="link-icon" src="@/assets/slack.png" />
        <a v-bind:href="slack" target="_blank">{{ slackText }}</a>
      </div>
      <div class="link">
        <img class="link-icon" src="@/assets/jira.png" />
        <a v-bind:href="jira" target="_blank">{{ jiraText }}</a>
      </div>
    </div>
  </v-card>
</template>

<script>
export default {
  name: 'QuickLinks',
  props: {
    confluence: String,
    slack: String,
    jira: String,
  },
  data() {
    return {
      header: 'Quick Links:',
      jiraText: 'Jira board',
      slackText: 'Slack channel',
      confluenceText: 'Confluence page',
    };
  },
};
</script>

<style scoped>
.links {
  margin-bottom: 50px;
  margin-left: 50px;
  margin-right: 50px;
  padding-bottom: 50px;
  padding-left: 15px;
  padding-right: 15px;
}

.links-header {
  text-align: left;
  font-weight:bold;
  font-size: 20px;
  margin-bottom: 15px;
}

.links-list {
  display: flex;
  flex-direction: row;
  padding: 0;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr;
  gap: 15px 2em;
}

.link {
  margin-right: 15px;
  padding: 3px 15px 3px 15px;
  border: 1px solid #1976d2;
  border-radius: 5px;
  font-weight:bold;
  display: flex;
  align-items: center;
  justify-content: center;
}

a {
    text-decoration: none;
    font-size: 14px;
}

.solid {
  border-top: 1px solid rgb(226, 224, 224);
  margin-bottom: 10px;
}

.link-icon {
  width: 15px;
  height: 15px;
  margin-right: 5px;
}
</style>
