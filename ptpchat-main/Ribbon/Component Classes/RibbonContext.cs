namespace PtpChat.Main.Ribbon.Component_Classes
{
    using System.ComponentModel;
    using System.Drawing;

    using PtpChat.Main.Ribbon.Classes.Collections;

    /// <summary>
    /// Represents a context on the Ribbon
    /// </summary>
    /// <remarks>Contexts are useful when some tabs are volatile, depending on some selection. A RibbonTabContext can be added to the ribbon by calling Ribbon.Contexts.Add</remarks>
    [ToolboxItem(false)]
    public class RibbonContext : Component
    {
        /// <summary>
        /// Creates a new RibbonTabContext
        /// </summary>
        /// <param name="Ribbon">Ribbon that owns the context</param>
        public RibbonContext(Ribbon owner)
        {
            this.Tabs = new RibbonTabCollection(owner);
        }

        /// <summary>
        /// Gets or sets the text of the Context
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the glow that indicates a context
        /// </summary>
        public Color GlowColor { get; set; }

        /// <summary>
        /// Gets the Ribbon that owns this context
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Ribbon Owner { get; private set; }

        public RibbonTabCollection Tabs { get; }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Ribbon owner)
        {
            this.Owner = owner;
            this.Tabs.SetOwner(owner);
        }
    }
}