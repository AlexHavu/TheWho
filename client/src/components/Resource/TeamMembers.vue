<template>
  <div class="team">
    <div class="team-name">{{ teamName }}</div>
    <div class="team-members">
      <div class="member">
        <img class="member-icon" src="@/assets/leader.png" />
        <div class="leader">{{ teamLeader }}</div>
        <div class="leader-description">{{ teamLeaderDescription }}</div>
      </div>
      <div v-for="teamMember in teamMembers" :key="teamMember.link" class="member">
        <img class="member-icon" src="@/assets/person.png" />
        <div>{{ teamMember.Name }}</div>
        <div class="leader-description">{{ teamMember.Title }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TeamMembers',
  props: {
    teamName: String,
    teamMembers: Array,
    default() {
      return [];
    },
    teamLeader: String,
  },
  data() {
    return {
      teamLeaderDescription: 'Team Leader',
    };
  },
};
</script>

<style scoped>
.team {
   margin-bottom: 100px;
}

.team-name {
  text-align: center;
  font-weight:bold;
  font-size: 30px;
  margin-bottom: 40px;
  margin-top: 40px;
}

.team-members {
  display: flex;
  flex-direction: row;
  justify-content: center;
  width: 100%;
}

.member {
  align-items: center;
  display: flex;
  flex-direction: column;
  width: 120px;
  height: 70px;
}

.member-icon {
  width: 60px;
  height: 60px;
}

.leader {
    font-weight:bold;
}

.leader-description {
  font-size: 10px;
  color: gray;
}

</style>
