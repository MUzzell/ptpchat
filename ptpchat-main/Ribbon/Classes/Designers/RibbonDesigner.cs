namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;
    using System.Windows.Forms.Design.Behavior;

    using IContainsRibbonComponents = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsRibbonComponents;
    using IContainsSelectableRibbonItems = PtpChat.Main.Ribbon.Classes.Interfaces.IContainsSelectableRibbonItems;
    using IRibbonElement = PtpChat.Main.Ribbon.Classes.Interfaces.IRibbonElement;
    using Ribbon = PtpChat.Main.Ribbon.Component_Classes.Ribbon;
    using RibbonButton = PtpChat.Main.Ribbon.Component_Classes.RibbonButton;
    using RibbonItem = PtpChat.Main.Ribbon.Component_Classes.RibbonItem;
    using RibbonItemCollection = PtpChat.Main.Ribbon.Classes.Collections.RibbonItemCollection;
    using RibbonItemGroup = PtpChat.Main.Ribbon.Component_Classes.RibbonItemGroup;
    using RibbonOrbDropDown = PtpChat.Main.Ribbon.Component_Classes.RibbonOrbDropDown;
    using RibbonPanel = PtpChat.Main.Ribbon.Component_Classes.RibbonPanel;
    using RibbonQuickAccessToolbarGlyph = PtpChat.Main.Ribbon.Classes.Glyphs.RibbonQuickAccessToolbarGlyph;
    using RibbonSeparator = PtpChat.Main.Ribbon.Component_Classes.RibbonSeparator;
    using RibbonTab = PtpChat.Main.Ribbon.Component_Classes.RibbonTab;
    using RibbonTabGlyph = PtpChat.Main.Ribbon.Classes.Glyphs.RibbonTabGlyph;

    public class RibbonDesigner : ControlDesigner
    {
        #region Static

        public static RibbonDesigner Current;

        #endregion

        #region Fields

        private IRibbonElement _selectedElement;

        private Adorner quickAccessAdorner;

        private Adorner orbAdorner;

        private Adorner tabAdorner;

        #endregion

        #region Ctor

        public RibbonDesigner()
        {
            Current = this;
        }

        ~RibbonDesigner()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently selected RibbonElement
        /// </summary>
        public IRibbonElement SelectedElement
        {
            get { return this._selectedElement; }
            set
            {
                if (this.Ribbon == null)
                {
                    return;
                }
                this._selectedElement = value;

                var selector = this.GetService(typeof(ISelectionService)) as ISelectionService;

                if (selector != null && value != null)
                {
                    selector.SetSelectedComponents(new[] { value as Component }, SelectionTypes.Primary);
                }

                if (value is RibbonButton)
                {
                    (value as RibbonButton).ShowDropDown();
                }

                this.Ribbon.Refresh();
            }
        }

        /// <summary>
        /// Gets the Ribbon of the designer
        /// </summary>
        public Ribbon Ribbon => this.Control as Ribbon;

        #endregion

        #region Methods

        public virtual void CreateItem(Ribbon ribbon, RibbonItemCollection collection, Type t)
        {
            var host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;

            if (host != null && collection != null && ribbon != null)
            {
                var transaction = host.CreateTransaction("AddRibbonItem_" + this.Component.Site.Name);

                MemberDescriptor member = TypeDescriptor.GetProperties(this.Component)["Items"];
                this.RaiseComponentChanging(member);

                var item = host.CreateComponent(t) as RibbonItem;

                if (!(item is RibbonSeparator))
                {
                    item.Text = item.Site.Name;
                }

                collection.Add(item);
                ribbon.OnRegionsChanged();

                this.RaiseComponentChanged(member, null, null);
                transaction.Commit();
            }
        }

        private void CreateOrbItem(string collectionName, RibbonItemCollection collection, Type t)
        {
            if (this.Ribbon == null)
            {
                return;
            }

            var host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
            var transaction = host.CreateTransaction("AddRibbonOrbItem_" + this.Component.Site.Name);
            MemberDescriptor member = TypeDescriptor.GetProperties(this.Ribbon.OrbDropDown)[collectionName];
            this.RaiseComponentChanging(member);

            var item = host.CreateComponent(t) as RibbonItem;

            if (!(item is RibbonSeparator))
            {
                item.Text = item.Site.Name;
            }

            collection.Add(item);
            this.Ribbon.OrbDropDown.OnRegionsChanged();

            this.RaiseComponentChanged(member, null, null);
            transaction.Commit();

            this.Ribbon.OrbDropDown.SelectOnDesigner(item);
            this.Ribbon.OrbDropDown.WrappedDropDown.Size = this.Ribbon.OrbDropDown.Size;
        }

        /// <summary>
        /// Creates an Orb's MenuItem
        /// </summary>
        /// <param name="t"></param>
        public void CreteOrbMenuItem(Type t)
        {
            this.CreateOrbItem("MenuItems", this.Ribbon.OrbDropDown.MenuItems, t);
        }

        /// <summary>
        /// Creates an Orb's RecentItem
        /// </summary>
        /// <param name="t"></param>
        public void CreteOrbRecentItem(Type t)
        {
            this.CreateOrbItem("RecentItems", this.Ribbon.OrbDropDown.RecentItems, t);
        }

        /// <summary>
        /// Creates an Orb's OptionItem
        /// </summary>
        /// <param name="t"></param>
        public void CreteOrbOptionItem(Type t)
        {
            this.CreateOrbItem("OptionItems", this.Ribbon.OrbDropDown.OptionItems, t);
        }

        private void AssignEventHandler()
        {
            //TODO: Didn't work
            //if (SelectedElement == null) return;

            //IEventBindingService binder = GetService(typeof(IEventBindingService)) as IEventBindingService;

            //EventDescriptorCollection evts = TypeDescriptor.GetEvents(SelectedElement);

            ////string id = binder.CreateUniqueMethodName(SelectedElement as Component, evts["Click"]);

            //binder.ShowCode(SelectedElement as Component, evts["Click"]);
        }

        private void SelectRibbon()
        {
            var selector = this.GetService(typeof(ISelectionService)) as ISelectionService;

            if (selector != null)
            {
                selector.SetSelectedComponents(new Component[] { this.Ribbon }, SelectionTypes.Primary);
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                var verbs = new DesignerVerbCollection();

                verbs.Add(new DesignerVerb("Add Tab", this.AddTabVerb));

                return verbs;
            }
        }

        public void AddTabVerb(object sender, EventArgs e)
        {
            var r = this.Control as Ribbon;

            if (r != null)
            {
                var host = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (host == null)
                {
                    return;
                }

                var tab = host.CreateComponent(typeof(RibbonTab)) as RibbonTab;

                if (tab == null)
                {
                    return;
                }

                tab.Text = tab.Site.Name;

                this.Ribbon.Tabs.Add(tab);

                r.Refresh();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.HWnd == this.Control.Handle)
            {
                switch (m.Msg)
                {
                    case 0x203: //WM_LBUTTONDBLCLK
                        this.AssignEventHandler();
                        break;
                    case 0x201: //WM_LBUTTONDOWN
                    case 0x204: //WM_RBUTTONDOWN
                        return;
                    case 0x202: //WM_LBUTTONUP
                    case 0x205: //WM_RBUTTONUP
                        this.HitOn(WinApi.LoWord((int)m.LParam), WinApi.HiWord((int)m.LParam));
                        return;
                    default:
                        break;
                }
            }

            base.WndProc(ref m);
        }

        private void HitOn(int x, int y)
        {
            if (this.Ribbon.Tabs.Count == 0 || this.Ribbon.ActiveTab == null)
            {
                this.SelectRibbon();
                return;
            }

            if (this.Ribbon != null)
            {
                if (this.Ribbon.TabHitTest(x, y))
                {
                    this.SelectedElement = this.Ribbon.ActiveTab;
                }
                else
                {
                    #region Tab ScrollTest

                    if (this.Ribbon.ActiveTab.TabContentBounds.Contains(x, y))
                    {
                        if (this.Ribbon.ActiveTab.ScrollLeftBounds.Contains(x, y) && this.Ribbon.ActiveTab.ScrollLeftVisible)
                        {
                            this.Ribbon.ActiveTab.ScrollLeft();
                            this.SelectedElement = this.Ribbon.ActiveTab;
                            return;
                        }

                        if (this.Ribbon.ActiveTab.ScrollRightBounds.Contains(x, y) && this.Ribbon.ActiveTab.ScrollRightVisible)
                        {
                            this.Ribbon.ActiveTab.ScrollRight();
                            this.SelectedElement = this.Ribbon.ActiveTab;
                            return;
                        }
                    }

                    #endregion

                    //Check Panel
                    if (this.Ribbon.ActiveTab.TabContentBounds.Contains(x, y))
                    {
                        RibbonPanel hittedPanel = null;

                        foreach (var panel in this.Ribbon.ActiveTab.Panels)
                        {
                            if (panel.Bounds.Contains(x, y))
                            {
                                hittedPanel = panel;
                                break;
                            }
                        }

                        if (hittedPanel != null)
                        {
                            //Check item
                            RibbonItem hittedItem = null;

                            foreach (var item in hittedPanel.Items)
                            {
                                if (item.Bounds.Contains(x, y))
                                {
                                    hittedItem = item;
                                    break;
                                }
                            }

                            if (hittedItem != null && hittedItem is IContainsSelectableRibbonItems)
                            {
                                //Check subitem
                                RibbonItem hittedSubItem = null;

                                foreach (var subItem in (hittedItem as IContainsSelectableRibbonItems).GetItems())
                                {
                                    if (subItem.Bounds.Contains(x, y))
                                    {
                                        hittedSubItem = subItem;
                                        break;
                                    }
                                }

                                if (hittedSubItem != null)
                                {
                                    this.SelectedElement = hittedSubItem;
                                }
                                else
                                {
                                    this.SelectedElement = hittedItem;
                                }
                            }
                            else if (hittedItem != null)
                            {
                                this.SelectedElement = hittedItem;
                            }
                            else
                            {
                                this.SelectedElement = hittedPanel;
                            }
                        }
                        else
                        {
                            this.SelectedElement = this.Ribbon.ActiveTab;
                        }
                    }
                    else if (this.Ribbon.QuickAcessToolbar.SuperBounds.Contains(x, y))
                    {
                        var itemHitted = false;

                        foreach (var item in this.Ribbon.QuickAcessToolbar.Items)
                        {
                            if (item.Bounds.Contains(x, y))
                            {
                                itemHitted = true;
                                this.SelectedElement = item;
                                break;
                            }
                        }
                        if (!itemHitted)
                        {
                            this.SelectedElement = this.Ribbon.QuickAcessToolbar;
                        }
                    }
                    else if (this.Ribbon.OrbBounds.Contains(x, y))
                    {
                        this.Ribbon.OrbMouseDown();
                    }
                    else
                    {
                        this.SelectRibbon();

                        this.Ribbon.ForceOrbMenu = false;
                        if (this.Ribbon.OrbDropDown.Visible)
                        {
                            this.Ribbon.OrbDropDown.Close();
                        }
                    }
                }
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            base.OnPaintAdornments(pe);

            using (var p = new Pen(Color.Black))
            {
                p.DashStyle = DashStyle.Dot;

                var host = this.GetService(typeof(ISelectionService)) as ISelectionService;

                if (host != null)
                {
                    foreach (IComponent comp in host.GetSelectedComponents())
                    {
                        var item = comp as RibbonItem;
                        if (item != null && !this.Ribbon.OrbDropDown.AllItems.Contains(item))
                        {
                            pe.Graphics.DrawRectangle(p, item.Bounds);
                        }
                    }
                }
            }
        }

        #endregion

        #region Site

        public BehaviorService GetBehaviorService()
        {
            return this.BehaviorService;
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            var changeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            var desigerEvt = this.GetService(typeof(IDesignerEventService)) as IDesignerEventService;

            changeService.ComponentRemoved += this.changeService_ComponentRemoved;

            this.orbAdorner = new Adorner();
            this.tabAdorner = new Adorner();

            this.BehaviorService.Adorners.AddRange(new[] { this.orbAdorner, this.tabAdorner });
            if (this.Ribbon.QuickAcessToolbar.Visible)
            {
                this.quickAccessAdorner = new Adorner();
                this.BehaviorService.Adorners.Add(this.quickAccessAdorner);
                this.quickAccessAdorner.Glyphs.Add(new RibbonQuickAccessToolbarGlyph(this.BehaviorService, this, this.Ribbon));
            }
            else
            {
                this.quickAccessAdorner = null;
            }
            //orbAdorner.Glyphs.Add(new RibbonOrbAdornerGlyph(BehaviorService, this, Ribbon));
            this.tabAdorner.Glyphs.Add(new RibbonTabGlyph(this.BehaviorService, this, this.Ribbon));
        }

        /// <summary>
        /// Catches the event of a component on the ribbon being removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void changeService_ComponentRemoved(object sender, ComponentEventArgs e)
        {
            var tab = e.Component as RibbonTab;
            var panel = e.Component as RibbonPanel;
            var item = e.Component as RibbonItem;

            var designerService = this.GetService(typeof(IDesignerHost)) as IDesignerHost;

            this.RemoveRecursive(e.Component as IContainsRibbonComponents, designerService);

            if (tab != null && this.Ribbon != null)
            {
                this.Ribbon.Tabs.Remove(tab);
            }
            else if (panel != null)
            {
                panel.OwnerTab.Panels.Remove(panel);
            }
            else if (item != null)
            {
                if (item.Canvas is RibbonOrbDropDown)
                {
                    this.Ribbon.OrbDropDown.HandleDesignerItemRemoved(item);
                }
                else if (item.OwnerItem is RibbonItemGroup)
                {
                    (item.OwnerItem as RibbonItemGroup).Items.Remove(item);
                }
                else if (item.OwnerPanel != null)
                {
                    item.OwnerPanel.Items.Remove(item);
                }
                else if (this.Ribbon != null && this.Ribbon.QuickAcessToolbar.Items.Contains(item))
                {
                    this.Ribbon.QuickAcessToolbar.Items.Remove(item);
                }
            }

            this.SelectedElement = null;

            if (this.Ribbon != null)
            {
                this.Ribbon.OnRegionsChanged();
            }
        }

        public void RemoveRecursive(IContainsRibbonComponents item, IDesignerHost service)
        {
            if (item == null || service == null)
            {
                return;
            }

            foreach (var c in item.GetAllChildComponents())
            {
                if (c is IContainsRibbonComponents)
                {
                    this.RemoveRecursive(c as IContainsRibbonComponents, service);
                }
                service.DestroyComponent(c);
            }
        }

        #endregion
    }
}