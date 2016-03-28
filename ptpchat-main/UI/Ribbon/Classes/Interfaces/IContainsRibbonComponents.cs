namespace PtpChat.Main.Ribbon.Classes.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Used to extract all child components from RibbonItem objects
    /// </summary>
    public interface IContainsRibbonComponents
    {
        IEnumerable<Component> GetAllChildComponents();
    }
}