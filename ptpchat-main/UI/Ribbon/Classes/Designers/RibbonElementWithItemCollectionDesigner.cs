namespace PtpChat.Main.Ribbon.Classes.Designers
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;

    using PtpChat.Main.Ribbon.Classes.Collections;
    using PtpChat.Main.Ribbon.Component_Classes;

    internal abstract class RibbonElementWithItemCollectionDesigner : ComponentDesigner
    {
        #region Props

        /// <summary>
        /// Gets a reference to the Ribbon that owns the item
        /// </summary>
        public abstract Ribbon Ribbon { get; }

        /// <summary>
        /// Gets the collection of items hosted by this item
        /// </summary>
        public abstract RibbonItemCollection Collection { get; }

        /// <summary>
        /// Called when verbs must be retrieved
        /// </summary>
        /// <returns></returns>
        protected virtual DesignerVerbCollection OnGetVerbs()
        {
            return
                new DesignerVerbCollection(
                    new[]
                        {
                            new DesignerVerb("Add Button", this.AddButton), new DesignerVerb("Add ButtonList", this.AddButtonList), new DesignerVerb("Add ItemGroup", this.AddItemGroup),
                            new DesignerVerb("Add Separator", this.AddSeparator), new DesignerVerb("Add TextBox", this.AddTextBox), new DesignerVerb("Add ComboBox", this.AddComboBox),
                            new DesignerVerb("Add ColorChooser", this.AddColorChooser), new DesignerVerb("Add CheckBox", this.AddCheckBox), new DesignerVerb("Add UpDown", this.AddUpDown),
                            new DesignerVerb("Add Label", this.AddLabel), new DesignerVerb("Add Host", this.AddHost)
                        });
        }

        /// <summary>
        /// Overriden. Passes the verbs to the designer
        /// </summary>
        public override DesignerVerbCollection Verbs => this.OnGetVerbs();

        #endregion

        #region Methods

        /// <summary>
        /// Creates an item of the speciifed type
        /// </summary>
        /// <param name="t"></param>
        private void CreateItem(Type t)
        {
            this.CreateItem(this.Ribbon, this.Collection, t);
        }

        /// <summary>
        /// Creates an item of the specified type and adds it to the specified collection
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="collection"></param>
        /// <param name="t"></param>
        protected virtual void CreateItem(Ribbon ribbon, RibbonItemCollection collection, Type t)
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

        protected virtual void AddButton(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonButton));
        }

        protected virtual void AddButtonList(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonButtonList));
        }

        protected virtual void AddItemGroup(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonItemGroup));
        }

        protected virtual void AddSeparator(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonSeparator));
        }

        protected virtual void AddTextBox(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonTextBox));
        }

        protected virtual void AddComboBox(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonComboBox));
        }

        protected virtual void AddColorChooser(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonColorChooser));
        }

        protected virtual void AddDescriptionMenuItem(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonDescriptionMenuItem));
        }

        protected virtual void AddCheckBox(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonCheckBox));
        }

        protected virtual void AddUpDown(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonUpDown));
        }

        protected virtual void AddLabel(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonLabel));
        }

        protected virtual void AddHost(object sender, EventArgs e)
        {
            this.CreateItem(typeof(RibbonHost));
        }

        #endregion
    }
}