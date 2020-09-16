<template>
  <div>
    <b-carousel
      id="carousel-1"
      controls
      indicators
    >
      <b-carousel-slide>
        <template v-slot:img>
          <img
            style="height: calc(100vh - 102px); width: 100%; min-width: 1024px;"
            src="../../assets/studio.jpg"
            alt="image slot"
          >
        </template>
        <div class="karta">
          <h1>Realizace</h1>
          <h4>Naše realizace mluví za vše.</h4>
        </div>
      </b-carousel-slide>

      <!-- Note the classes .d-block and .img-fluid to prevent browser default image alignment -->
      <b-carousel-slide>
        <template v-slot:img>
          <img
            style="height: calc(100vh - 102px); width: 100%; min-width: 1024px;"
            src="../../assets/doors.jpg"
            alt="image slot"
          >
        </template>
        <div class="karta">
          <h1>Showroom plný novinek</h1>
          <h4>Najdete nás v Praze, Brně a Zlíně</h4>
        </div>
      </b-carousel-slide>

      <b-carousel-slide>
        <template v-slot:img>
          <img
            style="height: calc(100vh - 102px); width: 100%; min-width: 1024px;"
            src="../../assets/zavod.jpg"
            alt="image slot"
          >
        </template>
        <div class="karta">
          <h1 style="color: black;">Spolehlivá česká značka</h1>
          <h4 style="color: black;">JSME S VÁMI, JSME TU PRO VÁS</h4>
        </div>
      </b-carousel-slide>
    </b-carousel>
    <b-row style="margin-right: 0px; margin-left: 0px;">
      <b-col lg="1" md="12"></b-col>
      <b-col lg="10" md="12">
        <b-row>
          <b-col cols="2">
          </b-col>
          <b-col cols="8">
            <p>Firma LIGNIS s.r.o. vznikla v roce 1990 a  jejím hlavním výrobním programem byla výroba eurooken, zimních zahrad, exteriérových a interiérových dveří, včetně výroby protipožárních dveří a uzávěrů.</p>
          </b-col>
          <b-col cols="2">
          </b-col>
        </b-row>
        <b-row>
          <b-col cols="2">
          </b-col>
          <b-col cols="8">
            <p>Od roku 1998 se specializujeme na výrobu interiérových a protipožárních dveří a dveří se zvukovou neprůzvučností 51dB a od roku 2007, po provedení úspěšných protipožárních a cyklických zkoušek, vyrábíme i dveře ocelové a hliníkové.</p>
          </b-col>
          <b-col cols="2">
          </b-col>
        </b-row>
        <b-row>
          <b-col cols="2">
          </b-col>
          <b-col cols="8">
            <p>LIGNIS s.r.o. je mladá, dynamicky se rozvíjející firma, vybavená moderními technologiemi. Díky kvalitním strojům a odborné zdatnosti našich pracovníků dosahujeme dokonalého opracování materiálů i povrchových úprav a držíme tak krok se světovými standardy kvality.</p>
          </b-col>
          <b-col cols="2">
          </b-col>
        </b-row>
        <b-row>
          <b-col cols="2">
          </b-col>
          <b-col cols="8">
            <p>Klademe velký důraz na kvalitu naší práce, od výběru materiálu přes jeho zpracování, až po detailní kompletační práce. Máme vlastní konstrukční a vývojové oddělení, ve kterém připravujeme návrhy a tvary nových typů dveří podle požadavků zákazníka. Používáme nejkvalitnější dostupné materiály a technologické postupy. I proto můžeme na většinu modelů naší firmy poskytnout záruku 5 let !</p>
          </b-col>
          <b-col cols="2">
          </b-col>
        </b-row>
        <b-row>
          <b-col cols="2">
          </b-col>
          <b-col cols="8">
            <p>V LIGNIS s.r.o. spoléháme především na odbornost a profesionalitu svých pracovníků. Klíč k prosperitě spatřujeme v trvalém rozvoji firmy. Prosazujeme se kvalitou práce, rychlostí, pružností a uplatňováním prograsivních technologií. Klademe si za cíl být vždy v čele mezi dodavateli interiérových a protipožárních dveří.</p>
          </b-col>
          <b-col cols="2">
          </b-col>
        </b-row>
        <b-row>
          <b-col cols="8">
          </b-col>
          <b-col cols="2">
            <p>Miroslav Piňďák</p>
            <b-img center src="/assets/podpis.jpg"></b-img>
          </b-col>
          <b-col cols="2">
          </b-col>
        </b-row>
      </b-col>
      <b-col lg="1" md="12"></b-col>
    </b-row>
    <p class="carousel-control-next-icon"></p>
  </div>
</template>

<script>
export default {
  name: 'Home',
  components: {
  }
}
</script>

<style scoped>

.carousel-control-next-icon {
  width: 40px;
}

</style>
