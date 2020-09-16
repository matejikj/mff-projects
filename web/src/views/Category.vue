<template>
  <b-container class="my-container" fluid>
    <b-carousel
      style="padding: 40px; height: 576px;"
      id="carousel-1"
      controls
      indicators
    >
      <b-carousel-slide v-for="item in data.images" v-bind:key="item">
        <template v-slot:img>
          <img
            style="height: 516px;"
            :src="getImgUrl(item)"
            alt="image slot"
          >
        </template>
      </b-carousel-slide>
    </b-carousel>
    <h1>{{ data.title }}</h1>
    <p style="text-align: left; font-size: 1.1em;">{{ data.desc }}</p>

    <h4 v-if="data.help.length !== 0" style="text-align: left;">Vysvětlivky</h4>
    <p style="text-align: left;" v-for="help in data.help" :key="help">
      <b>{{ help[0] }}</b>: {{ help[1] }}
    </p>
    
    <div>
      <b-row>
        <b-col v-for="(detail, index) in data.details" :key="index" xl="3" lg="6" md="12">
          <b-card v-on:click="clicked(index)" class="my-card">
            <b-card-body>
              <b-card-img class="my-image" :src="getImgUrl(data.details[index].image)" alt="Image" bottom></b-card-img>
              <b-card-title class="my-title">{{ data.details[index].title }}</b-card-title>
            </b-card-body>
          </b-card>
        </b-col>
      </b-row>
    </div>

  </b-container>
</template>

<script>
import { data as mainpages } from '../../data/mainpages'

export default {
  name: 'Detail',
  components: {
  },
  computed: {
    data: function () {
      return mainpages[this.$route.params.category]
    }
  },
  methods: {
    getImgUrl(pic) {
      return "../../data/mainpages/" + pic
    },
    clicked: function (key) {
      this.$router.push('/kategorie/' + this.$route.params.category + '/typ/' + key)
    }
  }
}
</script>

<style scoped>
.my-card {
  max-width: 450px;
  margin-top: 18px;
  height: 45vh;
  margin-bottom: 18px;
  cursor: pointer;
}

.my-image {
  height: 33vh;
}

.my-title {
  padding: 10px;
}

.my-container {
  padding: 0px;
}

@media screen and (min-width: 800px) {
  .my-container {
    padding: 100px;
  }
}
</style>
