using DropStatistics.ImageDownloader;
using Newtonsoft.Json;
using PoeHUD.Controllers;
using PoeHUD.Models;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;
using SharpDX;
using System.Collections.Generic;
using System.IO;
using DropStatistics.CurrencyPrice;
using System.Linq;
using System.Threading;

namespace DropStatistics
{
    class StatsCore : BaseSettingsPlugin<StatsSettings>
    {
        private Dictionary<string, ItemInfo> items;
        private PoeImageDownloader imgDwn;
        private CurrencyInChaos currencyValue;
        private RectangleF background;
        private readonly string backupFile = "/backup.json";
        private readonly string backupFile2 = "/backup2.json";
        private HashSet<long> itemsDublicates = null;
        private double TotalValue;
        private string League;
        private uint areaHash;
        private bool isHideout;
        private bool isTown;
        private MiscTrackers miscData;

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable)
                return;
            if ((isHideout || isTown) && !Settings.showUIInTownHO)
                return;

            float X = GameController.Window.GetWindowRectangle().Width * Settings.PositionX.Value * .01f;
            float Y = GameController.Window.GetWindowRectangle().Height * Settings.PositionY.Value * .01f;
            float maxWidth = GameController.Window.GetWindowRectangle().Width * Settings.XSize.Value * .01f;
            Vector2 position = new Vector2(X, Y);
            Size2 textSize = new Size2(0, 0);
            float tmpWidth = 0;
            float maxHeight = Settings.imgSize.Value;
            double tmpTotalValue = TotalValue = 0;
            if (Settings.isCurrencyDisplay && items.Count > 0)
            {
                if (background.Width != 0 && background.Height != 0)
                {
                    Graphics.DrawFrame(background, 5, Settings.BorderColor);
                    Graphics.DrawImage("lightBackground.png", background);
                }

                foreach (var itm in items)
                {
                    if (!Settings.shouldDisableWorthChaos)
                    {
                        tmpTotalValue = currencyValue.GetPriceInChaos(GameController.Files.BaseItemTypes.Translate(itm.Key).BaseName);
                        if (tmpTotalValue * itm.Value.count < Settings.worthChaos.Value)
                            continue;
                        TotalValue += tmpTotalValue * itm.Value.count;
                    }
                    string imgPath = imgDwn.GetImage(itm.Value.ResourcePath);
                    if (string.IsNullOrEmpty(imgPath))
                        continue;

                    if (tmpWidth + Settings.imgSize.Value + textSize.Width >= maxWidth)
                    {
                        position.X = X;
                        tmpWidth = 0;
                        position.Y += Settings.imgSize.Value;
                        maxHeight += Settings.imgSize.Value;
                    }
                    Graphics.DrawPluginImage(imgPath, new RectangleF(position.X, position.Y, Settings.imgSize.Value, Settings.imgSize.Value));
                    position.X += Settings.imgSize.Value;

                    if (!Settings.shouldDisableWorthChaos && tmpTotalValue == 0)
                        textSize = Graphics.DrawText(itm.Value.count.ToString(), Settings.textSize.Value, position, Color.Red);
                    else
                        textSize = Graphics.DrawText(itm.Value.count.ToString(), Settings.textSize.Value, position, Settings.ForegroundColor);

                    position.X += textSize.Width;
                    tmpWidth += Settings.imgSize.Value + textSize.Width;
                }
                background.X = X;
                background.Y = Y;
                background.Width = maxWidth;
                background.Height = maxHeight;
            }
            if (Settings.isMiscDisplay)
            {
                X = GameController.Window.GetWindowRectangle().Width * Settings.MiscPositionX.Value * .01f;
                Y = GameController.Window.GetWindowRectangle().Height * Settings.MiscPositionY.Value * .01f;
                position.X = X;
                position.Y = Y;
                if (Settings.isWorthChaos)
                {
                    textSize = Graphics.DrawText("Worth Chaos:" + TotalValue.ToString(), Settings.textSize.Value, position, Settings.ForegroundColor);
                    position.Y += textSize.Height;
                }
                if (Settings.isSocketItems)
                {
                    textSize = Graphics.DrawText("Total (6L 6S RGB):" +
                        miscData.total6LDrops.ToString() + " " + miscData.total6SDrops.ToString() +
                        " " + miscData.totalRGBDrops.ToString(), Settings.textSize.Value, position,
                        Settings.ForegroundColor);
                    position.Y += textSize.Height;
                }
                if (Settings.isRarityItems)
                {
                    textSize = Graphics.DrawText("Total Items (M R U):" +
                        miscData.totalMagicDrops.ToString() + " " + miscData.totalRareDrops.ToString() +
                        " " + miscData.totalUniqueDrops.ToString(), Settings.textSize.Value, position,
                        Settings.ForegroundColor);
                    position.Y += textSize.Height;
                }
                if (Settings.isMiscItems)
                {
                    foreach (var i in miscData.NormalMiscItems)
                    {
                        textSize = Graphics.DrawText(i.Key + ":" + i.Value.ToString(), Settings.textSize.Value, position, Settings.ForegroundColor);
                        position.Y += textSize.Height;
                    }
                }
            }
        }
        public override void Initialise()
        {
            PluginName = "Drop Statistics";
            base.Initialise();
            switch (Settings.League.Value)
            {
                case 1:
                    League = "Standard";
                    break;
                case 2:
                    League = "Hardcore";
                    break;
                case 3:
                    League = "Hardcore+Bestiary";
                    break;
                default:
                    League = "Bestiary";
                    break;
            }
            currencyValue = new CurrencyInChaos(League);
            TotalValue = 0;
            isHideout = false;
            isTown = true;
            areaHash = 0;
            imgDwn = new PoeImageDownloader(LocalPluginDirectory.Replace("\\","/"));
            background = new RectangleF();
            itemsDublicates = new HashSet<long>();
            if (File.Exists(LocalPluginDirectory.Replace("\\", "/") + backupFile) )
            {
                var data = File.ReadAllText(LocalPluginDirectory.Replace("\\", "/") + backupFile);
                items = JsonConvert.DeserializeObject<Dictionary<string, ItemInfo>>(data);
            } else
            {
                items = new Dictionary<string, ItemInfo>();
            }
            if (File.Exists(LocalPluginDirectory.Replace("\\","/") + backupFile2))
            {
                var data = File.ReadAllText(LocalPluginDirectory.Replace("\\", "/") + backupFile2);
                miscData = JsonConvert.DeserializeObject<MiscTrackers>(data);
            } else
            {
                miscData = new MiscTrackers();
                miscData.NormalMiscItems.Add("SampleBaseName1", 0);
                miscData.NormalMiscItems.Add("SampleBaseName2", 0);
            }
            OnPluginToggle();
            Settings.Enable.OnValueChanged += OnPluginToggle;
        }
        public void OnPluginToggle()
        {
            if (Settings.Enable.Value)
            {
                GameController.Area.OnAreaChange += OnAreaChange;
            }
            else
            {
                GameController.Area.OnAreaChange -= OnAreaChange;
            }
        }
        private void WriteToFile()
        {
            string data = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(LocalPluginDirectory.Replace("\\", "/") + backupFile, data);

            data = JsonConvert.SerializeObject(miscData, Formatting.Indented);
            File.WriteAllText(LocalPluginDirectory.Replace("\\", "/") + backupFile2, data);
        }
        public override void OnClose()
        {
            base.OnClose();
            WriteToFile();
        }

        public void OnAreaChange(AreaController area)
        {
            if (Settings.Enable)
            {
                isHideout = area.CurrentArea.IsHideout;
                isTown = area.CurrentArea.IsTown;
                if (!isHideout && !isTown)
                {
                    if (area.CurrentArea.Hash != areaHash)
                    {
                        LogMessage("New Area/Instance Detected. Configuring DropStatistic Plugin.", 4);
                        itemsDublicates.Clear();
                        areaHash = area.CurrentArea.Hash;
                        if (Settings.resetOnAreaChange.Value)
                        {
                            items.Clear();
                            miscData.NormalMiscItems.Clear();
                            miscData.total6LDrops = 0;
                            miscData.total6SDrops = 0;
                            miscData.totalMagicDrops = 0;
                            miscData.totalRareDrops = 0;
                            miscData.totalRGBDrops = 0;
                            miscData.totalUniqueDrops = 0;
                        }
                    }
                }
            }
        }

        public void handleCurrency(EntityWrapper entityWrapper)
        {
            BaseItemType ItmBase = null;
            ItmBase = GameController.Files.BaseItemTypes.Translate(entityWrapper.GetComponent<WorldItem>().ItemEntity.Path);
            if (ItmBase == null || !(ItmBase.ClassName.Contains("Currency") || ItmBase.ClassName.Contains("Leaguestone") ||
                ItmBase.ClassName.Contains("Fragment") || ItmBase.ClassName.Contains("MiscMapItem")))
                return;

            var path = entityWrapper.GetComponent<WorldItem>().ItemEntity.Path;
            var resourcePath = entityWrapper.GetComponent<WorldItem>().ItemEntity.GetComponent<RenderItem>().ResourcePath;
            ItemInfo tmp = new ItemInfo()
            {
                count = 1,
                ResourcePath = resourcePath,
            };

            if (items.ContainsKey(path))
                items[path].count++;
            else
                items.Add(path, tmp);
        }
        public void handleSockets(EntityWrapper entityWrapper)
        {
            var socket = entityWrapper.GetComponent<WorldItem>().ItemEntity.GetComponent<Sockets>();
            if (socket.LargestLinkSize == 6)
                miscData.total6LDrops++;
            else if (socket.NumberOfSockets == 6)
                miscData.total6SDrops++;
            else if (IsContainSocketGroup(socket.SocketGroup, "RGB"))
                miscData.totalRGBDrops++;
        }
        private bool IsContainSocketGroup(List<string> socketGroup, string str)
        {
            str = string.Concat(str.OrderBy(y => y)); return
                socketGroup.Select(group => string.Concat(group.OrderBy(y => y)))
                    .Any(sortedGroup => sortedGroup.Contains(str));
        }
        public void handleRareUniqueMisc(EntityWrapper entityWrapper)
        {
            var mods = entityWrapper.GetComponent<WorldItem>().ItemEntity.GetComponent<Mods>();
            var baseName = GameController.Files.BaseItemTypes.Translate(entityWrapper.GetComponent<WorldItem>().ItemEntity.Path).BaseName;
            switch (mods.ItemRarity)
            {
                case PoeHUD.Models.Enums.ItemRarity.Magic:
                    miscData.totalMagicDrops++;
                    break;
                case PoeHUD.Models.Enums.ItemRarity.Rare:
                    miscData.totalRareDrops++;
                    break;
                case PoeHUD.Models.Enums.ItemRarity.Unique:
                    miscData.totalUniqueDrops++;
                    break;
                default:
                    if (miscData.NormalMiscItems.ContainsKey(baseName))
                        miscData.NormalMiscItems[baseName]++;
                    break;
            }
        }
        public override void EntityAdded(EntityWrapper entityWrapper)
        {
            base.EntityAdded(entityWrapper);
            if (isTown || isHideout || !Settings.Enable)
                return;

            if (itemsDublicates.Contains(entityWrapper.Id))
                return;
            else
                itemsDublicates.Add(entityWrapper.Id);

            if (entityWrapper.HasComponent<WorldItem>() && entityWrapper.GetComponent<WorldItem>().ItemEntity.HasComponent<RenderItem>())
            {
                handleCurrency(entityWrapper);
            }
            if (entityWrapper.HasComponent<WorldItem>() && entityWrapper.GetComponent<WorldItem>().ItemEntity.HasComponent<Sockets>())
            {
                handleSockets(entityWrapper);
            }
            if (entityWrapper.HasComponent<WorldItem>() && entityWrapper.GetComponent<WorldItem>().ItemEntity.HasComponent<Mods>())
            {
                handleRareUniqueMisc(entityWrapper);
            }
            if (Settings.isAutoSave)
            {
                WriteToFile();
            }
        }

        private class MiscTrackers
        {
//            public long normalKills = 0;
//            public long magicKills = 0;
//            public long rareKills = 0;
//            public long uniqueKills = 0;

            public Dictionary<string, long> NormalMiscItems = new Dictionary<string, long>();
            public long totalMagicDrops = 0;
            public long totalRareDrops = 0;
            public long totalUniqueDrops = 0;

            public long total6SDrops = 0;
            public long totalRGBDrops = 0;
            public long total6LDrops = 0;
        }
    }
}
