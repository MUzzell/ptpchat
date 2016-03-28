namespace PtpChat.Main.Ribbon.Classes.Renderers.Color_Tables
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class RibbonProfesionalRendererColorTable
    {
        #region Theme Information

        public string ThemeName { get; set; }

        public string ThemeAuthor { get; set; }

        public string ThemeAuthorEmail { get; set; }

        public string ThemeAuthorWebsite { get; set; }

        public string ThemeDateCreated { get; set; }

        #endregion

        #region Pendent for black

        public Color FormBorder = FromHexStr("#3B5A82");

        public Color OrbDropDownDarkBorder = Color.FromArgb(0x9b, 0xaf, 0xca);

        public Color OrbDropDownLightBorder = Color.FromArgb(0xff, 0xff, 0xff);

        public Color OrbDropDownBack = Color.FromArgb(0xbf, 0xd3, 0xeb);

        public Color OrbDropDownNorthA = Color.FromArgb(0xd7, 0xe5, 0xf7);

        public Color OrbDropDownNorthB = Color.FromArgb(0xd4, 0xe1, 0xf3);

        public Color OrbDropDownNorthC = Color.FromArgb(0xc6, 0xd8, 0xee);

        public Color OrbDropDownNorthD = Color.FromArgb(0xb7, 0xca, 0xe6);

        public Color OrbDropDownSouthC = Color.FromArgb(0xb0, 0xc9, 0xea);

        public Color OrbDropDownSouthD = Color.FromArgb(0xcf, 0xe0, 0xf5);

        public Color OrbDropDownContentbg = Color.FromArgb(0xE9, 0xEA, 0xEE);

        public Color OrbDropDownContentbglight = Color.FromArgb(0xFA, 0xFA, 0xFA);

        public Color OrbDropDownSeparatorlight = Color.FromArgb(0xF5, 0xF5, 0xF5);

        public Color OrbDropDownSeparatordark = Color.FromArgb(0xC5, 0xC5, 0xC5);

        /// <summary>
        /// Caption bar is made of 4 rectangles height of each is indicated below
        /// </summary>
        public Color Caption1 = FromHexStr("#E3EBF6"); //4

        public Color Caption2 = FromHexStr("#DAE9FD");

        public Color Caption3 = FromHexStr("#D5E5FA"); //4

        public Color Caption4 = FromHexStr("#D9E7F9");

        public Color Caption5 = FromHexStr("#CADEF7"); //23

        public Color Caption6 = FromHexStr("#E4EFFD");

        public Color Caption7 = FromHexStr("#B0CFF7"); //1

        public Color QuickAccessBorderDark = FromHexStr("#B6CAE2");

        public Color QuickAccessBorderLight = FromHexStr("#F2F6FB");

        public Color QuickAccessUpper = FromHexStr("#E0EBF9");

        public Color QuickAccessLower = FromHexStr("#C9D9EE");

        public Color OrbOptionBorder = FromHexStr("#7793B9");

        public Color OrbOptionBackground = FromHexStr("#E8F1FC");

        public Color OrbOptionShine = FromHexStr("#D2E1F4");

        #endregion

        #region Fields

        public Color Arrow = FromHexStr("#678CBD");

        public Color ArrowLight = Color.FromArgb(200, Color.White);

        public Color ArrowDisabled = FromHexStr("#B7B7B7");

        public Color Text = FromHexStr("#15428B");

        /// <summary>
        /// Orb colors in normal state
        /// </summary>
        public Color OrbBackgroundDark = FromHexStr("#7C8CA4");

        public Color OrbBackgroundMedium = FromHexStr("#99ABC6");

        public Color OrbBackgroundLight = Color.White;

        public Color OrbLight = Color.White;

        /// <summary>
        /// Orb colors in selected state
        /// </summary>
        public Color OrbSelectedBackgroundDark = FromHexStr("#DFAA1A");

        public Color OrbSelectedBackgroundMedium = FromHexStr("#F9D12E");

        public Color OrbSelectedBackgroundLight = FromHexStr("#FFEF36");

        public Color OrbSelectedLight = FromHexStr("#FFF52B");

        /// <summary>
        /// Orb colors in pressed state
        /// </summary>
        public Color OrbPressedBackgroundDark = FromHexStr("#CE8410");

        public Color OrbPressedBackgroundMedium = FromHexStr("#CE8410");

        public Color OrbPressedBackgroundLight = FromHexStr("#F57603");

        public Color OrbPressedLight = FromHexStr("#F08500");

        public Color OrbBorderAero = FromHexStr("#99A1AD");

        /// <summary>
        /// 2010 style Orb colors
        /// </summary>
        public Color OrbButtonText = Color.White;

        public Color OrbButtonBackground = Color.FromArgb(60, 120, 187);

        public Color OrbButtonDark = Color.FromArgb(25, 65, 135);

        public Color OrbButtonMedium = Color.FromArgb(56, 135, 191);

        public Color OrbButtonLight = Color.FromArgb(64, 154, 207);

        public Color OrbButtonPressedCenter = Color.FromArgb(25, 64, 136);

        public Color OrbButtonPressedNorth = Color.FromArgb(71, 132, 194);

        public Color OrbButtonPressedSouth = Color.FromArgb(56, 135, 191);

        public Color OrbButtonGlossyNorth = Color.FromArgb(71, 132, 194);

        public Color OrbButtonGlossySouth = Color.FromArgb(46, 104, 178);

        public Color OrbButtonBorderDark = Color.FromArgb(68, 135, 213);

        public Color OrbButtonBorderLight = Color.FromArgb(160, 204, 243);

        //public Color RibbonBackground = FromHexStr("#BFDBFF");
        public Color RibbonBackground = FromHexStr("#BED0E8");

        public Color TabBorder = FromHexStr("#8DB2E3");

        public Color TabNorth = FromHexStr("#EBF3FE");

        public Color TabSouth = FromHexStr("#E1EAF6");

        public Color TabGlow = FromHexStr("#D1FBFF");

        public Color TabText = FromHexStr("#15428B");

        public Color TabActiveText = FromHexStr("#15428B");

        public Color TabContentNorth = FromHexStr("#C8D9ED");

        public Color TabContentSouth = FromHexStr("#E7F2FF");

        public Color TabSelectedGlow = FromHexStr("#E1D2A5");

        public Color PanelDarkBorder = Color.FromArgb(51, FromHexStr("#15428B"));

        public Color PanelLightBorder = Color.FromArgb(102, Color.White);

        public Color PanelTextBackground = FromHexStr("#C2D9F0");

        public Color PanelTextBackgroundSelected = FromHexStr("#C2D9F0");

        public Color PanelText = FromHexStr("#15428B");

        public Color PanelBackgroundSelected = Color.FromArgb(102, FromHexStr("#E8FFFD"));

        public Color PanelOverflowBackground = FromHexStr("#B9D1F0");

        public Color PanelOverflowBackgroundPressed = FromHexStr("#7699C8");

        public Color PanelOverflowBackgroundSelectedNorth = Color.FromArgb(100, Color.White);

        public Color PanelOverflowBackgroundSelectedSouth = Color.FromArgb(102, FromHexStr("#B8D7FD"));

        public Color ButtonBgOut = FromHexStr("#C1D5F1");

        public Color ButtonBgCenter = FromHexStr("#CFE0F7");

        public Color ButtonBorderOut = FromHexStr("#B9D0ED");

        public Color ButtonBorderIn = FromHexStr("#E3EDFB");

        public Color ButtonGlossyNorth = FromHexStr("#DEEBFE");

        public Color ButtonGlossySouth = FromHexStr("#CBDEF6");

        public Color ButtonDisabledBgOut = FromHexStr("#E0E4E8");

        public Color ButtonDisabledBgCenter = FromHexStr("#E8EBEF");

        public Color ButtonDisabledBorderOut = FromHexStr("#C5D1DE");

        public Color ButtonDisabledBorderIn = FromHexStr("#F1F3F5");

        public Color ButtonDisabledGlossyNorth = FromHexStr("#F0F3F6");

        public Color ButtonDisabledGlossySouth = FromHexStr("#EAEDF1");

        public Color ButtonSelectedBgOut = FromHexStr("#FFD646");

        public Color ButtonSelectedBgCenter = FromHexStr("#FFEAAC");

        public Color ButtonSelectedBorderOut = FromHexStr("#C2A978");

        public Color ButtonSelectedBorderIn = FromHexStr("#FFF2C7");

        public Color ButtonSelectedGlossyNorth = FromHexStr("#FFFDDB");

        public Color ButtonSelectedGlossySouth = FromHexStr("#FFE793");

        public Color ButtonPressedBgOut = FromHexStr("#F88F2C");

        public Color ButtonPressedBgCenter = FromHexStr("#FDF1B0");

        public Color ButtonPressedBorderOut = FromHexStr("#8E8165");

        public Color ButtonPressedBorderIn = FromHexStr("#F9C65A");

        public Color ButtonPressedGlossyNorth = FromHexStr("#FDD5A8");

        public Color ButtonPressedGlossySouth = FromHexStr("#FBB062");

        public Color ButtonCheckedBgOut = FromHexStr("#F9AA45");

        public Color ButtonCheckedBgCenter = FromHexStr("#FDEA9D");

        public Color ButtonCheckedBorderOut = FromHexStr("#8E8165");

        public Color ButtonCheckedBorderIn = FromHexStr("#F9C65A");

        public Color ButtonCheckedGlossyNorth = FromHexStr("#F8DBB7");

        public Color ButtonCheckedGlossySouth = FromHexStr("#FED18E");

        public Color ItemGroupOuterBorder = FromHexStr("#9EBAE1");

        public Color ItemGroupInnerBorder = Color.FromArgb(51, Color.White);

        public Color ItemGroupSeparatorLight = Color.FromArgb(64, Color.White);

        public Color ItemGroupSeparatorDark = Color.FromArgb(38, FromHexStr("#9EBAE1"));

        public Color ItemGroupBgNorth = FromHexStr("#CADCF0");

        public Color ItemGroupBgSouth = FromHexStr("#D0E1F7");

        public Color ItemGroupBgGlossy = FromHexStr("#BCD0E9");

        public Color ButtonListBorder = FromHexStr("#B9D0ED");

        public Color ButtonListBg = FromHexStr("#D4E6F8");

        public Color ButtonListBgSelected = FromHexStr("#ECF3FB");

        public Color DropDownBg = FromHexStr("#FAFAFA");

        public Color DropDownImageBg = FromHexStr("#E9EEEE");

        public Color DropDownImageSeparator = FromHexStr("#C5C5C5");

        public Color DropDownBorder = FromHexStr("#868686");

        public Color DropDownGripNorth = FromHexStr("#FFFFFF");

        public Color DropDownGripSouth = FromHexStr("#DFE9EF");

        public Color DropDownGripBorder = FromHexStr("#DDE7EE");

        public Color DropDownGripDark = FromHexStr("#5574A7");

        public Color DropDownGripLight = FromHexStr("#FFFFFF");

        public Color SeparatorLight = FromHexStr("#FAFBFD");

        public Color SeparatorDark = FromHexStr("#96B4DA");

        public Color SeparatorBg = FromHexStr("#DAE6EE");

        public Color SeparatorLine = FromHexStr("#C5C5C5");

        public Color TextBoxUnselectedBg = FromHexStr("#EAF2FB");

        public Color TextBoxBorder = FromHexStr("#ABC1DE");

        public Color ToolTipContentNorth = Color.FromArgb(250, 252, 254); // SystemColors.MenuBar;// FromHex("#C8D9ED");

        public Color ToolTipContentSouth = Color.FromArgb(206, 220, 241); // SystemColors.MenuBar;// FromHex("#E7F2FF");

        public Color ToolTipDarkBorder = Color.DarkGray; // Color.FromArgb(51, FromHex("#15428B"));

        public Color ToolTipLightBorder = Color.FromArgb(102, Color.White);

        public Color ToolTipText = WinApi.IsVista ? SystemColors.InactiveCaptionText : FromHexStr("#15428B"); // in XP SystemColors.InactiveCaptionText is hardly readable

        public Color ToolStripItemTextPressed = FromHexStr("#444444");

        public Color ToolStripItemTextSelected = FromHexStr("#444444");

        public Color ToolStripItemText = FromHexStr("#444444");

        public Color clrVerBG_Shadow = Color.FromArgb(255, 181, 190, 206);

        /// <summary>
        /// 2013 Colors
        /// Office 2013 White Theme
        /// </summary>
        public Color ButtonPressed_2013 = FromHexStr("#92C0E0");

        public Color ButtonSelected_2013 = FromHexStr("#CDE6F7");

        public Color OrbButton_2013 = FromHexStr("#0072C6");

        public Color OrbButtonSelected_2013 = FromHexStr("#2A8AD4");

        public Color OrbButtonPressed_2013 = FromHexStr("#2A8AD4");

        public Color TabText_2013 = FromHexStr("#0072C6");

        public Color TabTextSelected_2013 = FromHexStr("#444444");

        public Color PanelBorder_2013 = FromHexStr("#15428B");

        public Color RibbonBackground_2013 = FromHexStr("#FFFFFF");

        public Color TabCompleteBackground_2013 = FromHexStr("#FFFFFF");

        public Color TabNormalBackground_2013 = FromHexStr("#FFFFFF");

        public Color TabActiveBackbround_2013 = FromHexStr("#FFFFFF");

        public Color TabBorder_2013 = FromHexStr("#D4D4D4");

        public Color TabCompleteBorder_2013 = FromHexStr("#D4D4D4");

        public Color TabActiveBorder_2013 = FromHexStr("#D4D4D4");

        public Color OrbButtonText_2013 = FromHexStr("#FFFFFF");

        public Color PanelText_2013 = FromHexStr("#666666");

        public Color RibbonItemText_2013 = FromHexStr("#444444");

        public Color ToolTipText_2013 = FromHexStr("#262626");

        public Color ToolStripItemTextPressed_2013 = FromHexStr("#444444");

        public Color ToolStripItemTextSelected_2013 = FromHexStr("#444444");

        public Color ToolStripItemText_2013 = FromHexStr("#444444");

        #endregion

        #region Methods

        //internal static Color FromHex(string hex)
        private static Color FromHexStr(string hex)
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

        public Color FromHex(string hex)
        {
            return FromHexStr(hex);
        }

        internal static Color ToGray(Color c)
        {
            var m = (c.R + c.G + c.B) / 3;
            return Color.FromArgb(m, m, m);
        }

        #endregion

        #region Colors and Theme

        public void SetColor(RibbonColorPart ribbonColorPart, int red, int green, int blue)
        {
            this.SetColor(ribbonColorPart, Color.FromArgb(red, green, blue));
        }

        public void SetColor(RibbonColorPart ribbonColorPart, string hexColor)
        {
            this.SetColor(ribbonColorPart, this.FromHex(hexColor));
        }

        public void SetColor(RibbonColorPart ribbonColorPart, Color color)
        {
            switch (ribbonColorPart)
            {
                case RibbonColorPart.OrbDropDownDarkBorder:
                    this.OrbDropDownDarkBorder = color;
                    break;
                case RibbonColorPart.OrbDropDownLightBorder:
                    this.OrbDropDownLightBorder = color;
                    break;
                case RibbonColorPart.OrbDropDownBack:
                    this.OrbDropDownBack = color;
                    break;
                case RibbonColorPart.OrbDropDownNorthA:
                    this.OrbDropDownNorthA = color;
                    break;

                case RibbonColorPart.OrbDropDownNorthB:
                    this.OrbDropDownNorthB = color;
                    break;
                case RibbonColorPart.OrbDropDownNorthC:
                    this.OrbDropDownNorthC = color;
                    break;
                case RibbonColorPart.OrbDropDownNorthD:
                    this.OrbDropDownNorthD = color;
                    break;
                case RibbonColorPart.OrbDropDownSouthC:
                    this.OrbDropDownSouthC = color;
                    break;
                case RibbonColorPart.OrbDropDownSouthD:
                    this.OrbDropDownSouthD = color;
                    break;
                case RibbonColorPart.OrbDropDownContentbg:
                    this.OrbDropDownContentbg = color;
                    break;
                case RibbonColorPart.OrbDropDownContentbglight:
                    this.OrbDropDownContentbglight = color;
                    break;
                case RibbonColorPart.OrbDropDownSeparatorlight:
                    this.OrbDropDownSeparatorlight = color;
                    break;
                case RibbonColorPart.OrbDropDownSeparatordark:
                    this.OrbDropDownSeparatordark = color;
                    break;

                case RibbonColorPart.Caption1:
                    this.Caption1 = color;
                    break;
                case RibbonColorPart.Caption2:
                    this.Caption2 = color;
                    break;
                case RibbonColorPart.Caption3:
                    this.Caption3 = color;
                    break;
                case RibbonColorPart.Caption4:
                    this.Caption4 = color;
                    break;
                case RibbonColorPart.Caption5:
                    this.Caption5 = color;
                    break;
                case RibbonColorPart.Caption6:
                    this.Caption6 = color;
                    break;
                case RibbonColorPart.Caption7:
                    this.Caption7 = color;
                    break;

                case RibbonColorPart.QuickAccessBorderDark:
                    this.QuickAccessBorderDark = color;
                    break;
                case RibbonColorPart.QuickAccessBorderLight:
                    this.QuickAccessBorderLight = color;
                    break;
                case RibbonColorPart.QuickAccessUpper:
                    this.QuickAccessUpper = color;
                    break;
                case RibbonColorPart.QuickAccessLower:
                    this.QuickAccessLower = color;
                    break;

                case RibbonColorPart.OrbOptionBorder:
                    this.OrbOptionBorder = color;
                    break;
                case RibbonColorPart.OrbOptionBackground:
                    this.OrbOptionBackground = color;
                    break;
                case RibbonColorPart.OrbOptionShine:
                    this.OrbOptionShine = color;
                    break;

                case RibbonColorPart.Arrow:
                    this.Arrow = color;
                    break;
                case RibbonColorPart.ArrowLight:
                    this.ArrowLight = color;
                    break;
                case RibbonColorPart.ArrowDisabled:
                    this.ArrowDisabled = color;
                    break;
                case RibbonColorPart.Text:
                    this.Text = color;
                    break;

                //case RibbonColorPart.RibbonBackground:
                //RibbonBackground = color; break;
                case RibbonColorPart.RibbonBackground:
                    this.RibbonBackground = color;
                    break;
                case RibbonColorPart.TabBorder:
                    this.TabBorder = color;
                    break;
                case RibbonColorPart.TabNorth:
                    this.TabNorth = color;
                    break;
                case RibbonColorPart.TabSouth:
                    this.TabSouth = color;
                    break;
                case RibbonColorPart.TabGlow:
                    this.TabGlow = color;
                    break;
                case RibbonColorPart.TabText:
                    this.TabText = color;
                    break;
                case RibbonColorPart.TabActiveText:
                    this.TabActiveText = color;
                    break;
                case RibbonColorPart.TabContentNorth:
                    this.TabContentNorth = color;
                    break;
                case RibbonColorPart.TabContentSouth:
                    this.TabContentSouth = color;
                    break;
                case RibbonColorPart.TabSelectedGlow:
                    this.TabSelectedGlow = color;
                    break;
                case RibbonColorPart.PanelDarkBorder:
                    this.PanelDarkBorder = color;
                    break;
                case RibbonColorPart.PanelLightBorder:
                    this.PanelLightBorder = color;
                    break;
                case RibbonColorPart.PanelTextBackground:
                    this.PanelTextBackground = color;
                    break;
                case RibbonColorPart.PanelTextBackgroundSelected:
                    this.PanelTextBackgroundSelected = color;
                    break;
                case RibbonColorPart.PanelText:
                    this.PanelText = color;
                    break;
                case RibbonColorPart.PanelBackgroundSelected:
                    this.PanelBackgroundSelected = color;
                    break;
                case RibbonColorPart.PanelOverflowBackground:
                    this.PanelOverflowBackground = color;
                    break;
                case RibbonColorPart.PanelOverflowBackgroundPressed:
                    this.PanelOverflowBackgroundPressed = color;
                    break;
                case RibbonColorPart.PanelOverflowBackgroundSelectedNorth:
                    this.PanelOverflowBackgroundSelectedNorth = color;
                    break;
                case RibbonColorPart.PanelOverflowBackgroundSelectedSouth:
                    this.PanelOverflowBackgroundSelectedSouth = color;
                    break;

                case RibbonColorPart.ButtonBgOut:
                    this.ButtonBgOut = color;
                    break;
                case RibbonColorPart.ButtonBgCenter:
                    this.ButtonBgCenter = color;
                    break;
                case RibbonColorPart.ButtonBorderOut:
                    this.ButtonBorderOut = color;
                    break;
                case RibbonColorPart.ButtonBorderIn:
                    this.ButtonBorderIn = color;
                    break;
                case RibbonColorPart.ButtonGlossyNorth:
                    this.ButtonGlossyNorth = color;
                    break;
                case RibbonColorPart.ButtonGlossySouth:
                    this.ButtonGlossySouth = color;
                    break;

                case RibbonColorPart.ButtonDisabledBgOut:
                    this.ButtonDisabledBgOut = color;
                    break;
                case RibbonColorPart.ButtonDisabledBgCenter:
                    this.ButtonDisabledBgCenter = color;
                    break;
                case RibbonColorPart.ButtonDisabledBorderOut:
                    this.ButtonDisabledBorderOut = color;
                    break;
                case RibbonColorPart.ButtonDisabledBorderIn:
                    this.ButtonDisabledBorderIn = color;
                    break;
                case RibbonColorPart.ButtonDisabledGlossyNorth:
                    this.ButtonDisabledGlossyNorth = color;
                    break;
                case RibbonColorPart.ButtonDisabledGlossySouth:
                    this.ButtonDisabledGlossySouth = color;
                    break;

                case RibbonColorPart.ButtonSelectedBgOut:
                    this.ButtonSelectedBgOut = color;
                    break;
                case RibbonColorPart.ButtonSelectedBgCenter:
                    this.ButtonSelectedBgCenter = color;
                    break;
                case RibbonColorPart.ButtonSelectedBorderOut:
                    this.ButtonSelectedBorderOut = color;
                    break;
                case RibbonColorPart.ButtonSelectedBorderIn:
                    this.ButtonSelectedBorderIn = color;
                    break;
                case RibbonColorPart.ButtonSelectedGlossyNorth:
                    this.ButtonSelectedGlossyNorth = color;
                    break;
                case RibbonColorPart.ButtonSelectedGlossySouth:
                    this.ButtonSelectedGlossySouth = color;
                    break;

                case RibbonColorPart.ButtonPressedBgOut:
                    this.ButtonPressedBgOut = color;
                    break;
                case RibbonColorPart.ButtonPressedBgCenter:
                    this.ButtonPressedBgCenter = color;
                    break;
                case RibbonColorPart.ButtonPressedBorderOut:
                    this.ButtonPressedBorderOut = color;
                    break;
                case RibbonColorPart.ButtonPressedBorderIn:
                    this.ButtonPressedBorderIn = color;
                    break;
                case RibbonColorPart.ButtonPressedGlossyNorth:
                    this.ButtonPressedGlossyNorth = color;
                    break;
                case RibbonColorPart.ButtonPressedGlossySouth:
                    this.ButtonPressedGlossySouth = color;
                    break;

                case RibbonColorPart.ButtonCheckedBgOut:
                    this.ButtonCheckedBgOut = color;
                    break;
                case RibbonColorPart.ButtonCheckedBgCenter:
                    this.ButtonCheckedBgCenter = color;
                    break;
                case RibbonColorPart.ButtonCheckedBorderOut:
                    this.ButtonCheckedBorderOut = color;
                    break;
                case RibbonColorPart.ButtonCheckedBorderIn:
                    this.ButtonCheckedBorderIn = color;
                    break;
                case RibbonColorPart.ButtonCheckedGlossyNorth:
                    this.ButtonCheckedGlossyNorth = color;
                    break;
                case RibbonColorPart.ButtonCheckedGlossySouth:
                    this.ButtonCheckedGlossySouth = color;
                    break;

                case RibbonColorPart.ItemGroupOuterBorder:
                    this.ItemGroupOuterBorder = color;
                    break;
                case RibbonColorPart.ItemGroupInnerBorder:
                    this.ItemGroupInnerBorder = color;
                    break;
                case RibbonColorPart.ItemGroupSeparatorLight:
                    this.ItemGroupSeparatorLight = color;
                    break;
                case RibbonColorPart.ItemGroupSeparatorDark:
                    this.ItemGroupSeparatorDark = color;
                    break;
                case RibbonColorPart.ItemGroupBgNorth:
                    this.ItemGroupBgNorth = color;
                    break;
                case RibbonColorPart.ItemGroupBgSouth:
                    this.ItemGroupBgSouth = color;
                    break;
                case RibbonColorPart.ItemGroupBgGlossy:
                    this.ItemGroupBgGlossy = color;
                    break;

                case RibbonColorPart.ButtonListBorder:
                    this.ButtonListBorder = color;
                    break;
                case RibbonColorPart.ButtonListBg:
                    this.ButtonListBg = color;
                    break;
                case RibbonColorPart.ButtonListBgSelected:
                    this.ButtonListBgSelected = color;
                    break;

                case RibbonColorPart.DropDownBg:
                    this.DropDownBg = color;
                    break;
                case RibbonColorPart.DropDownImageBg:
                    this.DropDownImageBg = color;
                    break;
                case RibbonColorPart.DropDownImageSeparator:
                    this.DropDownImageSeparator = color;
                    break;
                case RibbonColorPart.DropDownBorder:
                    this.DropDownBorder = color;
                    break;
                case RibbonColorPart.DropDownGripNorth:
                    this.DropDownGripNorth = color;
                    break;
                case RibbonColorPart.DropDownGripSouth:
                    this.DropDownGripSouth = color;
                    break;
                case RibbonColorPart.DropDownGripBorder:
                    this.DropDownGripBorder = color;
                    break;
                case RibbonColorPart.DropDownGripDark:
                    this.DropDownGripDark = color;
                    break;
                case RibbonColorPart.DropDownGripLight:
                    this.DropDownGripLight = color;
                    break;

                case RibbonColorPart.SeparatorLight:
                    this.SeparatorLight = color;
                    break;
                case RibbonColorPart.SeparatorDark:
                    this.SeparatorDark = color;
                    break;
                case RibbonColorPart.SeparatorBg:
                    this.SeparatorBg = color;
                    break;
                case RibbonColorPart.SeparatorLine:
                    this.SeparatorLine = color;
                    break;

                case RibbonColorPart.TextBoxUnselectedBg:
                    this.TextBoxUnselectedBg = color;
                    break;
                case RibbonColorPart.TextBoxBorder:
                    this.TextBoxBorder = color;
                    break;

                case RibbonColorPart.ToolTipContentNorth:
                    this.ToolTipContentNorth = color;
                    break;
                case RibbonColorPart.ToolTipContentSouth:
                    this.ToolTipContentSouth = color;
                    break;
                case RibbonColorPart.ToolTipDarkBorder:
                    this.ToolTipDarkBorder = color;
                    break;
                case RibbonColorPart.ToolTipLightBorder:
                    this.ToolTipLightBorder = color;
                    break;

                case RibbonColorPart.ToolStripItemTextPressed:
                    this.ToolStripItemTextPressed = color;
                    break;
                case RibbonColorPart.ToolStripItemTextSelected:
                    this.ToolStripItemTextSelected = color;
                    break;
                case RibbonColorPart.ToolStripItemText:
                    this.ToolStripItemText = color;
                    break;

                case RibbonColorPart.ButtonPressed_2013:
                    this.ButtonPressed_2013 = color;
                    break;
                case RibbonColorPart.ButtonSelected_2013:
                    this.ButtonSelected_2013 = color;
                    break;
                case RibbonColorPart.OrbButton_2013:
                    this.OrbButton_2013 = color;
                    break;
                case RibbonColorPart.OrbButtonSelected_2013:
                    this.OrbButtonSelected_2013 = color;
                    break;
                case RibbonColorPart.OrbButtonPressed_2013:
                    this.OrbButtonPressed_2013 = color;
                    break;
                case RibbonColorPart.TabText_2013:
                    this.TabText_2013 = color;
                    break;
                case RibbonColorPart.TabTextSelected_2013:
                    this.TabTextSelected_2013 = color;
                    break;
                case RibbonColorPart.PanelBorder_2013:
                    this.PanelBorder_2013 = color;
                    break;
                case RibbonColorPart.RibbonBackground_2013:
                    this.RibbonBackground_2013 = color;
                    break;
                case RibbonColorPart.TabCompleteBackground_2013:
                    this.TabCompleteBackground_2013 = color;
                    break;
                case RibbonColorPart.TabNormalBackground_2013:
                    this.TabNormalBackground_2013 = color;
                    break;
                case RibbonColorPart.TabActiveBackbround_2013:
                    this.TabActiveBackbround_2013 = color;
                    break;
                case RibbonColorPart.TabBorder_2013:
                    this.TabBorder_2013 = color;
                    break;
                case RibbonColorPart.TabCompleteBorder_2013:
                    this.TabCompleteBorder_2013 = color;
                    break;
                case RibbonColorPart.TabActiveBorder_2013:
                    this.TabActiveBorder_2013 = color;
                    break;
                case RibbonColorPart.OrbButtonText_2013:
                    this.OrbButtonText_2013 = color;
                    break;
                case RibbonColorPart.PanelText_2013:
                    this.PanelText_2013 = color;
                    break;
                case RibbonColorPart.RibbonItemText_2013:
                    this.RibbonItemText_2013 = color;
                    break;
                case RibbonColorPart.ToolTipText_2013:
                    this.ToolTipText_2013 = color;
                    break;

                case RibbonColorPart.ToolStripItemTextPressed_2013:
                    this.ToolStripItemTextPressed_2013 = color;
                    break;
                case RibbonColorPart.ToolStripItemTextSelected_2013:
                    this.ToolStripItemTextSelected_2013 = color;
                    break;
                case RibbonColorPart.ToolStripItemText_2013:
                    this.ToolStripItemText_2013 = color;
                    break;
                default:
                    break;
            }
        }

        public string GetColorHexStr(RibbonColorPart ribbonColorPart)
        {
            var c = this.GetColor(ribbonColorPart);
            var sb = new StringBuilder();
            sb.AppendFormat("#");
            sb.Append(BitConverter.ToString(new[] { c.R }));
            sb.Append(BitConverter.ToString(new[] { c.G }));
            sb.Append(BitConverter.ToString(new[] { c.B }));
            return sb.ToString();
        }

        public Color GetColor(RibbonColorPart ribbonColorPart)
        {
            switch (ribbonColorPart)
            {
                case RibbonColorPart.OrbDropDownDarkBorder:
                    return this.OrbDropDownDarkBorder;
                case RibbonColorPart.OrbDropDownLightBorder:
                    return this.OrbDropDownLightBorder;
                case RibbonColorPart.OrbDropDownBack:
                    return this.OrbDropDownBack;
                case RibbonColorPart.OrbDropDownNorthA:
                    return this.OrbDropDownNorthA;
                case RibbonColorPart.OrbDropDownNorthB:
                    return this.OrbDropDownNorthB;
                case RibbonColorPart.OrbDropDownNorthC:
                    return this.OrbDropDownNorthC;
                case RibbonColorPart.OrbDropDownNorthD:
                    return this.OrbDropDownNorthD;
                case RibbonColorPart.OrbDropDownSouthC:
                    return this.OrbDropDownSouthC;
                case RibbonColorPart.OrbDropDownSouthD:
                    return this.OrbDropDownSouthD;
                case RibbonColorPart.OrbDropDownContentbg:
                    return this.OrbDropDownContentbg;
                case RibbonColorPart.OrbDropDownContentbglight:
                    return this.OrbDropDownContentbglight;
                case RibbonColorPart.OrbDropDownSeparatorlight:
                    return this.OrbDropDownSeparatorlight;
                case RibbonColorPart.OrbDropDownSeparatordark:
                    return this.OrbDropDownSeparatordark;

                case RibbonColorPart.Caption1:
                    return this.Caption1;
                case RibbonColorPart.Caption2:
                    return this.Caption2;
                case RibbonColorPart.Caption3:
                    return this.Caption3;
                case RibbonColorPart.Caption4:
                    return this.Caption4;
                case RibbonColorPart.Caption5:
                    return this.Caption5;
                case RibbonColorPart.Caption6:
                    return this.Caption6;
                case RibbonColorPart.Caption7:
                    return this.Caption7;

                case RibbonColorPart.QuickAccessBorderDark:
                    return this.QuickAccessBorderDark;
                case RibbonColorPart.QuickAccessBorderLight:
                    return this.QuickAccessBorderLight;
                case RibbonColorPart.QuickAccessUpper:
                    return this.QuickAccessUpper;
                case RibbonColorPart.QuickAccessLower:
                    return this.QuickAccessLower;

                case RibbonColorPart.OrbOptionBorder:
                    return this.OrbOptionBorder;
                case RibbonColorPart.OrbOptionBackground:
                    return this.OrbOptionBackground;
                case RibbonColorPart.OrbOptionShine:
                    return this.OrbOptionShine;

                case RibbonColorPart.Arrow:
                    return this.Arrow;
                case RibbonColorPart.ArrowLight:
                    return this.ArrowLight;
                case RibbonColorPart.ArrowDisabled:
                    return this.ArrowDisabled;
                case RibbonColorPart.Text:
                    return this.Text;

                case RibbonColorPart.RibbonBackground:
                    return this.RibbonBackground;
                case RibbonColorPart.TabBorder:
                    return this.TabBorder;
                case RibbonColorPart.TabNorth:
                    return this.TabNorth;
                case RibbonColorPart.TabSouth:
                    return this.TabSouth;
                case RibbonColorPart.TabGlow:
                    return this.TabGlow;
                case RibbonColorPart.TabText:
                    return this.TabText;
                case RibbonColorPart.TabActiveText:
                    return this.TabActiveText;
                case RibbonColorPart.TabContentNorth:
                    return this.TabContentNorth;
                case RibbonColorPart.TabContentSouth:
                    return this.TabContentSouth;
                case RibbonColorPart.TabSelectedGlow:
                    return this.TabSelectedGlow;
                case RibbonColorPart.PanelDarkBorder:
                    return this.PanelDarkBorder;
                case RibbonColorPart.PanelLightBorder:
                    return this.PanelLightBorder;
                case RibbonColorPart.PanelTextBackground:
                    return this.PanelTextBackground;
                case RibbonColorPart.PanelTextBackgroundSelected:
                    return this.PanelTextBackgroundSelected;
                case RibbonColorPart.PanelText:
                    return this.PanelText;
                case RibbonColorPart.PanelBackgroundSelected:
                    return this.PanelBackgroundSelected;
                case RibbonColorPart.PanelOverflowBackground:
                    return this.PanelOverflowBackground;
                case RibbonColorPart.PanelOverflowBackgroundPressed:
                    return this.PanelOverflowBackgroundPressed;
                case RibbonColorPart.PanelOverflowBackgroundSelectedNorth:
                    return this.PanelOverflowBackgroundSelectedNorth;
                case RibbonColorPart.PanelOverflowBackgroundSelectedSouth:
                    return this.PanelOverflowBackgroundSelectedSouth;

                case RibbonColorPart.ButtonBgOut:
                    return this.ButtonBgOut;
                case RibbonColorPart.ButtonBgCenter:
                    return this.ButtonBgCenter;
                case RibbonColorPart.ButtonBorderOut:
                    return this.ButtonBorderOut;
                case RibbonColorPart.ButtonBorderIn:
                    return this.ButtonBorderIn;
                case RibbonColorPart.ButtonGlossyNorth:
                    return this.ButtonGlossyNorth;
                case RibbonColorPart.ButtonGlossySouth:
                    return this.ButtonGlossySouth;

                case RibbonColorPart.ButtonDisabledBgOut:
                    return this.ButtonDisabledBgOut;
                case RibbonColorPart.ButtonDisabledBgCenter:
                    return this.ButtonDisabledBgCenter;
                case RibbonColorPart.ButtonDisabledBorderOut:
                    return this.ButtonDisabledBorderOut;
                case RibbonColorPart.ButtonDisabledBorderIn:
                    return this.ButtonDisabledBorderIn;
                case RibbonColorPart.ButtonDisabledGlossyNorth:
                    return this.ButtonDisabledGlossyNorth;
                case RibbonColorPart.ButtonDisabledGlossySouth:
                    return this.ButtonDisabledGlossySouth;

                case RibbonColorPart.ButtonSelectedBgOut:
                    return this.ButtonSelectedBgOut;
                case RibbonColorPart.ButtonSelectedBgCenter:
                    return this.ButtonSelectedBgCenter;
                case RibbonColorPart.ButtonSelectedBorderOut:
                    return this.ButtonSelectedBorderOut;
                case RibbonColorPart.ButtonSelectedBorderIn:
                    return this.ButtonSelectedBorderIn;
                case RibbonColorPart.ButtonSelectedGlossyNorth:
                    return this.ButtonSelectedGlossyNorth;
                case RibbonColorPart.ButtonSelectedGlossySouth:
                    return this.ButtonSelectedGlossySouth;

                case RibbonColorPart.ButtonPressedBgOut:
                    return this.ButtonPressedBgOut;
                case RibbonColorPart.ButtonPressedBgCenter:
                    return this.ButtonPressedBgCenter;
                case RibbonColorPart.ButtonPressedBorderOut:
                    return this.ButtonPressedBorderOut;
                case RibbonColorPart.ButtonPressedBorderIn:
                    return this.ButtonPressedBorderIn;
                case RibbonColorPart.ButtonPressedGlossyNorth:
                    return this.ButtonPressedGlossyNorth;
                case RibbonColorPart.ButtonPressedGlossySouth:
                    return this.ButtonPressedGlossySouth;

                case RibbonColorPart.ButtonCheckedBgOut:
                    return this.ButtonCheckedBgOut;
                case RibbonColorPart.ButtonCheckedBgCenter:
                    return this.ButtonCheckedBgCenter;
                case RibbonColorPart.ButtonCheckedBorderOut:
                    return this.ButtonCheckedBorderOut;
                case RibbonColorPart.ButtonCheckedBorderIn:
                    return this.ButtonCheckedBorderIn;
                case RibbonColorPart.ButtonCheckedGlossyNorth:
                    return this.ButtonCheckedGlossyNorth;
                case RibbonColorPart.ButtonCheckedGlossySouth:
                    return this.ButtonCheckedGlossySouth;

                case RibbonColorPart.ItemGroupOuterBorder:
                    return this.ItemGroupOuterBorder;
                case RibbonColorPart.ItemGroupInnerBorder:
                    return this.ItemGroupInnerBorder;
                case RibbonColorPart.ItemGroupSeparatorLight:
                    return this.ItemGroupSeparatorLight;
                case RibbonColorPart.ItemGroupSeparatorDark:
                    return this.ItemGroupSeparatorDark;
                case RibbonColorPart.ItemGroupBgNorth:
                    return this.ItemGroupBgNorth;
                case RibbonColorPart.ItemGroupBgSouth:
                    return this.ItemGroupBgSouth;
                case RibbonColorPart.ItemGroupBgGlossy:
                    return this.ItemGroupBgGlossy;

                case RibbonColorPart.ButtonListBorder:
                    return this.ButtonListBorder;
                case RibbonColorPart.ButtonListBg:
                    return this.ButtonListBg;
                case RibbonColorPart.ButtonListBgSelected:
                    return this.ButtonListBgSelected;

                case RibbonColorPart.DropDownBg:
                    return this.DropDownBg;
                case RibbonColorPart.DropDownImageBg:
                    return this.DropDownImageBg;
                case RibbonColorPart.DropDownImageSeparator:
                    return this.DropDownImageSeparator;
                case RibbonColorPart.DropDownBorder:
                    return this.DropDownBorder;
                case RibbonColorPart.DropDownGripNorth:
                    return this.DropDownGripNorth;
                case RibbonColorPart.DropDownGripSouth:
                    return this.DropDownGripSouth;
                case RibbonColorPart.DropDownGripBorder:
                    return this.DropDownGripBorder;
                case RibbonColorPart.DropDownGripDark:
                    return this.DropDownGripDark;
                case RibbonColorPart.DropDownGripLight:
                    return this.DropDownGripLight;

                case RibbonColorPart.SeparatorLight:
                    return this.SeparatorLight;
                case RibbonColorPart.SeparatorDark:
                    return this.SeparatorDark;
                case RibbonColorPart.SeparatorBg:
                    return this.SeparatorBg;
                case RibbonColorPart.SeparatorLine:
                    return this.SeparatorLine;

                case RibbonColorPart.TextBoxUnselectedBg:
                    return this.TextBoxUnselectedBg;
                case RibbonColorPart.TextBoxBorder:
                    return this.TextBoxBorder;

                case RibbonColorPart.ToolTipContentNorth:
                    return this.ToolTipContentNorth;
                case RibbonColorPart.ToolTipContentSouth:
                    return this.ToolTipContentSouth;
                case RibbonColorPart.ToolTipDarkBorder:
                    return this.ToolTipDarkBorder;
                case RibbonColorPart.ToolTipLightBorder:
                    return this.ToolTipLightBorder;

                case RibbonColorPart.ToolStripItemTextPressed:
                    return this.ToolStripItemTextPressed;
                case RibbonColorPart.ToolStripItemTextSelected:
                    return this.ToolStripItemTextSelected;
                case RibbonColorPart.ToolStripItemText:
                    return this.ToolStripItemText;

                case RibbonColorPart.ButtonPressed_2013:
                    return this.ButtonPressed_2013;
                case RibbonColorPart.ButtonSelected_2013:
                    return this.ButtonSelected_2013;
                case RibbonColorPart.OrbButton_2013:
                    return this.OrbButton_2013;
                case RibbonColorPart.OrbButtonSelected_2013:
                    return this.OrbButtonSelected_2013;
                case RibbonColorPart.OrbButtonPressed_2013:
                    return this.OrbButtonPressed_2013;
                case RibbonColorPart.TabText_2013:
                    return this.TabText_2013;
                case RibbonColorPart.TabTextSelected_2013:
                    return this.TabTextSelected_2013;
                case RibbonColorPart.PanelBorder_2013:
                    return this.PanelBorder_2013;
                case RibbonColorPart.RibbonBackground_2013:
                    return this.RibbonBackground_2013;
                case RibbonColorPart.TabCompleteBackground_2013:
                    return this.TabCompleteBackground_2013;
                case RibbonColorPart.TabNormalBackground_2013:
                    return this.TabNormalBackground_2013;
                case RibbonColorPart.TabActiveBackbround_2013:
                    return this.TabActiveBackbround_2013;
                case RibbonColorPart.TabBorder_2013:
                    return this.TabBorder_2013;
                case RibbonColorPart.TabCompleteBorder_2013:
                    return this.TabCompleteBorder_2013;
                case RibbonColorPart.TabActiveBorder_2013:
                    return this.TabActiveBorder_2013;
                case RibbonColorPart.OrbButtonText_2013:
                    return this.OrbButtonText_2013;
                case RibbonColorPart.PanelText_2013:
                    return this.PanelText_2013;
                case RibbonColorPart.RibbonItemText_2013:
                    return this.RibbonItemText_2013;
                case RibbonColorPart.ToolTipText_2013:
                    return this.ToolTipText_2013;

                case RibbonColorPart.ToolStripItemTextPressed_2013:
                    return this.ToolStripItemTextPressed_2013;
                case RibbonColorPart.ToolStripItemTextSelected_2013:
                    return this.ToolStripItemTextSelected_2013;
                case RibbonColorPart.ToolStripItemText_2013:
                    return this.ToolStripItemText_2013;

                default:
                    return Color.White;
            }
        }

        #endregion

        #region Theme File Read / Write

        public string WriteThemeIniFile()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[Properties]");
            sb.AppendLine("ThemeName = " + this.ThemeName);
            sb.AppendLine("Author = " + this.ThemeAuthor);
            sb.AppendLine("AuthorEmail = " + this.ThemeAuthorEmail);
            sb.AppendLine("AuthorWebsite = " + this.ThemeAuthorWebsite);
            sb.AppendLine("DateCreated = " + this.ThemeDateCreated);
            sb.AppendLine();
            sb.AppendLine("[ColorTable]");

            var count = Enum.GetNames(typeof(RibbonColorPart)).Length;
            for (var i = 0; i < count; i++)
            {
                sb.AppendLine((RibbonColorPart)i + " = " + this.GetColorHexStr((RibbonColorPart)i));
            }

            return sb.ToString();
        }

        public void ReadThemeIniFile(string iniFileContent)
        {
            string[] sa = null;
            if (iniFileContent.Contains("\r\n"))
            {
                sa = iniFileContent.Split(new[] { "\r\n" }, StringSplitOptions.None);
            }
            else if (iniFileContent.Contains("\n"))
            {
                sa = iniFileContent.Split(new[] { "\n" }, StringSplitOptions.None);
            }
            else
            {
                throw new Exception("Unrecognized end line delimeter.");
            }

            var dic1 = new Dictionary<string, RibbonColorPart>();
            foreach (RibbonColorPart e in Enum.GetValues(typeof(RibbonColorPart)))
            {
                dic1[e.ToString().ToLower()] = e;
            }

            foreach (var s in sa)
            {
                var a = s.Trim();
                if (a.Length == 0)
                {
                }
                else
                {
                    var sb = a.Split('=');
                    if (sb.Length != 2)
                    {
                        continue;
                    }
                    var b1 = sb[0].Trim().ToLower();
                    var b2 = sb[1].Trim();

                    if (b1 == "author")
                    {
                        this.ThemeAuthor = b2;
                    }
                    else if (b1 == "authorwebsite")
                    {
                        this.ThemeAuthorWebsite = b2;
                    }
                    else if (b1 == "authoremail")
                    {
                        this.ThemeAuthorEmail = b2;
                    }
                    else if (b1 == "datecreated")
                    {
                        this.ThemeDateCreated = b2;
                    }
                    else if (b1 == "themename")
                    {
                        this.ThemeName = b2;
                    }
                    else
                    {
                        if (dic1.ContainsKey(b1))
                        {
                            this.SetColor(dic1[b1], b2);
                        }
                    }
                }
            }
        }

        public string WriteThemeXmlFile()
        {
            var a = "";
            using (var str = new StringWriter())
            {
                using (var xml = new XmlTextWriter(str))
                {
                    xml.WriteStartDocument();
                    xml.WriteWhitespace("\r\n");
                    xml.WriteStartElement("RibbonColorTheme");
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteStartElement("Properties");
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("ThemeName", this.ThemeName);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("Author", this.ThemeAuthor);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("AuthorEmail", this.ThemeAuthorEmail);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("AuthorWebsite", this.ThemeAuthorWebsite);
                    xml.WriteWhitespace("\r\n\t\t");
                    xml.WriteElementString("DateCreated", this.ThemeDateCreated);
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteEndElement();
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteStartElement("ColorTable");
                    var count = Enum.GetNames(typeof(RibbonColorPart)).Length;
                    for (var i = 0; i < count; i++)
                    {
                        xml.WriteWhitespace("\r\n\t\t");
                        xml.WriteElementString(((RibbonColorPart)i).ToString(), this.GetColorHexStr((RibbonColorPart)i));
                    }
                    xml.WriteWhitespace("\r\n\t");
                    xml.WriteEndElement();
                    xml.WriteWhitespace("\r\n");
                    xml.WriteEndElement();
                    xml.WriteWhitespace("\r\n");
                    xml.WriteEndDocument();
                }
                a = str.ToString();
            }
            return a;
        }

        public void ReadThemeXmlFile(string xmlFileContent)
        {
            var dic1 = new Dictionary<string, RibbonColorPart>();
            foreach (RibbonColorPart e in Enum.GetValues(typeof(RibbonColorPart)))
            {
                dic1[e.ToString().ToLower()] = e;
            }

            using (var stringReader = new StringReader(xmlFileContent))
            {
                using (var reader = new XmlTextReader(stringReader))
                {
                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {
                            case "ThemeName":
                                this.ThemeName = reader.ReadString();
                                break;
                            case "Author":
                                this.ThemeAuthor = reader.ReadString();
                                break;
                            case "AuthorEmail":
                                this.ThemeAuthorEmail = reader.ReadString();
                                break;
                            case "AuthorWebsite":
                                this.ThemeAuthorWebsite = reader.ReadString();
                                break;
                            case "DateCreated":
                                this.ThemeDateCreated = reader.ReadString();
                                break;
                            default:
                                {
                                    if (dic1.ContainsKey(reader.Name.ToLower()))
                                    {
                                        this.SetColor(dic1[reader.Name.ToLower()], reader.ReadString());
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }

        #endregion
    }
}