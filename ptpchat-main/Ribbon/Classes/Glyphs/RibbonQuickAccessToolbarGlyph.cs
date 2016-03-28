namespace PtpChat.Main.Ribbon.Classes.Glyphs
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Windows.Forms.Design.Behavior;

    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;
    using RibbonButton = PtpChat.Main.Ribbon.Component_Classes.RibbonButton;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;

    public class RibbonQuickAccessToolbarGlyph : Glyph

    {
        public RibbonQuickAccessToolbarGlyph(BehaviorService behaviorService, RibbonDesigner designer, Ribbon ribbon)
            : base(new RibbonQuickAccessGlyphBehavior(designer, ribbon))
        {
            this._behaviorService = behaviorService;
            this._componentDesigner = designer;
            this._ribbon = ribbon;
        }

        private readonly BehaviorService _behaviorService;

        private RibbonDesigner _componentDesigner;

        private readonly Ribbon _ribbon;

        public override Rectangle Bounds
        {
            get
            {
                var edge = this._behaviorService.ControlToAdornerWindow(this._ribbon);
                if (!this._ribbon.CaptionBarVisible || !this._ribbon.QuickAcessToolbar.Visible)
                {
                    return Rectangle.Empty;
                }
                if (this._ribbon.RightToLeft == RightToLeft.No)
                {
                    return
                        new Rectangle(
                            edge.X + this._ribbon.QuickAcessToolbar.Bounds.Right + this._ribbon.QuickAcessToolbar.Bounds.Height / 2 + 4 + this._ribbon.QuickAcessToolbar.DropDownButton.Bounds.Width,
                            edge.Y + this._ribbon.QuickAcessToolbar.Bounds.Top,
                            this._ribbon.QuickAcessToolbar.Bounds.Height,
                            this._ribbon.QuickAcessToolbar.Bounds.Height);
                }
                return new Rectangle(
                    this._ribbon.QuickAcessToolbar.Bounds.Left - this._ribbon.QuickAcessToolbar.Bounds.Height / 2 - 4 - this._ribbon.QuickAcessToolbar.DropDownButton.Bounds.Width,
                    edge.Y + this._ribbon.QuickAcessToolbar.Bounds.Top,
                    this._ribbon.QuickAcessToolbar.Bounds.Height,
                    this._ribbon.QuickAcessToolbar.Bounds.Height);
            }
        }

        public override Cursor GetHitTest(Point p)
        {
            if (this.Bounds.Contains(p))
            {
                return Cursors.Hand;
            }

            return null;
        }

        public override void Paint(PaintEventArgs pe)
        {
            if (this._ribbon.CaptionBarVisible && this._ribbon.QuickAcessToolbar.Visible)
            {
                var smbuff = pe.Graphics.SmoothingMode;
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    pe.Graphics.FillEllipse(b, this.Bounds);
                }
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                pe.Graphics.DrawString("+", SystemFonts.DefaultFont, Brushes.White, this.Bounds, sf);
                pe.Graphics.SmoothingMode = smbuff;
            }
        }
    }

    public class RibbonQuickAccessGlyphBehavior : Behavior
    {
        public RibbonQuickAccessGlyphBehavior(RibbonDesigner designer, Ribbon ribbon)
        {
            this._designer = designer;
            this._ribbon = ribbon;
        }

        private readonly RibbonDesigner _designer;

        private readonly Ribbon _ribbon;

        public override bool OnMouseUp(Glyph g, MouseButtons button)
        {
            this._designer.CreateItem(this._ribbon, this._ribbon.QuickAcessToolbar.Items, typeof(RibbonButton));
            return base.OnMouseUp(g, button);
        }
    }
}