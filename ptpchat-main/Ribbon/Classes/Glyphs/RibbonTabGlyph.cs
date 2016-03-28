namespace PtpChat.Main.Ribbon.Classes.Glyphs
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Windows.Forms.Design.Behavior;

    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;
    using RibbonDesigner = PtpChat.Main.Ribbon.Classes.Designers.RibbonDesigner;
    using RibbonProfessionalRenderer = PtpChat.Main.Ribbon.Classes.Renderers.RibbonProfessionalRenderer;

    public class RibbonTabGlyph : Glyph
    {
        public RibbonTabGlyph(BehaviorService behaviorService, RibbonDesigner designer, Ribbon ribbon)
            : base(new RibbonTabGlyphBehavior(designer, ribbon))
        {
            this._behaviorService = behaviorService;
            this._componentDesigner = designer;
            this._ribbon = ribbon;
            this.size = new Size(60, 16);
        }

        private readonly BehaviorService _behaviorService;

        private RibbonDesigner _componentDesigner;

        private readonly Ribbon _ribbon;

        private Size size;

        public override Rectangle Bounds
        {
            get
            {
                var edge = this._behaviorService.ControlToAdornerWindow(this._ribbon);
                var tab = new Point(5, this._ribbon.OrbBounds.Bottom + 5);

                //If has tabs
                if (this._ribbon.Tabs.Count > 0 && this._ribbon.RightToLeft == RightToLeft.No)
                {
                    //Place glyph next to the last tab
                    var t = this._ribbon.Tabs[this._ribbon.Tabs.Count - 1];
                    tab.X = t.Bounds.Right + 5;
                    tab.Y = t.Bounds.Top + 2;
                }
                else if (this._ribbon.Tabs.Count > 0 && this._ribbon.RightToLeft == RightToLeft.Yes)
                {
                    //Place glyph next to the first tab
                    var t = this._ribbon.Tabs[this._ribbon.Tabs.Count - 1];
                    tab.X = t.Bounds.Left - 5 - this.size.Width;
                    tab.Y = t.Bounds.Top + 2;
                }
                return new Rectangle(edge.X + tab.X, edge.Y + tab.Y, this.size.Width, this.size.Height);
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
            using (var p = RibbonProfessionalRenderer.RoundRectangle(this.Bounds, 2))
            {
                using (var b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    pe.Graphics.FillPath(b, p);
                }
            }
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            pe.Graphics.DrawString("Add Tab", SystemFonts.DefaultFont, Brushes.White, this.Bounds, sf);
            pe.Graphics.SmoothingMode = smbuff;
        }
    }

    public class RibbonTabGlyphBehavior : Behavior
    {
        public RibbonTabGlyphBehavior(RibbonDesigner designer, Ribbon ribbon)
        {
            this._designer = designer;
            this._ribbon = ribbon;
        }

        private readonly RibbonDesigner _designer;

        private Ribbon _ribbon;

        public override bool OnMouseUp(Glyph g, MouseButtons button)
        {
            this._designer.AddTabVerb(this, EventArgs.Empty);
            return base.OnMouseUp(g, button);
        }
    }
}