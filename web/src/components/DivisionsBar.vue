<template>
  <div v-on:mouseleave="typy = false; konstrukce = false; materialy = false;">
    <b-nav class="div-bar" tabs>
      <b-nav-item class="div-item" v-on:mouseover="typy = true; konstrukce = false; materialy = false;"><span style="color: black;">Modely dveří</span></b-nav-item>
      <b-nav-item class="div-item" v-on:mouseover="typy = false; konstrukce = true; materialy = false;"><span style="color: black;">Konstrukce dveří</span></b-nav-item>
      <b-nav-item class="div-item" v-on:mouseover="typy = false; konstrukce = false; materialy = true;"><span style="color: black;">Materiály a doplňky</span></b-nav-item>
    </b-nav>
    <b-nav v-if="konstrukce" tabs>
      <b-nav-item to="/kategorie/konstrukce_dvere"><span style="color: black;">Interiérové dveří</span></b-nav-item>
      <b-nav-item to="/kategorie/protipozarni"><span style="color: black;">Protipožární dveře</span></b-nav-item>
      <b-nav-item to="/kategorie/protipozarni"><span style="color: black;">Bezpečnostní dveře</span></b-nav-item>
      <b-nav-item to="/kategorie/akusticke"><span style="color: black;">Akustické dveře</span></b-nav-item>
      <b-nav-item to="/kategorie/protipozarni"><span style="color: black;">Konstrukce zárubní</span></b-nav-item>
    </b-nav>
    <b-nav v-if="materialy" tabs>
      <b-nav-item><span style="color: black;">Interiérové dveří</span></b-nav-item>
      <b-nav-item><span style="color: black;">Interiérové dveří</span></b-nav-item>
      <b-nav-item><span style="color: black;">Interiérové dveří</span></b-nav-item>
      <b-nav-item><span style="color: black;">Interiérové dveří</span></b-nav-item>
      <b-nav-item><span style="color: black;">Interiérové dveří</span></b-nav-item>
    </b-nav>
    <b-nav v-if="typy" tabs>
      <b-nav-item><span style="color: black;">Benátky</span></b-nav-item>
      <b-nav-item><span style="color: black;">Domašov</span></b-nav-item>
      <b-nav-item><span style="color: black;">Verony</span></b-nav-item>
      <b-nav-item><span style="color: black;">Riminy</span></b-nav-item>
      <b-nav-item><span style="color: black;">Palestro</span></b-nav-item>
      <b-nav-item><span style="color: black;">Insbruk</span></b-nav-item>
      <b-nav-item><span style="color: black;">Mistřín</span></b-nav-item>
      <b-nav-item><span style="color: black;">Měřín</span></b-nav-item>
      <b-nav-item><span style="color: black;">Lexus</span></b-nav-item>
      <b-nav-item><span style="color: black;">Hladké a Lux</span></b-nav-item>
      <b-nav-item><span style="color: black;">Linie</span></b-nav-item>
      <b-nav-item><span style="color: black;">Psaníčka</span></b-nav-item>
      <b-nav-item><span style="color: black;">Kvadrát</span></b-nav-item>
      <b-nav-item><span style="color: black;">Majetín</span></b-nav-item>
      <b-nav-item><span style="color: black;">Majetín A</span></b-nav-item>
      <b-nav-item><span style="color: black;">Margaret</span></b-nav-item>
      <b-nav-item><span style="color: black;">Sedlec</span></b-nav-item>
      <b-nav-item><span style="color: black;">Loket</span></b-nav-item>
      <b-nav-item><span style="color: black;">Lukov</span></b-nav-item>
      <b-nav-item><span style="color: black;">Brno</span></b-nav-item>
      <b-nav-item><span style="color: black;">Pikaso</span></b-nav-item>
    </b-nav>
  </div>
</template>

<script>

export default {
  name: 'Navigation',
  components: {
  },
  data: () => ({
    konstrukce: false,
    materialy: false,
    typy: false
  }),
}
</script>
