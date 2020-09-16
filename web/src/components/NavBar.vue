<template>
  <div v-on:mouseleave="typy = false; konstrukce = false; materialy = false;">
    <b-navbar toggleable="lg" type="light">
      <b-navbar-brand
        style="padding-left: 24px;padding-right: 80px;"
        href="/"
      >
        <b-row>
          <img
            src="../../assets/lignis-logo.png">
        </b-row>
      </b-navbar-brand>

      <b-navbar-toggle target="nav-collapse-1">
      </b-navbar-toggle>

      <b-collapse id="nav-collapse-1" is-nav>
        <b-navbar-nav>
          <b-nav-item to="/">Domů</b-nav-item>
          <b-nav-item to="/clanky">Aktuality</b-nav-item>
          <b-nav-item to="/prodejny">Prodejny</b-nav-item>
          <b-nav-item to="/kariera">Kariéra</b-nav-item>
          <b-nav-item to="/kontakt">Kontakt</b-nav-item>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <b-navbar toggleable="lg" type="light">
      <b-navbar-toggle target="nav-collapse-3">
        <b-button>Zobraz oddělení</b-button>
      </b-navbar-toggle>
      <b-collapse id="nav-collapse-3" is-nav>
        <b-navbar-nav>
          <b-nav-item v-on:mouseover="typy = true; konstrukce = false; materialy = false;"><span style="color: black;">Modely dveří</span></b-nav-item>
          <b-nav-item v-on:mouseover="typy = false; konstrukce = true; materialy = false;"><span style="color: black;">Konstrukce dveří</span></b-nav-item>
          <b-nav-item v-on:mouseover="typy = false; konstrukce = false; materialy = true;"><span style="color: black;">Materiály a doplňky</span></b-nav-item>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
    <b-navbar v-on:mouseleave="typy = false; konstrukce = false; materialy = false;">
      <b-nav v-if="konstrukce" tabs>
        <b-nav-item v-on:click="hide" to="/kategorie/konstrukce_dvere"><span style="color: black;">Interiérové dveří</span></b-nav-item>
        <b-nav-item v-on:click="hide" to="/kategorie/protipozarni"><span style="color: black;">Protipožární dveře</span></b-nav-item>
        <b-nav-item v-on:click="hide" to="/kategorie/bezpecnostni"><span style="color: black;">Bezpečnostní dveře</span></b-nav-item>
        <b-nav-item v-on:click="hide" to="/kategorie/akusticke"><span style="color: black;">Akustické dveře</span></b-nav-item>
        <b-nav-item v-on:click="hide" to="/kategorie/zarubne"><span style="color: black;">Konstrukce zárubní</span></b-nav-item>
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
    </b-navbar>
  </div>
</template>

<script>

export default {
  name: 'NavBar',
  components: {
  },
  data: () => ({
    konstrukce: false,
    materialy: false,
    typy: false
  }),
  methods: {
    hide: function () {
      this.typy = false;
      this.konstrukce = false;
      this.materialy = false;
    }
  }
}
</script>