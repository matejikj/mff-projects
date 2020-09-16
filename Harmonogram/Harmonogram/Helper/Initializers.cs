using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Helper
{
    class Initializers
    {
        //private void btnReset_Click(object sender, RoutedEventArgs e)
        //{
        //    int i = 1;
        //    db.Orders.ToList().ForEach(o =>
        //    {
        //        if (o.Material == "")
        //        {
        //            o.NotBlank = false;
        //            o.Technik = "";
        //            o.ZarubneText = "";
        //            o.KridlaText = "";

        //            if (o.Color == null)
        //            {
        //                o.Color = "#FF00FFFF";
        //            }

        //            o.PripravaC = null;
        //            o.OblozkyC = null;
        //            o.StredoveC = null;
        //            o.RamecekC = null;
        //            o.KlapackaC = null;
        //            o.PosuvC = null;
        //            o.DvereC = null;
        //            o.DokonceniC = null;
        //            o.KompletaceC = null;
        //            o.SkladC = null;

        //            o.PripravaZarC = null;
        //            o.LaserZarC = null;
        //            o.NuzkyZarC = null;
        //            o.OhybackaZarC = null;
        //            o.PilaZarC = null;
        //            o.VyrazeckaZarC = null;
        //            o.BodovaniZarC = null;
        //            o.SvarovaniZarC = null;
        //            o.BrouseniZarC = null;
        //            o.StrikarnaZarC = null;
        //            o.RameckyZarC = null;
        //            o.KompletaceZarC = null;
        //            o.SkladZarC = null;
        //            o.PripravaKridC = null;
        //            o.LaserKridC = null;
        //            o.OhybackaKridC = null;
        //            o.BodovaniKridC = null;
        //            o.ThermacolKridC = null;
        //            o.LepeniKridC = null;
        //            o.StrikarnaKridC = null;
        //            o.RameckyKridC = null;
        //            o.KompletaceKridC = null;
        //            o.SkladKridC = null;

        //            o.PripravaHliC = null;
        //            o.NarezHliC = null;
        //            o.CncHliC = null;
        //            o.FrezaHliC = null;
        //            o.Priprava2HliC = null;
        //            o.StrikarnaHliC = null;
        //            o.KompletaceHliC = null;
        //            o.SkladHliC = null;
        //        }
        //    });
        //    db.SaveChanges();
        //}

        //private void btnResetValues_Click(object sender, RoutedEventArgs e)
        //{
        //    int i = 1;

        //    var list = db.Orders;
        //    Console.WriteLine(list.Count());

        //    string finded = "1";
        //    string value = "✓";

        //    //string finded = "0";
        //    //string value = "";


        //    foreach (var item in list)
        //    {
        //        if (item.BrouseniDyhaDokonceni == finded)
        //        {
        //            item.BrouseniDyhaDokonceni = value;
        //        }
        //        if (item.BrouseniZakladDokonceni == finded)
        //        {
        //            item.BrouseniZakladDokonceni = value;
        //        }

        //        if (item.CentrumDvere == finded)
        //        {
        //            item.CentrumDvere = value;
        //        }

        //        if (item.CentrumStredove == finded)
        //        {
        //            item.CentrumStredove = value;
        //        }

        //        if (item.CncDvere == finded)
        //        {
        //            item.CncDvere = value;
        //        }

        //        if (item.CncOblozky == finded)
        //        {
        //            item.CncOblozky = value;
        //        }

        //        if (item.Dokumentace == finded)
        //        {
        //            item.Dokumentace = value;
        //        }

        //        if (item.DorazovaPosuv == finded)
        //        {
        //            item.DorazovaPosuv = value;
        //        }

        //        if (item.DvereKompletace == finded)
        //        {
        //            item.DvereKompletace = value;
        //        }

        //        if (item.FormatkaDvere == finded)
        //        {
        //            item.FormatkaDvere = value;
        //        }

        //        if (item.FrezovaniStredove == finded)
        //        {
        //            item.FrezovaniStredove = value;
        //        }

        //        if (item.GarnyzPosuv == finded)
        //        {
        //            item.GarnyzPosuv = value;
        //        }

        //        if (item.HranolPosuv == finded)
        //        {
        //            item.HranolPosuv = value;
        //        }

        //        if (item.InDoca == finded)
        //        {
        //            item.InDoca = value;
        //        }

        //        if (item.KorpusyDvere == finded)
        //        {
        //            item.KorpusyDvere = value;
        //        }

        //        if (item.KovaniSklad == finded)
        //        {
        //            item.KovaniSklad = value;
        //        }

        //        if (item.LisDvere == finded)
        //        {
        //            item.LisDvere = value;
        //        }

        //        if (item.LisStredove == finded)
        //        {
        //            item.LisStredove = value;
        //        }

        //        if (item.ObalovaniKlapacka == finded)
        //        {
        //            item.ObalovaniKlapacka = value;
        //        }

        //        if (item.ObalovaniRamecek == finded)
        //        {
        //            item.ObalovaniRamecek = value;
        //        }

        //        if (item.ObalovatOblozky == finded)
        //        {
        //            item.ObalovatOblozky = value;
        //        }

        //        if (item.OlepovackaDvere == finded)
        //        {
        //            item.OlepovackaDvere = value;
        //        }

        //        if (item.OlepovaniStredove == finded)
        //        {
        //            item.OlepovaniStredove = value;
        //        }

        //        if (item.PgmDvere == finded)
        //        {
        //            item.PgmDvere = value;
        //        }

        //        if (item.RezaniKlapacka == finded)
        //        {
        //            item.RezaniKlapacka = value;
        //        }

        //        if (item.RezaniRamecek == finded)
        //        {
        //            item.RezaniRamecek = value;
        //        }

        //        if (item.RezatOblozky == finded)
        //        {
        //            item.RezatOblozky = value;
        //        }

        //        if (item.SesazenkyDvere == finded)
        //        {
        //            item.SesazenkyDvere = value;
        //        }

        //        if (item.SkloSklad == finded)
        //        {
        //            item.SkloSklad = value;
        //        }

        //        if (item.TypRamecek == finded)
        //        {
        //            item.TypRamecek = value;
        //        }

        //        if (item.VrchDokonceni == finded)
        //        {
        //            item.VrchDokonceni = value;
        //        }

        //        if (item.ZakladDokonceni == finded)
        //        {
        //            item.ZakladDokonceni = value;
        //        }

        //        if (item.ZarubenKompletace == finded)
        //        {
        //            item.ZarubenKompletace = value;
        //        }

        //        if (item.BrouseniDyhaDokonceni == finded)
        //        {
        //            item.BrouseniDyhaDokonceni = value;
        //        }

        //        if (item.DokumentPripravaHli == finded) { item.DokumentPripravaHli = value; }
        //        if (item.IndocaPripravaHli == finded) { item.IndocaPripravaHli = value; }
        //        if (item.ProfilNarezHli == finded) { item.ProfilNarezHli = value; }
        //        if (item.ListyNarezHli == finded) { item.ListyNarezHli = value; }
        //        if (item.SplnenoCncHli == finded) { item.SplnenoCncHli = value; }
        //        if (item.SplnenoFrezaHli == finded) { item.SplnenoFrezaHli = value; }
        //        if (item.SplnenoPriprava2Hli == finded) { item.SplnenoPriprava2Hli = value; }
        //        if (item.SplnenoStrikarnaHli == finded) { item.SplnenoStrikarnaHli = value; }
        //        if (item.SplnenoKompletaceHli == finded) { item.SplnenoKompletaceHli = value; }
        //        if (item.ProfilSkladHli == finded) { item.ProfilSkladHli = value; }
        //        if (item.BarvaSkladHli == finded) { item.BarvaSkladHli = value; }
        //        if (item.KovaniSkladHli == finded) { item.KovaniSkladHli = value; }
        //        if (item.SkloSkladHli == finded) { item.SkloSkladHli = value; }

        //        if (item.DokumentPripravaZar == finded) { item.DokumentPripravaZar = value; }
        //        if (item.InDocaPripravaZar == finded) { item.InDocaPripravaZar = value; }

        //        if (item.PohLaserZar == finded) { item.PohLaserZar = value; }
        //        if (item.NepohLaserZar == finded) { item.NepohLaserZar = value; }
        //        if (item.PoutecLaserZar == finded) { item.PoutecLaserZar = value; }

        //        if (item.SplnenoNuzkyZar == finded) { item.SplnenoNuzkyZar = value; }

        //        if (item.PohOhybackaZar == finded) { item.PohOhybackaZar = value; }
        //        if (item.NepohOhybackaZar == finded) { item.NepohOhybackaZar = value; }
        //        if (item.PoutecOhybackaZar == finded) { item.PoutecOhybackaZar = value; }

        //        if (item.PohPilaZar == finded) { item.PohPilaZar = value; }
        //        if (item.NepohPilaZar == finded) { item.NepohPilaZar = value; }

        //        if (item.SplnenoVyrazeckaZar == finded) { item.SplnenoVyrazeckaZar = value; }

        //        if (item.SplnenoBodovaniZar == finded) { item.SplnenoBodovaniZar = value; }

        //        if (item.SplnenoSvarovaniZar == finded) { item.SplnenoSvarovaniZar = value; }

        //        if (item.SplnenoBrouseniZar == finded) { item.SplnenoBrouseniZar = value; }

        //        if (item.SplnenoStrikarnaZar == finded) { item.SplnenoStrikarnaZar = value; }

        //        if (item.NarezRameckyZar == finded) { item.NarezRameckyZar = value; }
        //        if (item.FrezRameckyZar == finded) { item.FrezRameckyZar = value; }
        //        if (item.SvarRameckyZar == finded) { item.SvarRameckyZar = value; }
        //        if (item.StriRameckyZar == finded) { item.StriRameckyZar = value; }

        //        if (item.SplnenoKompletaceZar == finded) { item.SplnenoKompletaceZar = value; }

        //        if (item.PlechSkladZar == finded) { item.PlechSkladZar = value; }
        //        if (item.BarvaSkladZar == finded) { item.BarvaSkladZar = value; }
        //        if (item.KovaniSkladZar == finded) { item.KovaniSkladZar = value; }
        //        if (item.SkloSkladZar == finded) { item.SkloSkladZar = value; }


        //        if (item.DokumentPripravaKrid == finded) { item.DokumentPripravaKrid = value; }
        //        if (item.IndocaPripravaKrid == finded) { item.IndocaPripravaKrid = value; }

        //        if (item.PohLaserKrid == finded) { item.PohLaserKrid = value; }
        //        if (item.NepohLaserKrid == finded) { item.NepohLaserKrid = value; }
        //        if (item.KlapackaLaserKrid == finded) { item.KlapackaLaserKrid = value; }
        //        if (item.OkopLaserKrid == finded) { item.OkopLaserKrid = value; }

        //        if (item.PohOhybackaKrid == finded) { item.PohOhybackaKrid = value; }
        //        if (item.NepohOhybackaKrid == finded) { item.NepohOhybackaKrid = value; }
        //        if (item.KlapackaOhybackaKrid == finded) { item.KlapackaOhybackaKrid = value; }

        //        if (item.SplnenoBodovaniKrid == finded) { item.SplnenoBodovaniKrid = value; }

        //        if (item.NarezThermacolKrid == finded) { item.NarezThermacolKrid = value; }
        //        if (item.LepeniThermacolKrid == finded) { item.LepeniThermacolKrid = value; }
        //        if (item.CncThermacolKrid == finded) { item.CncThermacolKrid = value; }

        //        if (item.SplnenoLepeniKrid == finded) { item.SplnenoLepeniKrid = value; }

        //        if (item.SplnenoStrikarnaKrid == finded) { item.SplnenoStrikarnaKrid = value; }

        //        if (item.NarezRameckyKrid == finded) { item.NarezRameckyKrid = value; }
        //        if (item.FrezRameckyKrid == finded) { item.FrezRameckyKrid = value; }
        //        if (item.SvarRameckyKrid == finded) { item.SvarRameckyKrid = value; }
        //        if (item.StriRameckyKrid == finded) { item.StriRameckyKrid = value; }

        //        if (item.SplnenoKompletaceKrid == finded) { item.SplnenoKompletaceKrid = value; }

        //        if (item.PlechSkladKrid == finded) { item.PlechSkladKrid = value; }
        //        if (item.KovaniSkladKrid == finded) { item.KovaniSkladKrid = value; }
        //        if (item.BarvaSkladKrid == finded) { item.BarvaSkladKrid = value; }
        //        if (item.SkloSkladKrid == finded) { item.SkloSkladKrid = value; }


        //        if (item.Priprava == finded) { item.Priprava = value; }
        //        if (item.Oblozky == finded) { item.Oblozky = value; }
        //        if (item.Stredove == finded) { item.Stredove = value; }
        //        if (item.Ramecek == finded) { item.Ramecek = value; }
        //        if (item.Klapacka == finded) { item.Klapacka = value; }
        //        if (item.Posuv == finded) { item.Posuv = value; }
        //        if (item.Dvere == finded) { item.Dvere = value; }
        //        if (item.Dokonceni == finded) { item.Dokonceni = value; }
        //        if (item.Kompletace == finded) { item.Kompletace = value; }
        //        if (item.Sklad == finded) { item.Sklad = value; }
        //        if (item.PripravaHli == finded) { item.PripravaHli = value; }
        //        if (item.NarezHli == finded) { item.NarezHli = value; }
        //        if (item.CncHli == finded) { item.CncHli = value; }
        //        if (item.FrezaHli == finded) { item.FrezaHli = value; }
        //        if (item.Priprava2Hli == finded) { item.Priprava2Hli = value; }
        //        if (item.StrikarnaHli == finded) { item.StrikarnaHli = value; }
        //        if (item.KompletaceHli == finded) { item.KompletaceHli = value; }
        //        if (item.SkladHli == finded) { item.SkladHli = value; }
        //        if (item.PripravaZar == finded) { item.PripravaZar = value; }
        //        if (item.LaserZar == finded) { item.LaserZar = value; }
        //        if (item.NuzkyZar == finded) { item.NuzkyZar = value; }
        //        if (item.OhybackaZar == finded) { item.OhybackaZar = value; }
        //        if (item.PilaZar == finded) { item.PilaZar = value; }
        //        if (item.VyrazeckaZar == finded) { item.VyrazeckaZar = value; }
        //        if (item.BodovaniZar == finded) { item.BodovaniZar = value; }
        //        if (item.SvarovaniZar == finded) { item.SvarovaniZar = value; }
        //        if (item.BrouseniZar == finded) { item.BrouseniZar = value; }
        //        if (item.StrikarnaZar == finded) { item.StrikarnaZar = value; }
        //        if (item.RameckyZar == finded) { item.RameckyZar = value; }
        //        if (item.KompletaceZar == finded) { item.KompletaceZar = value; }
        //        if (item.SkladZar == finded) { item.SkladZar = value; }
        //        if (item.PripravaKrid == finded) { item.PripravaKrid = value; }
        //        if (item.LaserKrid == finded) { item.LaserKrid = value; }
        //        if (item.OhybackaKrid == finded) { item.OhybackaKrid = value; }
        //        if (item.BodovaniKrid == finded) { item.BodovaniKrid = value; }
        //        if (item.ThermacolKrid == finded) { item.ThermacolKrid = value; }
        //        if (item.LepeniKrid == finded) { item.LepeniKrid = value; }
        //        if (item.StrikarnaKrid == finded) { item.StrikarnaKrid = value; }
        //        if (item.RameckyKrid == finded) { item.RameckyKrid = value; }
        //        if (item.KompletaceKrid == finded) { item.KompletaceKrid = value; }
        //        if (item.SkladKrid == finded) { item.SkladKrid = value; }
        //    }
        //    db.SaveChanges();
        //}

        //private void btnResetColors_Click(object sender, RoutedEventArgs e)
        //{
        //    int i = 1;

        //    var list = db.Orders;
        //    Console.WriteLine(list.Count());

        //    foreach (var item in list)
        //    {
        //        if (item.BrouseniDyhaDokonceniC == null)
        //        {
        //            item.BrouseniDyhaDokonceniC = "Yellow";
        //        }
        //        if (item.BrouseniZakladDokonceniC == null)
        //        {
        //            item.BrouseniZakladDokonceniC = "Yellow";
        //        }

        //        if (item.CentrumDvereC == null)
        //        {
        //            item.CentrumDvereC = "Yellow";
        //        }

        //        if (item.CentrumStredoveC == null)
        //        {
        //            item.CentrumStredoveC = "Yellow";
        //        }

        //        if (item.CncDvereC == null)
        //        {
        //            item.CncDvereC = "Yellow";
        //        }

        //        if (item.CncOblozkyC == null)
        //        {
        //            item.CncOblozkyC = "Yellow";
        //        }

        //        if (item.DokumentaceC == null)
        //        {
        //            item.DokumentaceC = "Yellow";
        //        }

        //        if (item.DorazovaPosuvC == null)
        //        {
        //            item.DorazovaPosuvC = "Yellow";
        //        }

        //        if (item.DvereKompletaceC == null)
        //        {
        //            item.DvereKompletaceC = "Yellow";
        //        }

        //        if (item.FormatkaDvereC == null)
        //        {
        //            item.FormatkaDvereC = "Yellow";
        //        }

        //        if (item.FrezovaniStredoveC == null)
        //        {
        //            item.FrezovaniStredoveC = "Yellow";
        //        }

        //        if (item.GarnyzPosuvC == null)
        //        {
        //            item.GarnyzPosuvC = "Yellow";
        //        }

        //        if (item.HranolPosuvC == null)
        //        {
        //            item.HranolPosuvC = "Yellow";
        //        }

        //        if (item.InDocaC == null)
        //        {
        //            item.InDocaC = "Yellow";
        //        }

        //        if (item.KorpusyDvereC == null)
        //        {
        //            item.KorpusyDvereC = "Yellow";
        //        }

        //        if (item.KovaniSkladC == null)
        //        {
        //            item.KovaniSkladC = "Yellow";
        //        }

        //        if (item.LisDvereC == null)
        //        {
        //            item.LisDvereC = "Yellow";
        //        }

        //        if (item.LisStredoveC == null)
        //        {
        //            item.LisStredoveC = "Yellow";
        //        }

        //        if (item.ObalovaniKlapackaC == null)
        //        {
        //            item.ObalovaniKlapackaC = "Yellow";
        //        }

        //        if (item.ObalovaniRamecekC == null)
        //        {
        //            item.ObalovaniRamecekC = "Yellow";
        //        }

        //        if (item.ObalovatOblozkyC == null)
        //        {
        //            item.ObalovatOblozkyC = "Yellow";
        //        }

        //        if (item.OlepovackaDvereC == null)
        //        {
        //            item.OlepovackaDvereC = "Yellow";
        //        }

        //        if (item.OlepovaniStredoveC == null)
        //        {
        //            item.OlepovaniStredoveC = "Yellow";
        //        }

        //        if (item.PgmDvereC == null)
        //        {
        //            item.PgmDvereC = "Yellow";
        //        }

        //        if (item.RezaniKlapackaC == null)
        //        {
        //            item.RezaniKlapackaC = "Yellow";
        //        }

        //        if (item.RezaniRamecekC == null)
        //        {
        //            item.RezaniRamecekC = "Yellow";
        //        }

        //        if (item.RezatOblozkyC == null)
        //        {
        //            item.RezatOblozkyC = "Yellow";
        //        }

        //        if (item.SesazenkyDvereC == null)
        //        {
        //            item.SesazenkyDvereC = "Yellow";
        //        }

        //        if (item.SkloSkladC == null)
        //        {
        //            item.SkloSkladC = "Yellow";
        //        }

        //        if (item.TypRamecekC == null)
        //        {
        //            item.TypRamecekC = "Yellow";
        //        }

        //        if (item.VrchDokonceniC == null)
        //        {
        //            item.VrchDokonceniC = "Yellow";
        //        }

        //        if (item.ZakladDokonceniC == null)
        //        {
        //            item.ZakladDokonceniC = "Yellow";
        //        }

        //        if (item.ZarubenKompletaceC == null)
        //        {
        //            item.ZarubenKompletaceC = "Yellow";
        //        }

        //        if (item.BrouseniDyhaDokonceniC == null)
        //        {
        //            item.BrouseniDyhaDokonceniC = "Yellow";
        //        }

        //        if (item.DokumentPripravaHliC == null) { item.DokumentPripravaHliC = "Yellow"; }
        //        if (item.IndocaPripravaHliC == null) { item.IndocaPripravaHliC = "Yellow"; }
        //        if (item.ProfilNarezHliC == null) { item.ProfilNarezHliC = "Yellow"; }
        //        if (item.ListyNarezHliC == null) { item.ListyNarezHliC = "Yellow"; }
        //        if (item.SplnenoCncHliC == null) { item.SplnenoCncHliC = "Yellow"; }
        //        if (item.SplnenoFrezaHliC == null) { item.SplnenoFrezaHliC = "Yellow"; }
        //        if (item.SplnenoPriprava2HliC == null) { item.SplnenoPriprava2HliC = "Yellow"; }
        //        if (item.SplnenoStrikarnaHliC == null) { item.SplnenoStrikarnaHliC = "Yellow"; }
        //        if (item.SplnenoKompletaceHliC == null) { item.SplnenoKompletaceHliC = "Yellow"; }
        //        if (item.ProfilSkladHliC == null) { item.ProfilSkladHliC = "Yellow"; }
        //        if (item.BarvaSkladHliC == null) { item.BarvaSkladHliC = "Yellow"; }
        //        if (item.KovaniSkladHliC == null) { item.KovaniSkladHliC = "Yellow"; }
        //        if (item.SkloSkladHliC == null) { item.SkloSkladHliC = "Yellow"; }

        //        if (item.DokumentPripravaZarC == null) { item.DokumentPripravaZarC = "Yellow"; }
        //        if (item.InDocaPripravaZarC == null) { item.InDocaPripravaZarC = "Yellow"; }

        //        if (item.PohLaserZarC == null) { item.PohLaserZarC = "Yellow"; }
        //        if (item.NepohLaserZarC == null) { item.NepohLaserZarC = "Yellow"; }
        //        if (item.PoutecLaserZarC == null) { item.PoutecLaserZarC = "Yellow"; }

        //        if (item.SplnenoNuzkyZarC == null) { item.SplnenoNuzkyZarC = "Yellow"; }

        //        if (item.PohOhybackaZarC == null) { item.PohOhybackaZarC = "Yellow"; }
        //        if (item.NepohOhybackaZarC == null) { item.NepohOhybackaZarC = "Yellow"; }
        //        if (item.PoutecOhybackaZarC == null) { item.PoutecOhybackaZarC = "Yellow"; }

        //        if (item.PohPilaZarC == null) { item.PohPilaZarC = "Yellow"; }
        //        if (item.NepohPilaZarC == null) { item.NepohPilaZarC = "Yellow"; }

        //        if (item.SplnenoVyrazeckaZarC == null) { item.SplnenoVyrazeckaZarC = "Yellow"; }

        //        if (item.SplnenoBodovaniZarC == null) { item.SplnenoBodovaniZarC = "Yellow"; }

        //        if (item.SplnenoSvarovaniZarC == null) { item.SplnenoSvarovaniZarC = "Yellow"; }

        //        if (item.SplnenoBrouseniZarC == null) { item.SplnenoBrouseniZarC = "Yellow"; }

        //        if (item.SplnenoStrikarnaZarC == null) { item.SplnenoStrikarnaZarC = "Yellow"; }

        //        if (item.NarezRameckyZarC == null) { item.NarezRameckyZarC = "Yellow"; }
        //        if (item.FrezRameckyZarC == null) { item.FrezRameckyZarC = "Yellow"; }
        //        if (item.SvarRameckyZarC == null) { item.SvarRameckyZarC = "Yellow"; }
        //        if (item.StriRameckyZarC == null) { item.StriRameckyZarC = "Yellow"; }

        //        if (item.SplnenoKompletaceZarC == null) { item.SplnenoKompletaceZarC = "Yellow"; }

        //        if (item.PlechSkladZarC == null) { item.PlechSkladZarC = "Yellow"; }
        //        if (item.BarvaSkladZarC == null) { item.BarvaSkladZarC = "Yellow"; }
        //        if (item.KovaniSkladZarC == null) { item.KovaniSkladZarC = "Yellow"; }
        //        if (item.SkloSkladZarC == null) { item.SkloSkladZarC = "Yellow"; }


        //        if (item.DokumentPripravaKridC == null) { item.DokumentPripravaKridC = "Yellow"; }
        //        if (item.IndocaPripravaKridC == null) { item.IndocaPripravaKridC = "Yellow"; }

        //        if (item.PohLaserKridC == null) { item.PohLaserKridC = "Yellow"; }
        //        if (item.NepohLaserKridC == null) { item.NepohLaserKridC = "Yellow"; }
        //        if (item.KlapackaLaserKridC == null) { item.KlapackaLaserKridC = "Yellow"; }
        //        if (item.OkopLaserKridC == null) { item.OkopLaserKridC = "Yellow"; }

        //        if (item.PohOhybackaKridC == null) { item.PohOhybackaKridC = "Yellow"; }
        //        if (item.NepohOhybackaKridC == null) { item.NepohOhybackaKridC = "Yellow"; }
        //        if (item.KlapackaOhybackaKridC == null) { item.KlapackaOhybackaKridC = "Yellow"; }

        //        if (item.SplnenoBodovaniKridC == null) { item.SplnenoBodovaniKridC = "Yellow"; }

        //        if (item.NarezThermacolKridC == null) { item.NarezThermacolKridC = "Yellow"; }
        //        if (item.LepeniThermacolKridC == null) { item.LepeniThermacolKridC = "Yellow"; }
        //        if (item.CncThermacolKridC == null) { item.CncThermacolKridC = "Yellow"; }

        //        if (item.SplnenoLepeniKridC == null) { item.SplnenoLepeniKridC = "Yellow"; }

        //        if (item.SplnenoStrikarnaKridC == null) { item.SplnenoStrikarnaKridC = "Yellow"; }

        //        if (item.NarezRameckyKridC == null) { item.NarezRameckyKridC = "Yellow"; }
        //        if (item.FrezRameckyKridC == null) { item.FrezRameckyKridC = "Yellow"; }
        //        if (item.SvarRameckyKridC == null) { item.SvarRameckyKridC = "Yellow"; }
        //        if (item.StriRameckyKridC == null) { item.StriRameckyKridC = "Yellow"; }

        //        if (item.SplnenoKompletaceKridC == null) { item.SplnenoKompletaceKridC = "Yellow"; }

        //        if (item.PlechSkladKridC == null) { item.PlechSkladKridC = "Yellow"; }
        //        if (item.KovaniSkladKridC == null) { item.KovaniSkladKridC = "Yellow"; }
        //        if (item.BarvaSkladKridC == null) { item.BarvaSkladKridC = "Yellow"; }
        //        if (item.SkloSkladKridC == null) { item.SkloSkladKridC = "Yellow"; }


        //        if (item.PripravaC == null) { item.PripravaC = "Yellow"; }
        //        if (item.OblozkyC == null) { item.OblozkyC = "Yellow"; }
        //        if (item.StredoveC == null) { item.StredoveC = "Yellow"; }
        //        if (item.RamecekC == null) { item.RamecekC = "Yellow"; }
        //        if (item.KlapackaC == null) { item.KlapackaC = "Yellow"; }
        //        if (item.PosuvC == null) { item.PosuvC = "Yellow"; }
        //        if (item.DvereC == null) { item.DvereC = "Yellow"; }
        //        if (item.DokonceniC == null) { item.DokonceniC = "Yellow"; }
        //        if (item.KompletaceC == null) { item.KompletaceC = "Yellow"; }
        //        if (item.SkladC == null) { item.SkladC = "Yellow"; }
        //        if (item.PripravaHliC == null) { item.PripravaHliC = "Yellow"; }
        //        if (item.NarezHliC == null) { item.NarezHliC = "Yellow"; }
        //        if (item.CncHliC == null) { item.CncHliC = "Yellow"; }
        //        if (item.FrezaHliC == null) { item.FrezaHliC = "Yellow"; }
        //        if (item.Priprava2HliC == null) { item.Priprava2HliC = "Yellow"; }
        //        if (item.StrikarnaHliC == null) { item.StrikarnaHliC = "Yellow"; }
        //        if (item.KompletaceHliC == null) { item.KompletaceHliC = "Yellow"; }
        //        if (item.SkladHliC == null) { item.SkladHliC = "Yellow"; }
        //        if (item.PripravaZarC == null) { item.PripravaZarC = "Yellow"; }
        //        if (item.LaserZarC == null) { item.LaserZarC = "Yellow"; }
        //        if (item.NuzkyZarC == null) { item.NuzkyZarC = "Yellow"; }
        //        if (item.OhybackaZarC == null) { item.OhybackaZarC = "Yellow"; }
        //        if (item.PilaZarC == null) { item.PilaZarC = "Yellow"; }
        //        if (item.VyrazeckaZarC == null) { item.VyrazeckaZarC = "Yellow"; }
        //        if (item.BodovaniZarC == null) { item.BodovaniZarC = "Yellow"; }
        //        if (item.SvarovaniZarC == null) { item.SvarovaniZarC = "Yellow"; }
        //        if (item.BrouseniZarC == null) { item.BrouseniZarC = "Yellow"; }
        //        if (item.StrikarnaZarC == null) { item.StrikarnaZarC = "Yellow"; }
        //        if (item.RameckyZarC == null) { item.RameckyZarC = "Yellow"; }
        //        if (item.KompletaceZarC == null) { item.KompletaceZarC = "Yellow"; }
        //        if (item.SkladZarC == null) { item.SkladZarC = "Yellow"; }
        //        if (item.PripravaKridC == null) { item.PripravaKridC = "Yellow"; }
        //        if (item.LaserKridC == null) { item.LaserKridC = "Yellow"; }
        //        if (item.OhybackaKridC == null) { item.OhybackaKridC = "Yellow"; }
        //        if (item.BodovaniKridC == null) { item.BodovaniKridC = "Yellow"; }
        //        if (item.ThermacolKridC == null) { item.ThermacolKridC = "Yellow"; }
        //        if (item.LepeniKridC == null) { item.LepeniKridC = "Yellow"; }
        //        if (item.StrikarnaKridC == null) { item.StrikarnaKridC = "Yellow"; }
        //        if (item.RameckyKridC == null) { item.RameckyKridC = "Yellow"; }
        //        if (item.KompletaceKridC == null) { item.KompletaceKridC = "Yellow"; }
        //        if (item.SkladKridC == null) { item.SkladKridC = "Yellow"; }
        //    }
        //    db.SaveChanges();
        //}

        //private void btnResetValues_Click(object sender, RoutedEventArgs e)
        //{
        //    var list = db.Orders;
        //    Console.WriteLine(list.Count());

        //    foreach (var o in list)
        //    {
        //        o.PripravaC = "Yellow";
        //        o.OblozkyC = "Yellow";
        //        o.StredoveC = "Yellow";
        //        o.RamecekC = "Yellow";
        //        o.KlapackaC = "Yellow";
        //        o.PosuvC = "Yellow";
        //        o.DvereC = "Yellow";
        //        o.DokonceniC = "Yellow";
        //        o.KompletaceC = "Yellow";
        //        o.SkladC = "Yellow";



        //        if ((o.Dokumentace == "✓") && (o.InDoca == "✓"))
        //        {
        //            o.PripravaC = "LimeGreen";
        //        }

        //        if ((o.RezatOblozky == "✓") && (o.ObalovatOblozky == "✓") && (o.CncOblozky == "✓"))
        //        {
        //            o.OblozkyC = "LimeGreen";
        //        }

        //        if ((o.OlepovaniStredove == "✓") && (o.LisStredove == "✓") &&
        //            (o.FrezovaniStredove == "✓") && (o.CentrumStredove == "✓"))
        //        {
        //            o.StredoveC = "LimeGreen";
        //        }

        //        if ((o.ObalovaniRamecek == "✓") && (o.RezaniRamecek == "✓") && (o.TypRamecek == "✓"))
        //        {
        //            o.RamecekC = "LimeGreen";
        //        }

        //        if ((o.ObalovaniKlapacka == "✓") && (o.RezaniKlapacka == "✓"))
        //        {
        //            o.KlapackaC = "LimeGreen";
        //        }

        //        if ((o.GarnyzPosuv == "✓") && (o.DorazovaPosuv == "✓") && (o.HranolPosuv == "✓"))
        //        {
        //            o.PosuvC = "LimeGreen";
        //        }

        //        if ((o.CentrumDvere == "✓") && (o.CncDvere == "✓") &&
        //            (o.FormatkaDvere == "✓") && (o.KorpusyDvere == "✓") &&
        //            (o.LisDvere == "✓") && (o.OlepovackaDvere == "✓") &&
        //            (o.PgmDvere == "✓") && (o.SesazenkyDvere == "✓"))
        //        {
        //            o.DvereC = "LimeGreen";
        //        }

        //        if ((o.BrouseniDyhaDokonceni == "✓") && (o.BrouseniZakladDokonceni == "✓") &&
        //            (o.VrchDokonceni == "✓") && (o.ZakladDokonceni == "✓"))
        //        {
        //            o.DokonceniC = "LimeGreen";
        //        }

        //        if ((o.DvereKompletace == "✓") && (o.ZarubenKompletace == "✓"))
        //        {
        //            o.KompletaceC = "LimeGreen";
        //        }

        //        if ((o.KovaniSklad == "✓") && (o.SkloSklad == "✓"))
        //        {
        //            o.SkladC = "LimeGreen";
        //        }

        //        o.PripravaZarC = "Yellow";
        //        o.LaserZarC = "Yellow";
        //        o.NuzkyZarC = "Yellow";
        //        o.OhybackaZarC = "Yellow";
        //        o.PilaZarC = "Yellow";
        //        o.VyrazeckaZarC = "Yellow";
        //        o.BodovaniZarC = "Yellow";
        //        o.SvarovaniZarC = "Yellow";
        //        o.BrouseniZarC = "Yellow";
        //        o.StrikarnaZarC = "Yellow";
        //        o.RameckyZarC = "Yellow";
        //        o.KompletaceZarC = "Yellow";
        //        o.SkladZarC = "Yellow";
        //        o.PripravaKridC = "Yellow";
        //        o.LaserKridC = "Yellow";
        //        o.OhybackaKridC = "Yellow";
        //        o.BodovaniKridC = "Yellow";
        //        o.ThermacolKridC = "Yellow";
        //        o.LepeniKridC = "Yellow";
        //        o.StrikarnaKridC = "Yellow";
        //        o.RameckyKridC = "Yellow";
        //        o.KompletaceKridC = "Yellow";
        //        o.SkladKridC = "Yellow";




        //        if ((o.InDocaPripravaZar == "✓") && (o.DokumentPripravaZar == "✓"))
        //        {
        //            o.PripravaZarC = "Limegreen";
        //        }
        //        if ((o.NepohLaserZar == "✓") && (o.PohLaserZar == "✓") && (o.PoutecLaserZar == "✓"))
        //        {
        //            o.LaserZarC = "Limegreen";
        //        }
        //        if ((o.SplnenoNuzkyZar == "✓"))
        //        {
        //            o.NuzkyZarC = "Limegreen";
        //        }
        //        if ((o.PohOhybackaZar == "✓") && (o.PoutecOhybackaZar == "✓") &&
        //            (o.NepohOhybackaZar == "✓"))
        //        {
        //            o.OhybackaZarC = "Limegreen";
        //        }
        //        if ((o.NepohPilaZar == "✓") && (o.PohPilaZar == "✓"))
        //        {
        //            o.PilaZarC = "Limegreen";
        //        }
        //        if ((o.SplnenoVyrazeckaZar == "✓"))
        //        {
        //            o.VyrazeckaZarC = "Limegreen";
        //        }
        //        if ((o.SplnenoBodovaniZar == "✓"))
        //        {
        //            o.BodovaniZarC = "Limegreen";
        //        }
        //        if ((o.SplnenoSvarovaniZar == "✓"))
        //        {
        //            o.SkladC = "Limegreen";
        //        }
        //        if ((o.SplnenoBrouseniZar == "✓"))
        //        {
        //            o.BrouseniZarC = "Limegreen";
        //        }
        //        if ((o.SplnenoStrikarnaZar == "✓"))
        //        {
        //            o.StrikarnaZarC = "Limegreen";
        //        }
        //        if ((o.FrezRameckyZar == "✓") && (o.StriRameckyZar == "✓") &&
        //            (o.SvarRameckyZar == "✓") && (o.NarezRameckyZar == "✓"))
        //        {
        //            o.RameckyZarC = "Limegreen";
        //        }
        //        if ((o.SplnenoKompletaceZar == "✓"))
        //        {
        //            o.KompletaceZarC = "Limegreen";
        //        }
        //        if ((o.PlechSkladZar == "✓") && (o.BarvaSkladZar == "✓") &&
        //            (o.KovaniSkladZar == "✓") && (o.SkloSkladZar == "✓"))
        //        {
        //            o.SkladZarC = "Limegreen";
        //        }


        //        if ((o.IndocaPripravaKrid == "✓") && (o.DokumentPripravaKrid == "✓"))
        //        {
        //            o.PripravaKridC = "Limegreen";
        //        }
        //        if ((o.NepohLaserKrid == "✓") && (o.KlapackaLaserKrid == "✓") &&
        //            (o.PohLaserKrid == "✓") && (o.OkopLaserKrid == "✓"))
        //        {
        //            o.LaserKridC = "Limegreen";
        //        }
        //        if ((o.NepohOhybackaKrid == "✓") && (o.PohOhybackaKrid == "✓") &&
        //            (o.KlapackaOhybackaKrid == "✓"))
        //        {
        //            o.OhybackaKridC = "Limegreen";
        //        }
        //        if ((o.SplnenoBodovaniKrid == "✓"))
        //        {
        //            o.BodovaniKridC = "Limegreen";
        //        }
        //        if ((o.NarezThermacolKrid == "✓") && (o.LepeniThermacolKrid == "✓") &&
        //            (o.CncThermacolKrid == "✓"))
        //        {
        //            o.ThermacolKridC = "Limegreen";
        //        }
        //        if ((o.SplnenoLepeniKrid == "✓"))
        //        {
        //            o.LepeniKridC = "Limegreen";
        //        }
        //        if ((o.SplnenoStrikarnaKrid == "✓"))
        //        {
        //            o.StrikarnaKridC = "Limegreen";
        //        }
        //        if ((o.StriRameckyKrid == "✓") && (o.NarezRameckyKrid == "✓") &&
        //            (o.FrezRameckyKrid == "✓") && (o.SvarRameckyKrid == "✓"))
        //        {
        //            o.RameckyKridC = "Limegreen";
        //        }
        //        if ((o.SplnenoKompletaceKrid == "✓"))
        //        {
        //            o.KompletaceKridC = "Limegreen";
        //        }
        //        if ((o.PlechSkladKrid == "✓") && (o.SkloSkladKrid == "✓") &&
        //            (o.KovaniSkladKrid == "✓") && (o.BarvaSkladKrid == "✓"))
        //        {
        //            o.SkladKridC = "Limegreen";
        //        }

        //        o.PripravaHliC = "Yellow";
        //        o.NarezHliC = "Yellow";
        //        o.CncHliC = "Yellow";
        //        o.FrezaHliC = "Yellow";
        //        o.Priprava2HliC = "Yellow";
        //        o.StrikarnaHliC = "Yellow";
        //        o.KompletaceHliC = "Yellow";
        //        o.SkladHliC = "Yellow";


        //        if ((o.IndocaPripravaHli == "✓") && (o.DokumentPripravaHli == "✓"))
        //        {
        //            o.PripravaHliC = "LimeGreen";
        //        }
        //        if ((o.ProfilNarezHli == "✓") && (o.ListyNarezHli == "✓"))
        //        {
        //            o.NarezHliC = "LimeGreen";
        //        }
        //        if ((o.SplnenoCncHli == "✓"))
        //        {
        //            o.CncHliC = "LimeGreen";
        //        }
        //        if ((o.SplnenoFrezaHli == "✓"))
        //        {
        //            o.FrezaHliC = "LimeGreen";
        //        }
        //        if ((o.SplnenoPriprava2Hli == "✓"))
        //        {
        //            o.Priprava2HliC = "LimeGreen";
        //        }
        //        if ((o.SplnenoStrikarnaHli == "✓"))
        //        {
        //            o.StrikarnaHliC = "LimeGreen";
        //        }
        //        if ((o.SplnenoKompletaceHli == "✓"))
        //        {
        //            o.KompletaceHliC = "LimeGreen";
        //        }
        //        if ((o.ProfilSkladHli == "✓") && (o.BarvaSkladHli == "✓") &&
        //            (o.KovaniSkladHli == "✓") && (o.SkloSkladHli == "✓"))
        //        {
        //            o.SkladHliC = "LimeGreen";
        //        }
        //    }
        //}
    }
}
