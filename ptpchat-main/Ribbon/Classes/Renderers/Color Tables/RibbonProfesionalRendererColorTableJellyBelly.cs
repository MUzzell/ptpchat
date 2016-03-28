namespace PtpChat.Main.Ribbon.Classes.Renderers.Color_Tables
{
    using System;
    using System.Drawing;
    using System.Globalization;

    public class RibbonProfesionalRendererColorTableJellyBelly : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableJellyBelly()
        {
            // Rebuild the solution for the first time
            // for this ColorTable to take effect.
            // Guide for applying new theme: http://officeribbon.codeplex.com/wikipage?title=How%20to%20Make%20a%20New%20Theme%2c%20Skin%20for%20this%20Ribbon%20Programmatically

            this.ThemeName = "JellyBelly";
            this.ThemeAuthor = "Michael Spradlin";
            this.ThemeAuthorWebsite = "";
            this.ThemeAuthorEmail = "";
            this.ThemeDateCreated = "08/23/2013";

            this.OrbDropDownDarkBorder = this.FromHex("#282828");
            this.OrbDropDownLightBorder = this.FromHex("#484848");
            this.OrbDropDownBack = this.FromHex("#282828");
            this.OrbDropDownNorthA = this.FromHex("#282828");
            this.OrbDropDownNorthB = this.FromHex("#282828");
            this.OrbDropDownNorthC = this.FromHex("#282828");
            this.OrbDropDownNorthD = this.FromHex("#282828");
            this.OrbDropDownSouthC = this.FromHex("#484848");
            this.OrbDropDownSouthD = this.FromHex("#282828");
            this.OrbDropDownContentbg = this.FromHex("#8D8D8D");
            this.OrbDropDownContentbglight = this.FromHex("#B1B1B1");
            this.OrbDropDownSeparatorlight = this.FromHex("#F5F5F5");
            this.OrbDropDownSeparatordark = this.FromHex("#C5C5C5");
            this.Caption1 = this.FromHex("#484848");
            this.Caption2 = this.FromHex("#484848");
            this.Caption3 = this.FromHex("#484848");
            this.Caption4 = this.FromHex("#484848");
            this.Caption5 = this.FromHex("#484848");
            this.Caption6 = this.FromHex("#484848");
            this.Caption7 = this.FromHex("#484848");
            this.QuickAccessBorderDark = this.FromHex("#8D8D8D");
            this.QuickAccessBorderLight = this.FromHex("#B1B1B1");
            this.QuickAccessUpper = this.FromHex("#282828");
            this.QuickAccessLower = this.FromHex("#282828");
            this.OrbOptionBorder = this.FromHex("#8D8D8D");
            this.OrbOptionBackground = this.FromHex("#8D8D8D");
            this.OrbOptionShine = this.FromHex("#8D8D8D");
            this.Arrow = this.FromHex("#282828");
            this.ArrowLight = this.FromHex("#FFFFFF");
            this.ArrowDisabled = this.FromHex("#B7B7B7");
            this.Text = this.FromHex("#EBEBEB");
            this.RibbonBackground = this.FromHex("#282828");
            this.TabBorder = this.FromHex("#B1B1B1");
            this.TabNorth = this.FromHex("#B1B1B1");
            this.TabSouth = this.FromHex("#8D8D8D");
            this.TabGlow = this.FromHex("#B1B1B1");
            this.TabText = this.FromHex("#EBEBEB");
            this.TabActiveText = this.FromHex("#EBEBEB");
            this.TabContentNorth = this.FromHex("#484848");
            this.TabContentSouth = this.FromHex("#484848");
            this.TabSelectedGlow = this.FromHex("#30B4E4");
            this.PanelDarkBorder = this.FromHex("#282828");
            this.PanelLightBorder = this.FromHex("#FFFFFF");
            this.PanelTextBackground = this.FromHex("#B7B7B7");
            this.PanelTextBackgroundSelected = this.FromHex("#B7B7B7");
            this.PanelText = this.FromHex("#282828");
            this.PanelBackgroundSelected = this.FromHex("#757575");
            this.PanelOverflowBackground = this.FromHex("#B1B1B1");
            this.PanelOverflowBackgroundPressed = this.FromHex("#B1B1B1");
            this.PanelOverflowBackgroundSelectedNorth = this.FromHex("#757575");
            this.PanelOverflowBackgroundSelectedSouth = this.FromHex("#B1B1B1");
            this.ButtonBgOut = this.FromHex("#B1B1B1");
            this.ButtonBgCenter = this.FromHex("#B1B1B1");
            this.ButtonBorderOut = this.FromHex("#282828");
            this.ButtonBorderIn = this.FromHex("#B1B1B1");
            this.ButtonGlossyNorth = this.FromHex("#B1B1B1");
            this.ButtonGlossySouth = this.FromHex("#B1B1B1");
            this.ButtonDisabledBgOut = this.FromHex("#E0E0E0");
            this.ButtonDisabledBgCenter = this.FromHex("#E0E0E0");
            this.ButtonDisabledBorderOut = this.FromHex("#F1F3F5");
            this.ButtonDisabledBorderIn = this.FromHex("#F1F3F5");
            this.ButtonDisabledGlossyNorth = this.FromHex("#E0E0E0");
            this.ButtonDisabledGlossySouth = this.FromHex("#E0E0E0");
            this.ButtonSelectedBgOut = this.FromHex("#30B4E4");
            this.ButtonSelectedBgCenter = this.FromHex("#30B4E4");
            this.ButtonSelectedBorderOut = this.FromHex("#0B4956");
            this.ButtonSelectedBorderIn = this.FromHex("#3A8DB5");
            this.ButtonSelectedGlossyNorth = this.FromHex("#30B4E4");
            this.ButtonSelectedGlossySouth = this.FromHex("#30B4E4");
            this.ButtonPressedBgOut = this.FromHex("#0B4956");
            this.ButtonPressedBgCenter = this.FromHex("#0B4956");
            this.ButtonPressedBorderOut = this.FromHex("#3A8DB5");
            this.ButtonPressedBorderIn = this.FromHex("#3A8DB5");
            this.ButtonPressedGlossyNorth = this.FromHex("#0B4956");
            this.ButtonPressedGlossySouth = this.FromHex("#0B4956");
            this.ButtonCheckedBgOut = this.FromHex("#0B4956");
            this.ButtonCheckedBgCenter = this.FromHex("#0B4956");
            this.ButtonCheckedBorderOut = this.FromHex("#3A8DB5");
            this.ButtonCheckedBorderIn = this.FromHex("#3A8DB5");
            this.ButtonCheckedGlossyNorth = this.FromHex("#0B4956");
            this.ButtonCheckedGlossySouth = this.FromHex("#0B4956");
            this.ItemGroupOuterBorder = this.FromHex("#282828");
            this.ItemGroupInnerBorder = this.FromHex("#FFFFFF");
            this.ItemGroupSeparatorLight = this.FromHex("#FFFFFF");
            this.ItemGroupSeparatorDark = this.FromHex("#9EBAE1");
            this.ItemGroupBgNorth = this.FromHex("#C5C5C5");
            this.ItemGroupBgSouth = this.FromHex("#C5C5C5");
            this.ItemGroupBgGlossy = this.FromHex("#C5C5C5");
            this.ButtonListBorder = this.FromHex("#F1F3F5");
            this.ButtonListBg = this.FromHex("#484848");
            this.ButtonListBgSelected = this.FromHex("#757575");
            this.DropDownBg = this.FromHex("#8D8D8D");
            this.DropDownImageBg = this.FromHex("#E9EEEE");
            this.DropDownImageSeparator = this.FromHex("#C5C5C5");
            this.DropDownBorder = this.FromHex("#868686");
            this.DropDownGripNorth = this.FromHex("#FFFFFF");
            this.DropDownGripSouth = this.FromHex("#C5C5C5");
            this.DropDownGripBorder = this.FromHex("#C5C5C5");
            this.DropDownGripDark = this.FromHex("#484848");
            this.DropDownGripLight = this.FromHex("#C5C5C5");
            this.SeparatorLight = this.FromHex("#FAFBFD");
            this.SeparatorDark = this.FromHex("#484848");
            this.SeparatorBg = this.FromHex("#C5C5C5");
            this.SeparatorLine = this.FromHex("#C5C5C5");
            this.TextBoxUnselectedBg = this.FromHex("#C5C5C5");
            this.TextBoxBorder = this.FromHex("#FFFFFF");
            this.ToolTipContentNorth = this.FromHex("#B7B7B7");
            this.ToolTipContentSouth = this.FromHex("#B7B7B7");
            this.ToolTipDarkBorder = this.FromHex("#484848");
            this.ToolTipLightBorder = this.FromHex("#484848");
            this.ToolStripItemTextPressed = this.FromHex("#262626");
            this.ToolStripItemTextSelected = this.FromHex("#262626");
            this.ToolStripItemText = this.FromHex("#FFFFFF");
            this.clrVerBG_Shadow = this.FromHex("#FFFFFF");
            this.ButtonPressed_2013 = this.FromHex("#2A8AD4");
            this.ButtonSelected_2013 = this.FromHex("#92C0E0");
            this.OrbButton_2013 = this.FromHex("#333333");
            this.OrbButtonSelected_2013 = this.FromHex("#2A8AD4");
            this.OrbButtonPressed_2013 = this.FromHex("#2A8AD4");
            this.TabText_2013 = this.FromHex("#0072C6");
            this.TabTextSelected_2013 = this.FromHex("#FFFFFF");
            this.PanelBorder_2013 = this.FromHex("#15428B");
            this.RibbonBackground_2013 = this.FromHex("#484848");
            this.TabCompleteBackground_2013 = this.FromHex("#F3F3F3");
            this.TabNormalBackground_2013 = this.FromHex("#484848");
            this.TabActiveBackbround_2013 = this.FromHex("#F3F3F3");
            this.TabBorder_2013 = this.FromHex("#ABABAB");
            this.TabCompleteBorder_2013 = this.FromHex("#ABABAB");
            this.TabActiveBorder_2013 = this.FromHex("#ABABAB");
            this.OrbButtonText_2013 = this.FromHex("#FFFFFF");
            this.PanelText_2013 = this.FromHex("#262626");
            this.RibbonItemText_2013 = this.FromHex("#262626");
            this.ToolTipText_2013 = this.FromHex("#262626");
            this.ToolStripItemTextPressed_2013 = this.FromHex("#262626");
            this.ToolStripItemTextSelected_2013 = this.FromHex("#262626");
            this.ToolStripItemText_2013 = this.FromHex("#FFFFFF");
        }

        public Color FromHex(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            if (hex.Length != 6)
            {
                throw new Exception("Color not valid");
            }

            return Color.FromArgb(
                int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));
        }
    }
}