namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Windows.Forms.Design.Behavior;

    using PtpChat.Main.Ribbon.Classes.Glyphs;
    using PtpChat.Main.Ribbon.Component_Classes;

    public class RibbonTabDesigner : ComponentDesigner
    {
        private Adorner panelAdorner;

        public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new[] { new DesignerVerb("Add Panel", this.AddPanel) });

        public RibbonTab Tab => this.Component as RibbonTab;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            this.panelAdorner = new Adorner();

            //Kevin Carbis - another point where exception is thrown by the designer when current is null
            if (RibbonDesigner.Current != null)
            {
                var bs = RibbonDesigner.Current.GetBehaviorService();

                if (bs == null)
                {
                    return;
                }

                bs.Adorners.AddRange(new[] { this.panelAdorner });

                this.panelAdorner.Glyphs.Add(new RibbonPanelGlyph(bs, this, this.Tab));
            }
        }

        public void AddPanel(object sender, EventArgs e)
        {
            var host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;

            if (host != null && this.Tab != null)
            {
                var transaction = host.CreateTransaction("AddPanel" + this.Component.Site.Name);
                MemberDescriptor member = TypeDescriptor.GetProperties(this.Component)["Panels"];
                this.RaiseComponentChanging(member);

                var panel = host.CreateComponent(typeof(RibbonPanel)) as RibbonPanel;

                if (panel != null)
                {
                    panel.Text = panel.Site.Name;

                    //Michael Spradlin 07/05/2013 Added Panel Index code so we can tell where a panel is at on the ribbon.
                    panel.Index = this.Tab.Panels.Count;

                    if (panel.Index == 0)
                    {
                        panel.IsFirstPanel = true;
                    }
                    else
                    {
                        foreach (var pnl in this.Tab.Panels)
                        {
                            pnl.IsLastPanel = false;
                        }

                        panel.IsLastPanel = true;
                    }

                    this.Tab.Panels.Add(panel);
                    this.Tab.Owner.OnRegionsChanged();
                }

                this.RaiseComponentChanged(member, null, null);
                transaction.Commit();
            }
        }
    }
}