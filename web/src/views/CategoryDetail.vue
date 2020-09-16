<template>
  <b-container fluid="xl">
    <div>
      <b-img style="margin: 20px; max-height: 576px;" :src="getImgUrl(data.image)"></b-img>
    </div>
    
    <h1>{{ data.title }}</h1>
    
    <div v-for="(paragraph, index) in data.paragraphs" v-bind:key="index">
      <h3>{{ paragraph[0] }}</h3>
      <p>{{ paragraph[1] }}</p>
    </div>
    
    <b-carousel
      style="padding: 40px; height: 576px;"
      id="carousel-1"
      controls
      indicators
    >
      <b-carousel-slide v-for="item in data.plans" v-bind:key="item">
        <template v-slot:img>
          <img
            style="padding: 40px; height: 516px;"
            :src="getImgUrl(item)"
            alt="image slot"
          >
        </template>
      </b-carousel-slide>
    </b-carousel>
  </b-container>
</template>

<script>
import { data as mainpages } from '../../data/mainpages'

export default {
  name: 'Mainpage',
  components: {
  },
  computed: {
    data: function () {
      return mainpages[this.$route.params.category].details[this.$route.params.type]
    }
  },
  methods: {
    getImgUrl(pic) {
      return "/data/mainpages/" + pic
    }
  }
}
</script>

<style scoped>
.my-card {
  margin-top: 18px;
  height: 45vh;
  margin-bottom: 18px;
}

.my-image {
  height: 35vh;
}

.my-title {
  padding: 10px;
}
</style>
