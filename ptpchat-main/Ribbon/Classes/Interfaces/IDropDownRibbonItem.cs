namespace PtpChat.Main.Ribbon.Classes.Interfaces
{
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.Collections;

    public interface IDropDownRibbonItem
    {
        RibbonItemCollection DropDownItems { get; }

        Rectangle DropDownButtonBounds { get; }

        bool DropDownButtonVisible { get; }

        bool DropDownButtonSelected { get; }

        bool DropDownButtonPressed { get; }
    }
}