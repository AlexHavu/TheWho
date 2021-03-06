import Vue from 'vue';
import VueRouter from 'vue-router';
import SearchPage from '../views/SearchPage.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'SearchPage',
    component: SearchPage,
  },
  {
    path: '/resource/:id',
    name: 'Resource',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/Resource.vue'),
    props: true,
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
