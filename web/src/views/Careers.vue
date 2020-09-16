<template>
  <b-container fluid="xl">
    <div>
      <b-row>
        <b-col v-for="(article, index) in articles" :key="index" xl="12" lg="3" md="4" sm="12">
          <b-card style="margin: 20px; padding: 20px;" v-on:click="clicked(index)" class="my-card">
            <b-card-body>
              <b-card-title>{{ articles[index].title }}</b-card-title>
              <b-card-sub-title class="mb-2">{{ articles[index].date }}</b-card-sub-title>
              <b-card-text>{{ articles[index].text }}</b-card-text>
            </b-card-body>
          </b-card>
        </b-col>
      </b-row>
    </div>
  </b-container>
</template>

<script>
import { Article } from '../Models/Article'
import firebase from '../firebaseConfig'
const db = firebase.firestore()

export default {
  name: 'Articles',
  components: {
  },
  data: () => ({
    articles: []
  }),
  computed: {
  },
  mounted () {
    this.articles = []
    db.collection('career')
      .get()
      .then((querySnapshot) => {
        querySnapshot.forEach((doc) => {
          const newUser = {
            id: doc.id,
            title: doc.data().title,
            text: doc.data().text,
            date: doc.data().date
          }
          this.articles.push(newUser)
        })
      })
      .catch((error) => {
        console.log('Error getting documents: ', error)
      })
  },
  methods: {
    clicked: function (key) {
      this.$router.push('/pracovni-misto/' + key)
    }
  }
}
</script>

<style scoped>
.border1 {
  border: none;
}
</style>
