<template>
  <b-container fluid="xl">
    <b-button v-if="!newVisible" @click="showAdding" style="margin: 10px;">Přidat článek</b-button>
    <div v-if="newVisible">
      <b-row>
        <p>Titulek:</p>
        <b-form-input
          id="input-title"
          v-model="newTitle"
          required
          placeholder="Enter name"
        ></b-form-input>
      </b-row>
        <b-row>
          <p>Text:</p>
          <b-form-textarea
          id="input-text"
          v-model="newText"
          placeholder="Enter something..."
          rows="3"
          max-rows="6"
        ></b-form-textarea>
      </b-row>
      <b-row>
        <b-button @click="addArticle" style="margin: 10px;">Přidej článek</b-button>
        <b-button @click="newVisible = false" style="margin: 10px;">Zruš</b-button>
      </b-row>
    </div>
    <div v-if="selected[0] !== undefined">
      <b-row>
        <p>Titulek:</p>
        <b-form-input
          id="input-title"
          v-model="selected[0].title"
          required
          placeholder="Enter name"
        ></b-form-input>
      </b-row>
        <b-row>
          <p>Text:</p>
          <b-form-textarea
          id="input-text"
          v-model="selected[0].text"
          placeholder="Enter something..."
          rows="3"
          max-rows="6"
        ></b-form-textarea>
      </b-row>
      <b-row>
        <b-button @click="updateArticle" style="margin: 10px;">Ulož</b-button>
        <b-button @click="deleteArticle" style="margin: 10px;">Smaž článek</b-button>
        <b-button @click="clearSelected" style="margin: 10px;">Zruš výběr</b-button>
      </b-row>
    </div>
    <b-row>
      <b-table
        ref="selectableTable"
        selectable
        :select-mode="selectMode"
        :items="articles"
        :fields="fields"
        @row-selected="onRowSelected"
        responsive="sm"
      >
        <!-- Example scoped slot for select state illustrative purposes -->
        <template v-slot:cell(selected)="{ rowSelected }">
          <template v-if="rowSelected">
            <span aria-hidden="true">&check;</span>
            <span class="sr-only">Selected</span>
          </template>
          <template v-else>
            <span aria-hidden="true">&nbsp;</span>
            <span class="sr-only">Not selected</span>
          </template>
        </template>
      </b-table>
    </b-row>
  </b-container>
</template>

<script>
import { Article } from '../Models/Article'
import firebase from '../firebaseConfig'
const db = firebase.firestore()

export default {
  name: 'ArticlesAdmin',
  components: {
  },
  data: () => ({
    articles: [],
    fields: ['date', 'title', 'text'],
    selectMode: 'single',
    selected: [],
    textBackup: '',
    titleBackup: '',
    newText: '',
    newTitle: '',
    newVisible: false
  }),
  computed: {
  },
  mounted () {
    this.loadArticles()
  },
  methods: {
    onRowSelected (items) {
      this.selected = items
      this.textBackup = this.selected[0].text
      this.titleBackup = this.selected[0].title
    },
    clearSelected () {
      this.selected[0].text = this.textBackup
      this.selected[0].title = this.titleBackup
      this.$refs.selectableTable.clearSelected()
    },
    updateArticle: function () {
      db.collection('articles')
        .doc(this.selected[0].id)
        .update({
          title: this.selected[0].title,
          text: this.selected[0].text
        })
        .then(() => {
          console.log('Document successfully updated')
        })
        .catch((error) => {
          console.error('Error updating document: ', error)
        })
      this.selected = []
    },
    deleteArticle: function () {
      db.collection('articles')
        .doc(this.selected[0].id)
        .delete()
        .then(() => {
          console.log('Document successfully deleted!')
        })
        .catch((error) => {
          console.error('Error removing document: ', error)
        })
      this.articles = this.articles.filter(x => x.id !== this.selected[0].id)
      this.selected = []
    },
    addArticle: function () {
      var today = new Date();
      var dd = String(today.getDate()).padStart(2, '0');
      var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
      var yyyy = today.getFullYear();

      today = dd + '.' + mm + '.' + yyyy;
      db.collection('articles')
        .add({ title: this.newTitle, text: this.newText, date: today })
        .then(() => {
          console.log('Document successfully written!')
        })
        .catch((error) => {
          console.error('Error writing document: ', error)
        })
      this.articles = this.articles.push({ title: this.newTitle, text: this.newText })
      this.selected = []
      this.newText = ''
      this.newTitle = ''
      this.newVisible = false
    },
    loadArticles: function () {
      this.articles = []
      db.collection('articles')
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
    showAdding: function () {
      this.newVisible = true
    }
  }
}
</script>

<style scoped>
.border1 {
  border: none;
}
</style>
