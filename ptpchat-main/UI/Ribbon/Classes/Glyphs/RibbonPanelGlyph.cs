namespace PtpChat.Main.Ribbon.Classes.Glyphs
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Windows.Forms.Design.Behavior;

    using RibbonProfessionalRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonProfessionalRenderer;
    using RibbonTab = PtpChat.Main.Ribbon.Component_Classes.RibbonTab;
    using RibbonTabDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonTabDesigner;

    public class RibbonPanelGlyph : Glyph
    {
        public RibbonPanelGlyph(BehaviorService behaviorService, RibbonTabDesigner designer, RibbonTab tab)
            : base(new RibbonPanelGlyphBehavior(designer, tab))
        {
            this._behaviorService = behaviorService;
            this._componentDesigner = designer;
            this._tab = tab;
            this.size = new Size(60, 16);
        }

        private readonly BehaviorService _behaviorService;

        private readonly RibbonTab _tab;

        private RibbonTabDesigner _componentDesigner;

        private Size size;

        public override Rectangle Bounds
        {
            get
            {
                if (!this._tab.Active || !this._tab.Owner.Tabs.Contains(this._tab))
                {
                    return Rectangle.Empty;
                }
                var edge = this._behaviorService.ControlToAdornerWindow(this._tab.Owner);
                var pnl = new Point(5, this._tab.TabBounds.Bottom + 5); //_tab.Bounds.Y *2 + (_tab.Bounds.Height - size.Height) / 2);

                //If has panels
                if (this._tab.Panels.Count > 0)
                {
                    //Place glyph next to the last panel
                    var p = this._tab.Panels[this._tab.Panels.Count - 1];
                    if (this._tab.Owner.RightToLeft == RightToLeft.No)
                    {
                        pnl.X = p.Bounds.Right + 5;
                    }
                    else
                    {
                        pnl.X = p.Bounds.Left - 5 - this.size.Width;
                    }
                }

                return new Rectangle(edge.X + pnl.X, edge.Y + pnl.Y, this.size.Width, this.size.Height);
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
            var smbuff = pe.Graphics.SmoothingMode;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = RibbonProfessionalRenderer.RoundRectangle(this.Bounds, 9))
            {
                using (var b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    pe.Graphics.FillPath(b, p);
                }
            }
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            pe.Graphics.DrawString("Add Panel", SystemFonts.DefaultFont, Brushes.White, this.Bounds, sf);
            pe.Graphics.SmoothingMode = smbuff;
        }
    }

    public class RibbonPanelGlyphBehavior : Behavior
    {
        public RibbonPanelGlyphBehavior(RibbonTabDesigner designer, RibbonTab tab)
        {
            this._designer = designer;
            this._tab = tab;
        }

        private readonly RibbonTabDesigner _designer;

        private RibbonTab _tab;

        public override bool OnMouseUp(Glyph g, MouseButtons button)
        {
            this._designer.AddPanel(this, EventArgs.Empty);
            return base.OnMouseUp(g, button);
        }
    }
}