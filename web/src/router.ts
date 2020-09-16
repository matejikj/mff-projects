import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Home from './views/Home.vue'
import About from './views/About.vue'
import Articles from './views/Articles.vue'
import ArticlesAdmin from './views/ArticlesAdmin.vue'
import Shop from './views/Shop.vue'
import Careers from './views/Careers.vue'
import Contact from './views/Contact.vue'
import Category from './views/Category.vue'
import CategoryDetail from './views/CategoryDetail.vue'
import Model from './views/Model.vue'
import ModelDetail from './views/ModelDetail.vue'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/onas',
    name: 'About',
    component: About
  },
  {
    path: '/clanky',
    name: 'Articles',
    component: Articles
  },
  {
    path: '/clanky-administrace',
    name: 'Articles',
    component: ArticlesAdmin
  },
  {
    path: '/prodejny',
    name: 'Shop',
    component: Shop
  },
  {
    path: '/kariera',
    name: 'Careers',
    component: Careers
  },
  {
    path: '/kontakt',
    name: 'Kontakt',
    component: Contact
  },
  {
    path: '/kategorie/:category',
    name: 'Kategorie',
    component: Category
  },
  {
    path: '/kategorie/:category/typ/:type',
    name: 'Typ',
    component: CategoryDetail
  },
  {
    path: '/model/:model/typ/:typ',
    name: 'Model',
    component: Model
  },
  {
    path: '/model/:model/typ/:typ',
    name: 'Model',
    component: ModelDetail
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router
