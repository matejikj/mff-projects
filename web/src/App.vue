<template>
  <div id="app">
    <nav-bar></nav-bar>
    <router-view/>
    <custom-footer></custom-footer>
    <footer-logo></footer-logo>
  </div>
</template>

<script>
import CustomFooter from './components/CustomFooter.vue'
import FooterLogo from './components/FooterLogo.vue'
import NavBar from './components/NavBar.vue'

export default {
  name: 'app',
  metaInfo: {
    title: 'Default App Title',
    titleTemplate: '%s | vue-meta Example App',
    htmlAttrs: {
      reptilian: 'gator'
    },
    headAttrs: {
      nest: 'eggs'
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'description', content: 'gator' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' }
    ]
  },
  components: {
    CustomFooter,
    FooterLogo,
    NavBar,
  }
}
</script>

<style>
@import url('http://fonts.googleapis.com/css?family=Exo+2&subset=latin');

#app {
  font-family: 'Exo 2',monospace;
  font-size: 1.2em;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

h1, h2 {
  font-weight: normal;
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
