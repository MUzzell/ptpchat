namespace PtpChat.Main.Ribbon.Classes.Renderers.Color_Tables
{
    using System.Drawing;

    public class RibbonProfesionalRendererColorTableHalloween : RibbonProfesionalRendererColorTable
    {
        public RibbonProfesionalRendererColorTableHalloween()
        {
            #region Fields

            this.OrbDropDownDarkBorder = ToGray(this.OrbDropDownDarkBorder);
            this.OrbDropDownLightBorder = ToGray(this.OrbDropDownLightBorder);
            this.OrbDropDownBack = ToGray(this.OrbDropDownBack);
            this.OrbDropDownNorthA = ToGray(this.OrbDropDownNorthA);
            this.OrbDropDownNorthB = ToGray(this.OrbDropDownNorthB);
            this.OrbDropDownNorthC = ToGray(this.OrbDropDownNorthC);
            this.OrbDropDownNorthD = ToGray(this.OrbDropDownNorthD);
            this.OrbDropDownSouthC = ToGray(this.OrbDropDownSouthC);
            this.OrbDropDownSouthD = ToGray(this.OrbDropDownSouthD);
            this.OrbDropDownContentbg = ToGray(this.OrbDropDownContentbg);
            this.OrbDropDownContentbglight = ToGray(this.OrbDropDownContentbglight);
            this.OrbDropDownSeparatorlight = ToGray(this.OrbDropDownSeparatorlight);
            this.OrbDropDownSeparatordark = ToGray(this.OrbDropDownSeparatordark);

            //###################################################################################
            //Top Border Background of the Ribbon.  Bar is made of 4 rectangles height of each
            //is indicated below.
            //###################################################################################
            this.Caption1 = ToGray(this.Caption1); //4
            this.Caption2 = ToGray(this.Caption2);
            this.Caption3 = ToGray(this.Caption3); //4
            this.Caption4 = ToGray(this.Caption4);
            this.Caption5 = ToGray(this.Caption5); //23
            this.Caption6 = ToGray(this.Caption6);
            this.Caption7 = ToGray(this.Caption7); //1

            this.QuickAccessBorderDark = ToGray(this.QuickAccessBorderDark);
            this.QuickAccessBorderLight = ToGray(this.QuickAccessBorderLight);
            this.QuickAccessUpper = ToGray(this.QuickAccessUpper);
            this.QuickAccessLower = ToGray(this.QuickAccessLower);

            this.OrbOptionBorder = ToGray(this.OrbOptionBorder);
            this.OrbOptionBackground = ToGray(this.OrbOptionBackground);
            this.OrbOptionShine = ToGray(this.OrbOptionShine);

            this.Arrow = this.FromHex("#7C7C7C");
            this.ArrowLight = this.FromHex("#EAF2F9");
            this.ArrowDisabled = this.FromHex("#7C7C7C");
            this.Text = this.FromHex("#000000");

            //###################################################################################
            //Main backGround for the Ribbon.
            //###################################################################################
            this.RibbonBackground = this.FromHex("#535353"); //For Theme change this

            //###################################################################################
            //Tab backGround for the Ribbon.
            //###################################################################################
            this.TabBorder = this.FromHex("#BEBEBE");
            this.TabNorth = this.FromHex("#F1F2F2");
            this.TabSouth = this.FromHex("#D6D9DF");

            this.TabGlow = this.FromHex("#D1FBFF");
            this.TabSelectedGlow = this.FromHex("#E1D2A5");

            this.TabText = Color.White;
            this.TabActiveText = Color.Black;

            //###################################################################################
            //Tab Content backGround for the Ribbon.
            //###################################################################################
            this.TabContentNorth = this.FromHex("#B6BCC6");
            this.TabContentSouth = this.FromHex("#E6F0F1");

            //###################################################################################
            //Borders(Drop Shadow) for the Panels (Dark = Outer Edge) (Light = Inner Edge)
            //###################################################################################
            this.PanelDarkBorder = this.FromHex("#AEB0B4"); //Color.FromArgb(51, FromHex("#FF0000"));//For Theme change this
            this.PanelLightBorder = this.FromHex("#E7E9ED"); //Color.FromArgb(102, Color.White);//For Theme change this

            this.PanelTextBackground = this.FromHex("#ABAEAE");
            this.PanelTextBackgroundSelected = this.FromHex("#949495");
            this.PanelText = Color.White;

            this.PanelBackgroundSelected = this.FromHex("#F3F5F5"); // Color.FromArgb(102, FromHex("#E8FFFD"));//For Theme change this
            this.PanelOverflowBackground = this.FromHex("#B9D1F0");
            this.PanelOverflowBackgroundPressed = this.FromHex("#AAAEB3");
            this.PanelOverflowBackgroundSelectedNorth = Color.FromArgb(100, Color.White);
            this.PanelOverflowBackgroundSelectedSouth = Color.FromArgb(102, this.FromHex("#EBEBEB"));

            this.ButtonBgOut = this.FromHex("#B4B9C2"); // FromHex("#C1D5F1");//For Theme change this

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
            this.ItemGroupInnerBorder = Color.FromArgb(51, Color.White);
            this.ItemGroupSeparatorLight = Color.FromArgb(64, Color.White);
            this.ItemGroupSeparatorDark = Color.FromArgb(38, this.FromHex("#ADB7BB"));
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

            this.ToolStripItemTextPressed = this.FromHex("#262626");
            this.ToolStripItemTextSelected = this.FromHex("#262626");
            this.ToolStripItemText = this.FromHex("#0072C6");

            this.clrVerBG_Shadow = Color.FromArgb(255, 181, 190, 206);

            /// <summary>
            /// 2013 Colors
            /// Office 2013 Dark Theme
            /// </summary>
            this.ButtonPressed_2013 = this.FromHex("#92C0E0");
            this.ButtonSelected_2013 = this.FromHex("#CDE6F7");
            this.OrbButton_2013 = this.FromHex("#333333");
            this.OrbButtonSelected_2013 = this.FromHex("#2A8AD4");
            this.OrbButtonPressed_2013 = this.FromHex("#2A8AD4");

            this.TabText_2013 = this.FromHex("#0072C6");
            this.TabTextSelected_2013 = this.FromHex("#262626");
            this.PanelBorder_2013 = this.FromHex("#15428B");

            this.RibbonBackground_2013 = this.FromHex("#DEDEDE");
            this.TabCompleteBackground_2013 = this.FromHex("#F3F3F3");
            this.TabNormalBackground_2013 = this.FromHex("#DEDEDE");
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
            this.ToolStripItemText_2013 = this.FromHex("#0072C6");

            #endregion
        }
    }
}