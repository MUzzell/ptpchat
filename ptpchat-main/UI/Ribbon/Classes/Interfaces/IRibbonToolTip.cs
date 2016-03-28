namespace PtpChat.Main.Ribbon.Classes.Interfaces
{
    using System.Drawing;
    using System.Windows.Forms;

    using RibbonElementPopupEventHandler = PtpChat.Main.Ribbon.Classes.EventArgs.RibbonElementPopupEventHandler;

    public interface IRibbonToolTip
    {
        /// <summary>
        /// Gets or Sets the ToolTip Text
        /// </summary>
        string ToolTip { get; set; }

        /// <summary>
        /// Gets or Sets the ToolTip Title
        /// </summary>
        string ToolTipTitle { get; set; }

        /// <summary>
        /// Gets or Sets the ToolTip Image
        /// </summary>
        Image ToolTipImage { get; set; }

        /// <summary>
        /// Gets or Sets the stock ToolTip Icon
        /// </summary>
        ToolTipIcon ToolTipIcon { get; set; }

        /// <summary>
        /// Occurs before a ToolTip is initially displayed.
        /// <remarks>Use this event to change the ToolTip or Cancel it at all.</remarks>
        /// </summary>
        event RibbonElementPopupEventHandler ToolTipPopUp;
    }
}