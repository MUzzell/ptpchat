namespace PtpChat.Main.Ribbon.Classes.Renderers.Color_Tables
{
    using System;
    using System.Drawing;
    using System.Globalization;

    public class RibbonProfesionalRendererColorTableGreen : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableGreen()
        {
            // Rebuild the solution for the first time
            // for this ColorTable to take effect.
            // Guide for applying new theme: http://officeribbon.codeplex.com/wikipage?title=How%20to%20Make%20a%20New%20Theme%2c%20Skin%20for%20this%20Ribbon%20Programmatically

            this.ThemeName = "Green";
            this.ThemeAuthor = "Michael Spradlin";
            this.ThemeAuthorWebsite = "";
            this.ThemeAuthorEmail = "";
            this.ThemeDateCreated = "08/26/2013";

            this.OrbDropDownDarkBorder = this.FromHex("#B1B1B1");
            this.OrbDropDownLightBorder = this.FromHex("#FFFFFF");
            this.OrbDropDownBack = this.FromHex("#D4D4D4");
            this.OrbDropDownNorthA = this.FromHex("#E6E6E6");
            this.OrbDropDownNorthB = this.FromHex("#E2E2E2");
            this.OrbDropDownNorthC = this.FromHex("#D9D9D9");
            this.OrbDropDownNorthD = this.FromHex("#CDCDCD");
            this.OrbDropDownSouthC = this.FromHex("#CBCBCB");
            this.OrbDropDownSouthD = this.FromHex("#E1E1E1");
            this.OrbDropDownContentbg = this.FromHex("#EBEBEB");
            this.OrbDropDownContentbglight = this.FromHex("#FAFAFA");
            this.OrbDropDownSeparatorlight = this.FromHex("#F5F5F5");
            this.OrbDropDownSeparatordark = this.FromHex("#C5C5C5");
            this.Caption1 = this.FromHex("#24CE2C");
            this.Caption2 = this.FromHex("#22C62A");
            this.Caption3 = this.FromHex("#21BD28");
            this.Caption4 = this.FromHex("#1FB526");
            this.Caption5 = this.FromHex("#21BD28");
            this.Caption6 = this.FromHex("#22C62A");
            this.Caption7 = this.FromHex("#24CE2C");
            this.QuickAccessBorderDark = this.FromHex("#CBCBCB");
            this.QuickAccessBorderLight = this.FromHex("#F6F6F6");
            this.QuickAccessUpper = this.FromHex("#ECECEC");
            this.QuickAccessLower = this.FromHex("#DADADA");
            this.OrbOptionBorder = this.FromHex("#969696");
            this.OrbOptionBackground = this.FromHex("#F1F1F1");
            this.OrbOptionShine = this.FromHex("#E2E2E2");
            this.Arrow = this.FromHex("#7C7C7C");
            this.ArrowLight = this.FromHex("#EAF2F9");
            this.ArrowDisabled = this.FromHex("#7C7C7C");
            this.Text = this.FromHex("#000000");
            this.RibbonBackground = this.FromHex("#85BF88");
            this.TabBorder = this.FromHex("#A2FBA7");
            this.TabNorth = this.FromHex("#A2FBA7");
            this.TabSouth = this.FromHex("#97EB9B");
            this.TabGlow = this.FromHex("#D1FBFF");
            this.TabText = this.FromHex("#FFFFFF");
            this.TabActiveText = this.FromHex("#000000");
            this.TabContentNorth = this.FromHex("#A2FBA7");
            this.TabContentSouth = this.FromHex("#97EB9B");
            this.TabSelectedGlow = this.FromHex("#E1D2A5");
            this.PanelDarkBorder = this.FromHex("#DCF5DD");
            this.PanelLightBorder = this.FromHex("#E7E9ED");
            this.PanelTextBackground = this.FromHex("#85BF88");
            this.PanelTextBackgroundSelected = this.FromHex("#85BF88");
            this.PanelText = this.FromHex("#FFFFFF");
            this.PanelBackgroundSelected = this.FromHex("#C3FFC2");
            this.PanelOverflowBackground = this.FromHex("#B9D1F0");
            this.PanelOverflowBackgroundPressed = this.FromHex("#AAAEB3");
            this.PanelOverflowBackgroundSelectedNorth = this.FromHex("#FFFFFF");
            this.PanelOverflowBackgroundSelectedSouth = this.FromHex("#EBEBEB");
            this.ButtonBgOut = this.FromHex("#B3C1B4");
            this.ButtonBgCenter = this.FromHex("#CDD2D8");
            this.ButtonBorderOut = this.FromHex("#A9B1B8");
            this.ButtonBorderIn = this.FromHex("#DFE2E6");
            this.ButtonGlossyNorth = this.FromHex("#DBDFE4");
            this.ButtonGlossySouth = this.FromHex("#DFE2E8");
            this.ButtonDisabledBgOut = this.FromHex("#E0E4E8");
            this.ButtonDisabledBgCenter = this.FromHex("#E8EBEF");
            this.ButtonDisabledBorderOut = this.FromHex("#C5D1DE");
            this.ButtonDisabledBorderIn = this.FromHex("#F1F3F5");
            this.ButtonDisabledGlossyNorth = this.FromHex("#F0F3F6");
            this.ButtonDisabledGlossySouth = this.FromHex("#EAEDF1");
            this.ButtonSelectedBgOut = this.FromHex("#FFD646");
            this.ButtonSelectedBgCenter = this.FromHex("#FFEAAC");
            this.ButtonSelectedBorderOut = this.FromHex("#C2A978");
            this.ButtonSelectedBorderIn = this.FromHex("#FFF2C7");
            this.ButtonSelectedGlossyNorth = this.FromHex("#FFFDDB");
            this.ButtonSelectedGlossySouth = this.FromHex("#FFE793");
            this.ButtonPressedBgOut = this.FromHex("#F88F2C");
            this.ButtonPressedBgCenter = this.FromHex("#FDF1B0");
            this.ButtonPressedBorderOut = this.FromHex("#8E8165");
            this.ButtonPressedBorderIn = this.FromHex("#F9C65A");
            this.ButtonPressedGlossyNorth = this.FromHex("#FDD5A8");
            this.ButtonPressedGlossySouth = this.FromHex("#FBB062");
            this.ButtonCheckedBgOut = this.FromHex("#F9AA45");
            this.ButtonCheckedBgCenter = this.FromHex("#FDEA9D");
            this.ButtonCheckedBorderOut = this.FromHex("#8E8165");
            this.ButtonCheckedBorderIn = this.FromHex("#F9C65A");
            this.ButtonCheckedGlossyNorth = this.FromHex("#F8DBB7");
            this.ButtonCheckedGlossySouth = this.FromHex("#FED18E");
            this.ItemGroupOuterBorder = this.FromHex("#ADB7BB");
            this.ItemGroupInnerBorder = this.FromHex("#FFFFFF");
            this.ItemGroupSeparatorLight = this.FromHex("#FFFFFF");
            this.ItemGroupSeparatorDark = this.FromHex("#ADB7BB");
            this.ItemGroupBgNorth = this.FromHex("#D9E0E1");
            this.ItemGroupBgSouth = this.FromHex("#EDF0F1");
            this.ItemGroupBgGlossy = this.FromHex("#D2D9DB");
            this.ButtonListBorder = this.FromHex("#ACACAC");
            this.ButtonListBg = this.FromHex("#DAE2E2");
            this.ButtonListBgSelected = this.FromHex("#F7F7F7");
            this.DropDownBg = this.FromHex("#FAFAFA");
            this.DropDownImageBg = this.FromHex("#E9EEEE");
            this.DropDownImageSeparator = this.FromHex("#C5C5C5");
            this.DropDownBorder = this.FromHex("#868686");
            this.DropDownGripNorth = this.FromHex("#FFFFFF");
            this.DropDownGripSouth = this.FromHex("#DFE9EF");
            this.DropDownGripBorder = this.FromHex("#DDE7EE");
            this.DropDownGripDark = this.FromHex("#5574A7");
            this.DropDownGripLight = this.FromHex("#FFFFFF");
            this.SeparatorLight = this.FromHex("#E6E8EB");
            this.SeparatorDark = this.FromHex("#C5C5C5");
            this.SeparatorBg = this.FromHex("#EBEBEB");
            this.SeparatorLine = this.FromHex("#C5C5C5");
            this.TextBoxUnselectedBg = this.FromHex("#E8E8E8");
            this.TextBoxBorder = this.FromHex("#898989");
            this.ToolTipContentNorth = this.FromHex("#FAFCFE");
            this.ToolTipContentSouth = this.FromHex("#CEDCF1");
            this.ToolTipDarkBorder = this.FromHex("#A9A9A9");
            this.ToolTipLightBorder = this.FromHex("#FFFFFF");
            this.ToolStripItemTextPressed = this.FromHex("#262626");
            this.ToolStripItemTextSelected = this.FromHex("#262626");
            this.ToolStripItemText = this.FromHex("#FFFFFF");
            this.clrVerBG_Shadow = this.FromHex("#FFFFFF");
            this.ButtonPressed_2013 = this.FromHex("#92C0E0");
            this.ButtonSelected_2013 = this.FromHex("#CDE6F7");
            this.OrbButton_2013 = this.FromHex("#333333");
            this.OrbButtonSelected_2013 = this.FromHex("#2A8AD4");
            this.OrbButtonPressed_2013 = this.FromHex("#2A8AD4");
            this.TabText_2013 = this.FromHex("#0072C6");
            this.TabTextSelected_2013 = this.FromHex("#262626");
            this.PanelBorder_2013 = this.FromHex("#15428B");
            this.RibbonBackground_2013 = this.FromHex("#85BF88");
            this.TabCompleteBackground_2013 = this.FromHex("#F3F3F3");
            this.TabNormalBackground_2013 = this.FromHex("#85BF88");
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