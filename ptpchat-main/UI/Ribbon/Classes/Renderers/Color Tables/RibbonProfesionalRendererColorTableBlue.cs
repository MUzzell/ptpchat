namespace PtpChat.Main.Ribbon.Classes.Renderers.Color_Tables
{
    using System;
    using System.Drawing;
    using System.Globalization;

    public class RibbonProfesionalRendererColorTableBlue : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableBlue()
        {
            // Rebuild the solution for the first time
            // for this ColorTable to take effect.
            // Guide for applying new theme: http://officeribbon.codeplex.com/wikipage?title=How%20to%20Make%20a%20New%20Theme%2c%20Skin%20for%20this%20Ribbon%20Programmatically

            this.ThemeName = "Blue";
            this.ThemeAuthor = "Michael Spradlin";
            this.ThemeAuthorWebsite = "";
            this.ThemeAuthorEmail = "";
            this.ThemeDateCreated = "08/26/2013";

            this.OrbDropDownDarkBorder = this.FromHex("#9BAFCA");
            this.OrbDropDownLightBorder = this.FromHex("#FFFFFF");
            this.OrbDropDownBack = this.FromHex("#BFD3EB");
            this.OrbDropDownNorthA = this.FromHex("#D7E5F7");
            this.OrbDropDownNorthB = this.FromHex("#D4E1F3");
            this.OrbDropDownNorthC = this.FromHex("#C6D8EE");
            this.OrbDropDownNorthD = this.FromHex("#B7CAE6");
            this.OrbDropDownSouthC = this.FromHex("#B0C9EA");
            this.OrbDropDownSouthD = this.FromHex("#CFE0F5");
            this.OrbDropDownContentbg = this.FromHex("#E9EAEE");
            this.OrbDropDownContentbglight = this.FromHex("#FAFAFA");
            this.OrbDropDownSeparatorlight = this.FromHex("#F5F5F5");
            this.OrbDropDownSeparatordark = this.FromHex("#C5C5C5");
            this.Caption1 = this.FromHex("#E3EBF6");
            this.Caption2 = this.FromHex("#DAE9FD");
            this.Caption3 = this.FromHex("#D5E5FA");
            this.Caption4 = this.FromHex("#D9E7F9");
            this.Caption5 = this.FromHex("#CADEF7");
            this.Caption6 = this.FromHex("#E4EFFD");
            this.Caption7 = this.FromHex("#B0CFF7");
            this.QuickAccessBorderDark = this.FromHex("#B6CAE2");
            this.QuickAccessBorderLight = this.FromHex("#F2F6FB");
            this.QuickAccessUpper = this.FromHex("#E0EBF9");
            this.QuickAccessLower = this.FromHex("#C9D9EE");
            this.OrbOptionBorder = this.FromHex("#7793B9");
            this.OrbOptionBackground = this.FromHex("#E8F1FC");
            this.OrbOptionShine = this.FromHex("#D2E1F4");
            this.Arrow = this.FromHex("#678CBD");
            this.ArrowLight = this.FromHex("#FFFFFF");
            this.ArrowDisabled = this.FromHex("#B7B7B7");
            this.Text = this.FromHex("#15428B");
            this.RibbonBackground = this.FromHex("#BED0E8");
            this.TabBorder = this.FromHex("#8DB2E3");
            this.TabNorth = this.FromHex("#EBF3FE");
            this.TabSouth = this.FromHex("#E1EAF6");
            this.TabGlow = this.FromHex("#D1FBFF");
            this.TabText = this.FromHex("#15428B");
            this.TabActiveText = this.FromHex("#15428B");
            this.TabContentNorth = this.FromHex("#C8D9ED");
            this.TabContentSouth = this.FromHex("#E7F2FF");
            this.TabSelectedGlow = this.FromHex("#E1D2A5");
            this.PanelDarkBorder = this.FromHex("#15428B");
            this.PanelLightBorder = this.FromHex("#FFFFFF");
            this.PanelTextBackground = this.FromHex("#C2D9F0");
            this.PanelTextBackgroundSelected = this.FromHex("#C2D9F0");
            this.PanelText = this.FromHex("#15428B");
            this.PanelBackgroundSelected = this.FromHex("#E8FFFD");
            this.PanelOverflowBackground = this.FromHex("#B9D1F0");
            this.PanelOverflowBackgroundPressed = this.FromHex("#7699C8");
            this.PanelOverflowBackgroundSelectedNorth = this.FromHex("#FFFFFF");
            this.PanelOverflowBackgroundSelectedSouth = this.FromHex("#B8D7FD");
            this.ButtonBgOut = this.FromHex("#C1D5F1");
            this.ButtonBgCenter = this.FromHex("#CFE0F7");
            this.ButtonBorderOut = this.FromHex("#B9D0ED");
            this.ButtonBorderIn = this.FromHex("#E3EDFB");
            this.ButtonGlossyNorth = this.FromHex("#DEEBFE");
            this.ButtonGlossySouth = this.FromHex("#CBDEF6");
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
            this.ItemGroupOuterBorder = this.FromHex("#9EBAE1");
            this.ItemGroupInnerBorder = this.FromHex("#FFFFFF");
            this.ItemGroupSeparatorLight = this.FromHex("#FFFFFF");
            this.ItemGroupSeparatorDark = this.FromHex("#9EBAE1");
            this.ItemGroupBgNorth = this.FromHex("#CADCF0");
            this.ItemGroupBgSouth = this.FromHex("#D0E1F7");
            this.ItemGroupBgGlossy = this.FromHex("#BCD0E9");
            this.ButtonListBorder = this.FromHex("#B9D0ED");
            this.ButtonListBg = this.FromHex("#D4E6F8");
            this.ButtonListBgSelected = this.FromHex("#ECF3FB");
            this.DropDownBg = this.FromHex("#FAFAFA");
            this.DropDownImageBg = this.FromHex("#E9EEEE");
            this.DropDownImageSeparator = this.FromHex("#C5C5C5");
            this.DropDownBorder = this.FromHex("#868686");
            this.DropDownGripNorth = this.FromHex("#FFFFFF");
            this.DropDownGripSouth = this.FromHex("#DFE9EF");
            this.DropDownGripBorder = this.FromHex("#DDE7EE");
            this.DropDownGripDark = this.FromHex("#5574A7");
            this.DropDownGripLight = this.FromHex("#FFFFFF");
            this.SeparatorLight = this.FromHex("#FAFBFD");
            this.SeparatorDark = this.FromHex("#96B4DA");
            this.SeparatorBg = this.FromHex("#DAE6EE");
            this.SeparatorLine = this.FromHex("#C5C5C5");
            this.TextBoxUnselectedBg = this.FromHex("#EAF2FB");
            this.TextBoxBorder = this.FromHex("#ABC1DE");
            this.ToolTipContentNorth = this.FromHex("#FAFCFE");
            this.ToolTipContentSouth = this.FromHex("#CEDCF1");
            this.ToolTipDarkBorder = this.FromHex("#A9A9A9");
            this.ToolTipLightBorder = this.FromHex("#FFFFFF");
            this.ToolStripItemTextPressed = this.FromHex("#444444");
            this.ToolStripItemTextSelected = this.FromHex("#444444");
            this.ToolStripItemText = this.FromHex("#444444");
            this.clrVerBG_Shadow = this.FromHex("#FFFFFF");
            this.ButtonPressed_2013 = this.FromHex("#92C0E0");
            this.ButtonSelected_2013 = this.FromHex("#CDE6F7");
            this.OrbButton_2013 = this.FromHex("#0072C6");
            this.OrbButtonSelected_2013 = this.FromHex("#2A8AD4");
            this.OrbButtonPressed_2013 = this.FromHex("#2A8AD4");
            this.TabText_2013 = this.FromHex("#0072C6");
            this.TabTextSelected_2013 = this.FromHex("#444444");
            this.PanelBorder_2013 = this.FromHex("#15428B");
            this.RibbonBackground_2013 = this.FromHex("#FFFFFF");
            this.TabCompleteBackground_2013 = this.FromHex("#FFFFFF");
            this.TabNormalBackground_2013 = this.FromHex("#FFFFFF");
            this.TabActiveBackbround_2013 = this.FromHex("#FFFFFF");
            this.TabBorder_2013 = this.FromHex("#D4D4D4");
            this.TabCompleteBorder_2013 = this.FromHex("#D4D4D4");
            this.TabActiveBorder_2013 = this.FromHex("#D4D4D4");
            this.OrbButtonText_2013 = this.FromHex("#FFFFFF");
            this.PanelText_2013 = this.FromHex("#666666");
            this.RibbonItemText_2013 = this.FromHex("#444444");
            this.ToolTipText_2013 = this.FromHex("#262626");
            this.ToolStripItemTextPressed_2013 = this.FromHex("#444444");
            this.ToolStripItemTextSelected_2013 = this.FromHex("#444444");
            this.ToolStripItemText_2013 = this.FromHex("#444444");
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