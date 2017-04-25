using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Windows.Forms;

namespace DropStatistics.CurrencyPrice
{
    class CurrencyInChaos
    {
        private Dictionary<string, CurrencyCache> PriceCache = new Dictionary<string, CurrencyCache>();
        private Dictionary<string, int> PoeTradeCurrencyIndex = new Dictionary<string, int>();
        private string League;
        private int TopN;
        public CurrencyInChaos(string league, int topN = 5)
        {
            League = league;
            TopN = topN;
            #region Currency
            PoeTradeCurrencyIndex.Add("Orb of Alteration", 1);
            PoeTradeCurrencyIndex.Add("Orb of Fusing", 2);
            PoeTradeCurrencyIndex.Add("Orb of Alchemy", 3);
            PoeTradeCurrencyIndex.Add("Chaos Orb", 4);
            PoeTradeCurrencyIndex.Add("Gemcutter's Prism", 5);
            PoeTradeCurrencyIndex.Add("Exalted Orb", 6);
            PoeTradeCurrencyIndex.Add("Chromatic Orb", 7);
            PoeTradeCurrencyIndex.Add("Jeweller's Orb", 8);
            PoeTradeCurrencyIndex.Add("Orb of Chance", 9);
            PoeTradeCurrencyIndex.Add("Cartographer's Chisel", 10);
            PoeTradeCurrencyIndex.Add("Orb of Scouring", 11);
            PoeTradeCurrencyIndex.Add("Blessed Orb", 12);
            PoeTradeCurrencyIndex.Add("Orb of Regret", 13);
            PoeTradeCurrencyIndex.Add("Regal Orb", 14);
            PoeTradeCurrencyIndex.Add("Divine Orb", 15);
            PoeTradeCurrencyIndex.Add("Vaal Orb", 16);
            PoeTradeCurrencyIndex.Add("Scroll of Wisdom", 17);
            PoeTradeCurrencyIndex.Add("Portal Scroll", 18);
            PoeTradeCurrencyIndex.Add("Armourer's Scrap", 19);
            PoeTradeCurrencyIndex.Add("Blacksmith's Whetstone", 20);
            PoeTradeCurrencyIndex.Add("Glassblower's Bauble", 21);
            PoeTradeCurrencyIndex.Add("Orb of Transmutation", 22);
            PoeTradeCurrencyIndex.Add("Orb of Augmentation", 23);
            PoeTradeCurrencyIndex.Add("Mirror of Kalandra", 24);
            PoeTradeCurrencyIndex.Add("Eternal Orb", 25);
            PoeTradeCurrencyIndex.Add("Perandus Coin", 26);
            PoeTradeCurrencyIndex.Add("Silver Coin", 35);
            PoeTradeCurrencyIndex.Add("Sacrifice at Dusk", 27);
            PoeTradeCurrencyIndex.Add("Sacrifice at Midnight", 28);
            PoeTradeCurrencyIndex.Add("Sacrfice at Dawn", 29);
            PoeTradeCurrencyIndex.Add("Sacrifice at Noon", 30);
            PoeTradeCurrencyIndex.Add("Mortal Grief", 31);
            PoeTradeCurrencyIndex.Add("Mortal Rage", 32);
            PoeTradeCurrencyIndex.Add("Mortal Hope", 33);
            PoeTradeCurrencyIndex.Add("Mortal Ignorance", 34);
            PoeTradeCurrencyIndex.Add("Eber's Key", 36);
            PoeTradeCurrencyIndex.Add("Yriel's Key", 37);
            PoeTradeCurrencyIndex.Add("Inya's Key", 38);
            PoeTradeCurrencyIndex.Add("Volkuur's Key", 39);
            PoeTradeCurrencyIndex.Add("Offering to the Goddess", 40);
            PoeTradeCurrencyIndex.Add("Fragment of the Hydra", 41);
            PoeTradeCurrencyIndex.Add("Fragment of the Phoenix", 42);
            PoeTradeCurrencyIndex.Add("Fragment of the Minotaur", 43);
            PoeTradeCurrencyIndex.Add("Fragment of the Chimera", 44);
            PoeTradeCurrencyIndex.Add("Apprentice Cartographer's Sextant", 45);
            PoeTradeCurrencyIndex.Add("Journeyman Cartographer's Sextant", 46);
            PoeTradeCurrencyIndex.Add("Master Cartographer's Sextant", 47);
            PoeTradeCurrencyIndex.Add("Splinter of Xoph", 52);
            PoeTradeCurrencyIndex.Add("Splinter of Tul", 53);
            PoeTradeCurrencyIndex.Add("Splinter of Esh", 54);
            PoeTradeCurrencyIndex.Add("Splinter of Uul-Netol", 55);
            PoeTradeCurrencyIndex.Add("Splinter of Chayula", 56);
            PoeTradeCurrencyIndex.Add("Blessing of Xoph", 57);
            PoeTradeCurrencyIndex.Add("Blessing of Tul", 58);
            PoeTradeCurrencyIndex.Add("Blessing of Esh", 59);
            PoeTradeCurrencyIndex.Add("Blessing of Uul-Netol", 60);
            PoeTradeCurrencyIndex.Add("Blessing of Chayula", 61);
            PoeTradeCurrencyIndex.Add("Xoph's Breachstone", 62);
            PoeTradeCurrencyIndex.Add("Tul's Breachstone", 63);
            PoeTradeCurrencyIndex.Add("Esh's Breachstone", 64);
            PoeTradeCurrencyIndex.Add("Uul-Netol's Breachstone", 65);
            PoeTradeCurrencyIndex.Add("Chayula's Breachstone", 66);
            PoeTradeCurrencyIndex.Add("Ancient Reliquary Key", 494);
            #endregion
            #region LeagueStone
            PoeTradeCurrencyIndex.Add("Ambush Leaguestone", 495);
            PoeTradeCurrencyIndex.Add("Anarchy Leaguestone", 496);
            PoeTradeCurrencyIndex.Add("Beyond Leaguestone", 497);
            PoeTradeCurrencyIndex.Add("Bloodlines Leaguestone", 498);
            PoeTradeCurrencyIndex.Add("Breach Leaguestone", 499);
            PoeTradeCurrencyIndex.Add("Domination Leaguestone", 500);
            PoeTradeCurrencyIndex.Add("Essence Leaguestone", 501);
            PoeTradeCurrencyIndex.Add("Invasion Leaguestone", 502);
            PoeTradeCurrencyIndex.Add("Nemesis Leaguestone", 503);
            PoeTradeCurrencyIndex.Add("OnSlaught Leaguestone", 504);
            PoeTradeCurrencyIndex.Add("Perandus Leaguestone", 505);
            PoeTradeCurrencyIndex.Add("Prophecy Leaguestone", 506);
            PoeTradeCurrencyIndex.Add("Rampage Leaguestone", 507);
            PoeTradeCurrencyIndex.Add("Talisman Leaguestone", 508);
            PoeTradeCurrencyIndex.Add("Tempest Leaguestone", 509);
            PoeTradeCurrencyIndex.Add("Torment Leaguestone", 510);
            PoeTradeCurrencyIndex.Add("Warbands Leaguestone", 511);
            #endregion
            #region Essence
            PoeTradeCurrencyIndex.Add("Essence of Delirium", 76);
            PoeTradeCurrencyIndex.Add("Essence of Horror", 95);
            PoeTradeCurrencyIndex.Add("Essence of Hysteria", 96);
            PoeTradeCurrencyIndex.Add("Essence of Insanity", 97);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Anger", 67);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Anger", 68);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Anger", 69);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Anguish", 70);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Anguish", 71);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Anguish", 72);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Contempt", 73);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Contempt", 74);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Contempt", 75);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Doubt", 77);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Doubt", 78);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Doubt", 79);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Dread", 80);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Dread", 81);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Dread", 82);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Envy", 83);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Envy", 84);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Envy", 85);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Fear", 86);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Fear", 87);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Fear", 88);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Greed", 89);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Greed", 90);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Greed", 91);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Hatred", 92);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Hatred", 93);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Hatred", 94);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Loathing", 98);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Loathing", 99);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Loathing", 100);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Misery", 101);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Misery", 102);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Misery", 103);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Rage", 104);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Rage", 105);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Rage", 106);
            PoeTradeCurrencyIndex.Add("screaming Essence of Scorn", 107);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Scorn", 108);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Scorn", 109);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Sorrow", 110);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Sorrow", 111);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Sorrow", 112);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Spite", 113);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Spite", 114);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Spite", 115);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Suffering", 116);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Suffering", 117);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Suffering", 118);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Torment", 119);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Torment", 120);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Torment", 121);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Woe", 122);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Woe", 123);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Woe", 124);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Warth", 125);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Warth", 126);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Warth", 127);
            PoeTradeCurrencyIndex.Add("Screaming Essence of Zeal", 128);
            PoeTradeCurrencyIndex.Add("Shrieking Essence of Zeal", 129);
            PoeTradeCurrencyIndex.Add("Deafening Essence of Zeal", 130);
            PoeTradeCurrencyIndex.Add("Remnant of Corruption", 131);
            #endregion
        }

        public double GetPriceInChaos(string ItemBaseNameFriendly)
        {
            CurrencyCache result;

            if (!PriceCache.TryGetValue(ItemBaseNameFriendly, out result))
            {
                if (PoeTradeCurrencyIndex.ContainsKey(ItemBaseNameFriendly))
                    result = DownloadPrice(PoeTradeCurrencyIndex[ItemBaseNameFriendly], PoeTradeCurrencyIndex["Chaos Orb"]);
                else
                {
                    result = new CurrencyCache();
                    result.Value = -1;
                }
                PriceCache.Add(ItemBaseNameFriendly, result);
            }
            if (result.bIsDownloaded)
                return result.Value;
            else
                return 0;
        }

        private CurrencyCache DownloadPrice(int WantItemId, int HaveItemId)
        {
            var url = @"http://currency.poe.trade/search?league=" + League +"&online=x&have=" + HaveItemId + "&want=" + WantItemId;

            CurrencyCache price = new CurrencyCache()
            {
                Url = url,
                bIsDownloaded = false,
                Value = 0,
                topX = TopN,
                CurrencyId = WantItemId
            };
            if (WantItemId == HaveItemId)
            {
                price.bIsDownloaded = true;
                price.Value = 1;
                return price;
            }

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += price.OnGetDownloadedStringCompleted;
                webClient.DownloadStringAsync(new Uri(price.Url));
            }
            catch
            {
                MessageBox.Show("DropStatistics Error processing: Url: " + price.Url,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return price;
        }

        private class CurrencyCache
        {
            public bool bIsDownloaded;
            public string Url;
            public int CurrencyId;
            public double Value;
            public int topX;

            public void OnGetDownloadedStringCompleted(object sender, DownloadStringCompletedEventArgs e)
            {
                var client = ((WebClient)sender);
                client.DownloadStringCompleted -= OnGetDownloadedStringCompleted;
                var contentType = client.ResponseHeaders[HttpResponseHeader.ContentType];
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                if (e.Error == null && contentType != null && contentType == "text/html; charset=utf-8")
                {
                    document.LoadHtml(e.Result);
                    var resultOffers = document.DocumentNode.
                        SelectNodes(string.Format("//*[contains(@class,'{0}')]",
                        "currencyimg cur20-" + CurrencyId));
                    string tmpValue = "";
                    int j = 0;
                    if ( resultOffers == null || resultOffers.Count < topX * 2)
                        return;
                    for (j = 0; j*2 < resultOffers.Count; ++j)
                    {
                        if (j >= topX)
                        {
                            break;
                        }
                        tmpValue = resultOffers[j*2].NextSibling.InnerText.ToString().Split(' ')[2];
                        Value += Convert.ToDouble(tmpValue);
                    }
                    if (j > 0)
                        Value /= j;
                    bIsDownloaded = true;//Due to async processing this must be in the last line
                }
            }

        }
    }
}
