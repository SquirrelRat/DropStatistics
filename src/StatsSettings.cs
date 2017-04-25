using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;

namespace DropStatistics
{
    public sealed class StatsSettings : SettingsBase
    {
        public StatsSettings()
        {
            isCurrencyDisplay = false;
            isMiscDisplay = false;
            isWorthChaos = false;
            isSocketItems = false;
            isRarityItems = false;
            isMiscItems = false;
            isAutoSave = false;
            showUIInTownHO = false;
            shouldDisableWorthChaos = true;
            MiscPositionX = new RangeNode<float>(0f, 0f, 100f);
            MiscPositionY = new RangeNode<float>(10.0f, 0.0f, 100.0f);
            PositionX = new RangeNode<float>(0f, 0f, 100f);
            PositionY = new RangeNode<float>(30f, 0f, 100f);
            XSize = new RangeNode<float>(5f, 0f, 100f);
            imgSize = new RangeNode<float>(30f, 0f, 100f);
            textSize = new RangeNode<int>(15, 0, 100);
            League = new RangeNode<int>(4, 1, 4);
            worthChaos = new RangeNode<int>(0, 0, 10);
            BorderColor = Color.Red;
            ForegroundColor = Color.WhiteSmoke;
        }

        [Menu("Save To File During Game (Performance Issue)", 0)]
        public ToggleNode isAutoSave { get; set; }
        [Menu("Show UI in Town/Hideout", 1)]
        public ToggleNode showUIInTownHO { get; set; }
        [Menu("Disable Worth Chaos Feature", 2)]
        public ToggleNode shouldDisableWorthChaos { get; set; }
        #region CurrencyUI
        [Menu("Display Currency UI",3)]
        public ToggleNode isCurrencyDisplay { get; set; }
        [Menu("HC Tmp/ SC Tmp/ HC Standard / SC Standard",4,3)]
        public RangeNode<int> League { get; set; }
        [Menu("Hide Currencies worth less than X Chaos",5,3)]
        public RangeNode<int> worthChaos { get; set; } 
        [Menu("Position X",6,3)]
        public RangeNode<float> PositionX { get; set; }
        [Menu("Position Y",7,3)]
        public RangeNode<float> PositionY { get; set; }
        [Menu("Box Width",8,3)]
        public RangeNode<float> XSize { get; set; }
        [Menu("Text Size",9,3)]
        public RangeNode<int> textSize { get; set; }
        [Menu("Image Size",10,3)]
        public RangeNode<float> imgSize { get; set; }
        [Menu("Border Color",11,3)]
        public ColorNode BorderColor { get; set; }
        [Menu("Text Color",12,3)]
        public ColorNode ForegroundColor { get; set; }
        #endregion

        #region MiscUI
        [Menu("Display Misc UI", 20)]
        public ToggleNode isMiscDisplay { get; set; }
        [Menu("Position X", 21, 20)]
        public RangeNode<float> MiscPositionX { get; set; }
        [Menu("Position Y", 22, 20)]
        public RangeNode<float> MiscPositionY { get; set; }
        [Menu("Display Worth Chaos", 23, 20)]
        public ToggleNode isWorthChaos { get; set; }
        [Menu("Display Socket Items", 24, 20)]
        public ToggleNode isSocketItems { get; set; }
        [Menu("Display Rarity Items", 25, 20)]
        public ToggleNode isRarityItems { get; set; }
        [Menu("Display Misc Items", 26, 20)]
        public ToggleNode isMiscItems { get; set; }
        #endregion
    }
}
