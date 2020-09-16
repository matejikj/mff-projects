using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Helper
{
    public static class OrderCreator
    {
        public static Order createOrder(int department)
        {
            Order order = new Order();

            order.Cislo = "";
            order.Ulice = "";
            order.Mesto = "";
            order.PSC = "";

            order.Adresa = order.Ulice + " " + order.Cislo + ", " + order.Mesto;

            order.Department = department;

            order.VyrobniNr = "";
            order.DocUrl = "http://www.lignis.cz";
            order.ArchivedDate = null;
            order.IsInProccess = false;
            order.IsDeleted = false;

            order.Priprava = "";
            order.Oblozky = "";
            order.Stredove = "";
            order.Ramecek = "";
            order.Klapacka = "";
            order.Posuv = "";
            order.Dvere = "";
            order.Dokonceni = "";
            order.Kompletace = "";
            order.Sklad = "";

            order.BrouseniDyhaDokonceni = "";
            order.BrouseniZakladDokonceni = "";
            order.CentrumDvere = "";
            order.CentrumStredove = "";
            order.CncDvere = "";
            order.CncOblozky = "";
            order.Dokumentace = "";
            order.DorazovaPosuv = "";
            order.DvereKompletace = "";
            order.FormatkaDvere = "";
            order.FrezovaniStredove = "";
            order.GarnyzPosuv = "";
            order.HranolPosuv = "";
            order.InDoca = "";
            order.KorpusyDvere = "";
            order.KovaniSklad = "";
            order.LisDvere = "";
            order.LisStredove = "";
            order.ObalovaniKlapacka = "";
            order.ObalovaniRamecek = "";
            order.ObalovatOblozky = "";
            order.OlepovackaDvere = "";
            order.OlepovaniStredove = "";
            order.PgmDvere = "";
            order.RezaniKlapacka = "";
            order.RezaniRamecek = "";
            order.RezatOblozky = "";
            order.SesazenkyDvere = "";
            order.SkloSklad = "";
            order.TypRamecek = "";
            order.VrchDokonceni = "";
            order.ZakladDokonceni = "";
            order.ZarubenKompletace = "";

            order.PripravaHli = "";
            order.DokumentPripravaHli = "";
            order.IndocaPripravaHli = "";
            order.NarezHli = "";
            order.ProfilNarezHli = "";
            order.ListyNarezHli = "";
            order.CncHli = "";
            order.SplnenoCncHli = "";
            order.FrezaHli = "";
            order.SplnenoFrezaHli = "";
            order.Priprava2Hli = "";
            order.SplnenoPriprava2Hli = "";
            order.StrikarnaHli = "";
            order.SplnenoStrikarnaHli = "";
            order.KompletaceHli = "";
            order.SplnenoKompletaceHli = "";
            order.SkladHli = "";
            order.ProfilSkladHli = "";
            order.BarvaSkladHli = "";
            order.KovaniSkladHli = "";
            order.SkloSkladHli = "";

            //zarubne
            order.PripravaZar = "";
            order.DokumentPripravaZar = "";
            order.InDocaPripravaZar = "";
            order.LaserZar = "";
            order.PohLaserZar = "";
            order.NepohLaserZar = "";
            order.PoutecLaserZar = "";
            order.NuzkyZar = "";
            order.SplnenoNuzkyZar = "";
            order.OhybackaZar = "";
            order.PohOhybackaZar = "";
            order.NepohOhybackaZar = "";
            order.PoutecOhybackaZar = "";
            order.PilaZar = "";
            order.PohPilaZar = "";
            order.NepohPilaZar = "";
            order.VyrazeckaZar = "";
            order.SplnenoVyrazeckaZar = "";
            order.BodovaniZar = "";
            order.SplnenoBodovaniZar = "";
            order.SvarovaniZar = "";
            order.SplnenoSvarovaniZar = "";
            order.BrouseniZar = "";
            order.SplnenoBrouseniZar = "";
            order.StrikarnaZar = "";
            order.SplnenoStrikarnaZar = "";
            order.RameckyZar = "";
            order.NarezRameckyZar = "";
            order.FrezRameckyZar = "";
            order.SvarRameckyZar = "";
            order.StriRameckyZar = "";
            order.KompletaceZar = "";
            order.SplnenoKompletaceZar = "";
            order.SkladZar = "";
            order.PlechSkladZar = "";
            order.BarvaSkladZar = "";
            order.KovaniSkladZar = "";
            order.SkloSkladZar = "";

            order.PripravaKrid = "";
            order.DokumentPripravaKrid = "";
            order.IndocaPripravaKrid = "";
            order.LaserKrid = "";
            order.PohLaserKrid = "";
            order.NepohLaserKrid = "";
            order.KlapackaLaserKrid = "";
            order.OkopLaserKrid = "";
            order.OhybackaKrid = "";
            order.PohOhybackaKrid = "";
            order.NepohOhybackaKrid = "";
            order.KlapackaOhybackaKrid = "";
            order.BodovaniKrid = "";
            order.SplnenoBodovaniKrid = "";
            order.ThermacolKrid = "";
            order.NarezThermacolKrid = "";
            order.LepeniThermacolKrid = "";
            order.CncThermacolKrid = "";
            order.LepeniKrid = "";
            order.SplnenoLepeniKrid = "";
            order.StrikarnaKrid = "";
            order.SplnenoStrikarnaKrid = "";
            order.RameckyKrid = "";
            order.NarezRameckyKrid = "";
            order.FrezRameckyKrid = "";
            order.SvarRameckyKrid = "";
            order.StriRameckyKrid = "";
            order.KompletaceKrid = "";
            order.SplnenoKompletaceKrid = "";
            order.SkladKrid = "";
            order.PlechSkladKrid = "";
            order.KovaniSkladKrid = "";
            order.BarvaSkladKrid = "";
            order.SkloSkladKrid = "";

            order.PripravaC = "";
            order.OblozkyC = "";
            order.StredoveC = "";
            order.RamecekC = "";
            order.KlapackaC = "";
            order.PosuvC = "";
            order.DvereC = "";
            order.DokonceniC = "";
            order.KompletaceC = "";
            order.SkladC = "";

            order.BrouseniDyhaDokonceniC = "";
            order.BrouseniZakladDokonceniC = "";
            order.CentrumDvereC = "";
            order.CentrumStredoveC = "";
            order.CncDvereC = "";
            order.CncOblozkyC = "";
            order.DokumentaceC = "";
            order.DorazovaPosuvC = "";
            order.DvereKompletaceC = "";
            order.FormatkaDvereC = "";
            order.FrezovaniStredoveC = "";
            order.GarnyzPosuvC = "";
            order.HranolPosuvC = "";
            order.InDocaC = "";
            order.KorpusyDvereC = "";
            order.KovaniSkladC = "";
            order.LisDvereC = "";
            order.LisStredoveC = "";
            order.ObalovaniKlapackaC = "";
            order.ObalovaniRamecekC = "";
            order.ObalovatOblozkyC = "";
            order.OlepovackaDvereC = "";
            order.OlepovaniStredoveC = "";
            order.PgmDvereC = "";
            order.RezaniKlapackaC = "";
            order.RezaniRamecekC = "";
            order.RezatOblozkyC = "";
            order.SesazenkyDvereC = "";
            order.SkloSkladC = "";
            order.TypRamecekC = "";
            order.VrchDokonceniC = "";
            order.ZakladDokonceniC = "";
            order.ZarubenKompletaceC = "";

            order.PripravaHliC = "";
            order.DokumentPripravaHliC = "";
            order.IndocaPripravaHliC = "";
            order.NarezHliC = "";
            order.ProfilNarezHliC = "";
            order.ListyNarezHliC = "";
            order.CncHliC = "";
            order.SplnenoCncHliC = "";
            order.FrezaHliC = "";
            order.SplnenoFrezaHliC = "";
            order.Priprava2HliC = "";
            order.SplnenoPriprava2HliC = "";
            order.StrikarnaHliC = "";
            order.SplnenoStrikarnaHliC = "";
            order.KompletaceHliC = "";
            order.SplnenoKompletaceHliC = "";
            order.SkladHliC = "";
            order.ProfilSkladHliC = "";
            order.BarvaSkladHliC = "";
            order.KovaniSkladHliC = "";
            order.SkloSkladHliC = "";

            //zarubne
            order.PripravaZarC = "";
            order.DokumentPripravaZarC = "";
            order.InDocaPripravaZarC = "";
            order.LaserZarC = "";
            order.PohLaserZarC = "";
            order.NepohLaserZarC = "";
            order.PoutecLaserZarC = "";
            order.NuzkyZarC = "";
            order.SplnenoNuzkyZarC = "";
            order.OhybackaZarC = "";
            order.PohOhybackaZarC = "";
            order.NepohOhybackaZarC = "";
            order.PoutecOhybackaZarC = "";
            order.PilaZarC = "";
            order.PohPilaZarC = "";
            order.NepohPilaZarC = "";
            order.VyrazeckaZarC = "";
            order.SplnenoVyrazeckaZarC = "";
            order.BodovaniZarC = "";
            order.SplnenoBodovaniZarC = "";
            order.SvarovaniZarC = "";
            order.SplnenoSvarovaniZarC = "";
            order.BrouseniZarC = "";
            order.SplnenoBrouseniZarC = "";
            order.StrikarnaZarC = "";
            order.SplnenoStrikarnaZarC = "";
            order.RameckyZarC = "";
            order.NarezRameckyZarC = "";
            order.FrezRameckyZarC = "";
            order.SvarRameckyZarC = "";
            order.StriRameckyZarC = "";
            order.KompletaceZarC = "";
            order.SplnenoKompletaceZarC = "";
            order.SkladZarC = "";
            order.PlechSkladZarC = "";
            order.BarvaSkladZarC = "";
            order.KovaniSkladZarC = "";
            order.SkloSkladZarC = "";

            order.PripravaKridC = "";
            order.DokumentPripravaKridC = "";
            order.IndocaPripravaKridC = "";
            order.LaserKridC = "";
            order.PohLaserKridC = "";
            order.NepohLaserKridC = "";
            order.KlapackaLaserKridC = "";
            order.OkopLaserKridC = "";
            order.OhybackaKridC = "";
            order.PohOhybackaKridC = "";
            order.NepohOhybackaKridC = "";
            order.KlapackaOhybackaKridC = "";
            order.BodovaniKridC = "";
            order.SplnenoBodovaniKridC = "";
            order.ThermacolKridC = "";
            order.NarezThermacolKridC = "";
            order.LepeniThermacolKridC = "";
            order.CncThermacolKridC = "";
            order.LepeniKridC = "";
            order.SplnenoLepeniKridC = "";
            order.StrikarnaKridC = "";
            order.SplnenoStrikarnaKridC = "";
            order.RameckyKridC = "";
            order.NarezRameckyKridC = "";
            order.FrezRameckyKridC = "";
            order.SvarRameckyKridC = "";
            order.StriRameckyKridC = "";
            order.KompletaceKridC = "";
            order.SplnenoKompletaceKridC = "";
            order.SkladKridC = "";
            order.PlechSkladKridC = "";
            order.KovaniSkladKridC = "";
            order.BarvaSkladKridC = "";
            order.SkloSkladKridC = "";

            int i;
            order.KridlaKs = "";

            order.KridlaText = "";

            order.Material = "";
            order.MaterialColor = "NULL";

            order.IsArchived = false;

            order.ZahajeniDatum = Convert.ToDateTime(DateTime.Today);
            order.DokonceniDatum = Convert.ToDateTime(DateTime.Today);
            order.PozadovanyDatum = Convert.ToDateTime(DateTime.Today);

            order.Poznamky = "";

            order.Technik = "";
            order.Zakazka = StaticResources.blankRowName;
            order.Color = StaticResources.blankRowColor;
            order.ZakazkaNr = "";
            order.ZarubneKs = "";
            order.ZarubneText = "";

            order.Priprava = "";
            order.Oblozky = "";
            order.Stredove = "";
            order.Ramecek = "";
            order.Klapacka = "";
            order.Posuv = "";
            order.Dvere = "";
            order.KridlaKsExpedovanych = "";
            order.ZarubneKsExpedovanych = "";
            order.Dokonceni = "";
            order.Kompletace = "";
            order.Sklad = "";

            return order;
        }

    }
}
