namespace PtpChat.Main.Ribbon.Classes.Interfaces
{
    using System.Drawing;

    /// <summary>
    /// Implemented by Ribbon items that has scrollable content
    /// </summary>
    public interface IScrollableRibbonItem
    {
        /// <summary>
        /// Gets the bounds of the content (without scrolling controls)
        /// </summary>
        Rectangle ContentBounds { get; }

        /// <summary>
        /// Scrolls the content up
        /// </summary>
        void ScrollUp();

        /// <summary>
        /// Scrolls the content down
        /// </summary>
        void ScrollDown();
    }
}