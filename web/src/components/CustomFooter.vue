<template>
  <b-container style="background: gray; color: white; padding: 20px" fluid>
    <b-container fluid="xl">
      <b-row fluid="xl">
        <b-col lg="4" sm="12">
          <h1>VÝROBKY</h1>
          <a>Konstrukce interiérových dveří</a></br>
          <a>Protipožární dveře</a></br>
          <a>Bezpečnostní konstrukce</a></br>
          <a>Akustické konstrukce</a></br>
          <a>Konstrukce zárubní</a></br>
          <a>Interiérové a požární stěny</a></br>
          <a>Modelové řady interiérových dveří</a></br>
          <a>Materiály a doplňky pro interiérové dveře</a>
        </b-col>
        <b-col lg="4" sm="12">
          <h1>O FIRMĚ</h1>
          <a>Domů</a></br>
          <a>Aktuality</a></br>
          <a>Prodejny</a></br>
          <a>Kontakt</a>
        </b-col>
        <b-col lg="4" sm="12">
          <h1>DOKUMENTY KE STAŽENÍ</h1>
          <a href="data/documents/lignis katalog 2010_160dpi.pdf">Katalog</a></br>
          <a href="data/documents/VOP_LIGNIS_15052020.pdf">Všeobecné obchodní podmínky</a>
          <a href="data/documents/RR_LIGNIS_15052020.pdf">Reklamační řád</a>
          <h1>GDPR</h1>
          <a href="data/documents/Informace-pro-zakazniky_LIGNIS.pdf">Informace pro zákazníky</a>
        </b-col>
      </b-row>
    </b-container>
  </b-container>
</template>

<script>

export default {
  name: 'CustomFooter',
  components: {
  }
}
</script>

<style scoped>
.border1 {
  border: none;
}

a {
  color: white;
}
</style>
