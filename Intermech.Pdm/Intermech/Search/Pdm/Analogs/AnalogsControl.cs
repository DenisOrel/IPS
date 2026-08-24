// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Analogs.AnalogsControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Descriptos;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Navigator;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Analogs;

public sealed class AnalogsControl : UserControl, ISupportInitialize
{
  private long _objectVersionID;
  private BindingList<CompositionPart> _compositionParts = new BindingList<CompositionPart>();
  private LazyService<INamedImageList> _namedImageList = new LazyService<INamedImageList>();
  private LazyService<IObjectCreatorService> _objectCreatorService = new LazyService<IObjectCreatorService>();
  private IContainer components;
  private ContextMenuStrip _contextMenuStrip;
  private ToolStripMenuItem _addToolStripMenuItem;
  private ToolStripMenuItem _createToolStripMenuItem;
  private ToolStripMenuItem _removeToolStripMenuItem;
  private ToolStripMenuItem _cardToolStripMenuItem;
  private ToolStrip _toolStrip;
  private ToolStripButton _addToolStripButton;
  private ToolStripButton _createToolStripButton;
  private ToolStripButton _removeToolStripButton;
  private ToolStripButton _cardToolStripButton;
  private NavigatorGrid _navigatorGrid;

  public AnalogsControl()
  {
    this.InitializeComponent();
    this.UpdateControl();
    this._navigatorGrid.DataSource = (object) this._compositionParts;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long ObjectVersionID
  {
    get => this._objectVersionID;
    set
    {
      if (this._objectVersionID == value)
        return;
      this._objectVersionID = value;
      this.ReloadCompositionParts();
    }
  }

  public void BeginInit()
  {
    if (this.DesignMode)
      return;
    this._cardToolStripButton.Image = this._cardToolStripMenuItem.Image = this._namedImageList.Value.ImageList.Images[this._namedImageList.Value.ImageIndex("imgCard")];
    this._navigatorGrid.SetNodeColumns(new NodeColumnCollection()
    {
      new NodeColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_ID, typeof (long), FieldTypes.ftSystem, ObligatoryObjectAttributesHelper.GetCaption(ObligatoryObjectAttributes.F_OBJECT_ID)),
      new NodeColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.CAPTION, typeof (string), FieldTypes.ftSystem, ObligatoryObjectAttributesHelper.GetCaption(ObligatoryObjectAttributes.CAPTION)),
      new NodeColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_LEVEL_ID, typeof (string), FieldTypes.ftSystem, ObligatoryObjectAttributesHelper.GetCaption(ObligatoryObjectAttributes.F_LEVEL_ID)),
      new NodeColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) AnalogsConstants.StartDateAttributeTypeID, typeof (DateTime), FieldTypes.ftDateTime, MetaDataHelper.GetAttributeTypeName(AnalogsConstants.StartDateAttributeTypeID)),
      new NodeColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) AnalogsConstants.EndDateAttributeTypeID, typeof (DateTime), FieldTypes.ftDateTime, MetaDataHelper.GetAttributeTypeName(AnalogsConstants.EndDateAttributeTypeID)),
      new NodeColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) AnalogsConstants.PriorityAnalogAttributeTypeID, typeof (bool), FieldTypes.ftBoolean, MetaDataHelper.GetAttributeTypeName(AnalogsConstants.PriorityAnalogAttributeTypeID))
    });
  }

  public void EndInit()
  {
  }

  private void AddToolStripButton_Click(object sender, EventArgs e) => this.Add();

  private void CreateToolStripButton_Click(object sender, EventArgs e) => this.Create();

  private void RemoveToolStripButton_Click(object sender, EventArgs e) => this.Remove();

  private void CardToolStripButton_Click(object sender, EventArgs e) => this.Card();

  private void AddToolStripMenuItem_Click(object sender, EventArgs e) => this.Add();

  private void CreateToolStripMenuItem_Click(object sender, EventArgs e) => this.Create();

  private void RemoveToolStripMenuItem_Click(object sender, EventArgs e) => this.Remove();

  private void CardToolStripMenuItem_Click(object sender, EventArgs e) => this.Card();

  private void NavigatorGrid_SelectionChanged(object sender, EventArgs e) => this.UpdateControl();

  private void UpdateControl()
  {
    this._addToolStripButton.Enabled = this._addToolStripMenuItem.Enabled = this.CanAdd();
    this._createToolStripButton.Enabled = this._createToolStripMenuItem.Enabled = this.CanCreate();
    this._removeToolStripButton.Enabled = this._removeToolStripMenuItem.Enabled = this.CanRemove();
    this._cardToolStripButton.Enabled = this._cardToolStripMenuItem.Enabled = this.CanCard();
  }

  private void ReloadCompositionParts()
  {
    object[] selectedItems = this._navigatorGrid.SelectedItems;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ICompositionRepositoryServerService customService = (ICompositionRepositoryServerService) sessionKeeper.Session.GetCustomService(typeof (ICompositionRepositoryServerService));
        FindCompositionParams findCompositionParams = new FindCompositionParams()
        {
          AllPartTypes = true,
          ObjectAttributeTypeIds = new int[5]
          {
            -2,
            -50,
            -9,
            -6,
            -7
          },
          ProjectVersionID = this._objectVersionID,
          RelationAttributeTypeIds = new int[4]
          {
            -20,
            AnalogsConstants.EndDateAttributeTypeID,
            AnalogsConstants.PriorityAnalogAttributeTypeID,
            AnalogsConstants.StartDateAttributeTypeID
          },
          RelationTypeID = AnalogsConstants.AnalogsRelationTypeID
        };
        CompositionPart[] composition = customService.FindComposition(sessionKeeper.Session.SessionGUID, findCompositionParams);
        bool listChangedEvents = this._compositionParts.RaiseListChangedEvents;
        this._compositionParts.RaiseListChangedEvents = false;
        try
        {
          this._compositionParts.Clear();
          foreach (CompositionPart compositionPart in composition)
            this._compositionParts.Add(compositionPart);
        }
        finally
        {
          this._compositionParts.RaiseListChangedEvents = listChangedEvents;
        }
        this._compositionParts.ResetBindings();
      }
    }
    finally
    {
      List<object> objectList = new List<object>();
      foreach (CompositionPart compositionPart1 in selectedItems)
      {
        CompositionPart compositionPart = compositionPart1;
        CompositionPart compositionPart2 = this._compositionParts.FirstOrDefault<CompositionPart>((Func<CompositionPart, bool>) (o => o.Relation.ID == compositionPart.Relation.ID));
        if (compositionPart2 != null)
          objectList.Add((object) compositionPart2);
      }
      this._navigatorGrid.SelectedItems = objectList.ToArray();
    }
  }

  private bool CanAdd() => true;

  private bool CanCreate() => true;

  private bool CanRemove() => this._navigatorGrid.SelectedItems.Length != 0;

  private bool CanCard() => this._navigatorGrid.SelectedItems.Length != 0;

  private void Add()
  {
    int[] source = (int[]) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      source = MetaDataHelper.GetObjectTypeApplicabilities(sessionKeeper.Session.GetObject(this._objectVersionID).TypeID).Where<IMSApplicability>((Func<IMSApplicability, bool>) (o => o.RelationTypeID == AnalogsConstants.AnalogsRelationTypeID)).Select<IMSApplicability, int>((Func<IMSApplicability, int>) (o => o.ChildObjectTypeID)).ToArray<int>();
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects("Intermech Professional Solution", "Выберите объекты для добавления в список аналогов", (IDescriptor) new ObjectTypesDescriptor(((IEnumerable<int>) source).ToArray<int>(), "Типы объектов"), (IServiceProvider) ServicesManager.ServiceContainer, SelectionOptions.SelectObjects) ?? new long[0];
    if (numArray.Length == 0)
      return;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        using (NotificationContext.Create(sessionKeeper.Session, (object) this))
        {
          IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(AnalogsConstants.AnalogsRelationTypeID);
          foreach (long objectID in numArray)
          {
            IDBObject dbObject = sessionKeeper.Session.GetObject(objectID);
            NewRelationProperties properties = new NewRelationProperties()
            {
              BeginDate = DateTime.MinValue,
              EndDate = DateTime.MaxValue,
              PartID = dbObject.ID,
              PartObjectID = objectID,
              ProjectObjectID = this._objectVersionID
            };
            relationCollection.Create(properties);
          }
        }
      }
    }
    finally
    {
      this.ReloadCompositionParts();
    }
  }

  private void Create()
  {
    Hashtable aObjectTypeIDRelationTypeIDs = new Hashtable();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (IMSApplicability imsApplicability in MetaDataHelper.GetObjectTypeApplicabilities(sessionKeeper.Session.GetObject(this._objectVersionID).TypeID).Where<IMSApplicability>((Func<IMSApplicability, bool>) (o => o.RelationTypeID == AnalogsConstants.AnalogsRelationTypeID)))
        aObjectTypeIDRelationTypeIDs.Add((object) imsApplicability.ChildObjectTypeID, (object) AnalogsConstants.AnalogsRelationTypeID);
    }
    if (ObjectHelper.IsUnknownObjectID(this._objectCreatorService.Value.CreateObjectByTypeDialog(aObjectTypeIDRelationTypeIDs, new long[1]
    {
      this._objectVersionID
    })))
      return;
    this.ReloadCompositionParts();
  }

  private void Remove()
  {
    long[] array = ((IEnumerable<object>) this._navigatorGrid.SelectedItems).Select<object, long>((Func<object, long>) (o => ((RelationObjectBase) o).Relation.ID)).ToArray<long>();
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        using (NotificationContext.Create(sessionKeeper.Session, (object) this))
        {
          foreach (long aRelationID in array)
            sessionKeeper.Session.GetRelation(aRelationID).Delete((long) Intermech.Consts.PurgeMode);
        }
      }
    }
    finally
    {
      this.ReloadCompositionParts();
    }
  }

  private void Card()
  {
    CompositionPart selectedItem = (CompositionPart) this._navigatorGrid.SelectedItem;
    int num = (int) PropertiesWindow.Execute(SelectedItemsHelper.CreateSelectedItemsForCompositionPart(selectedItem.Relation.ID, selectedItem.Object.VersionID), "RelationProperties");
    this.ReloadCompositionParts();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AnalogsControl));
    this._contextMenuStrip = new ContextMenuStrip(this.components);
    this._addToolStripMenuItem = new ToolStripMenuItem();
    this._createToolStripMenuItem = new ToolStripMenuItem();
    this._removeToolStripMenuItem = new ToolStripMenuItem();
    this._cardToolStripMenuItem = new ToolStripMenuItem();
    this._toolStrip = new ToolStrip();
    this._addToolStripButton = new ToolStripButton();
    this._createToolStripButton = new ToolStripButton();
    this._removeToolStripButton = new ToolStripButton();
    this._cardToolStripButton = new ToolStripButton();
    this._navigatorGrid = new NavigatorGrid();
    this._contextMenuStrip.SuspendLayout();
    this._toolStrip.SuspendLayout();
    this._navigatorGrid.BeginInit();
    this.SuspendLayout();
    this._contextMenuStrip.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this._addToolStripMenuItem,
      (ToolStripItem) this._createToolStripMenuItem,
      (ToolStripItem) this._removeToolStripMenuItem,
      (ToolStripItem) this._cardToolStripMenuItem
    });
    this._contextMenuStrip.Name = "_contextMenuStrip";
    this._contextMenuStrip.Size = new Size((int) sbyte.MaxValue, 92);
    this._addToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("_addToolStripMenuItem.Image");
    this._addToolStripMenuItem.Name = "_addToolStripMenuItem";
    this._addToolStripMenuItem.Size = new Size(126, 22);
    this._addToolStripMenuItem.Text = "Добавить";
    this._addToolStripMenuItem.Click += new EventHandler(this.AddToolStripMenuItem_Click);
    this._createToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("_createToolStripMenuItem.Image");
    this._createToolStripMenuItem.Name = "_createToolStripMenuItem";
    this._createToolStripMenuItem.Size = new Size(126, 22);
    this._createToolStripMenuItem.Text = "Создать";
    this._createToolStripMenuItem.Click += new EventHandler(this.CreateToolStripMenuItem_Click);
    this._removeToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("_removeToolStripMenuItem.Image");
    this._removeToolStripMenuItem.Name = "_removeToolStripMenuItem";
    this._removeToolStripMenuItem.Size = new Size(126, 22);
    this._removeToolStripMenuItem.Text = "Удалить";
    this._removeToolStripMenuItem.Click += new EventHandler(this.RemoveToolStripMenuItem_Click);
    this._cardToolStripMenuItem.Name = "_cardToolStripMenuItem";
    this._cardToolStripMenuItem.Size = new Size(126, 22);
    this._cardToolStripMenuItem.Text = "Карточка";
    this._cardToolStripMenuItem.Click += new EventHandler(this.CardToolStripMenuItem_Click);
    this._toolStrip.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this._addToolStripButton,
      (ToolStripItem) this._createToolStripButton,
      (ToolStripItem) this._removeToolStripButton,
      (ToolStripItem) this._cardToolStripButton
    });
    this._toolStrip.Location = new Point(0, 0);
    this._toolStrip.Name = "_toolStrip";
    this._toolStrip.Size = new Size(783, 25);
    this._toolStrip.TabIndex = 4;
    this._toolStrip.Text = "toolStrip1";
    this._addToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._addToolStripButton.Image = (Image) componentResourceManager.GetObject("_addToolStripButton.Image");
    this._addToolStripButton.ImageTransparentColor = Color.Magenta;
    this._addToolStripButton.Name = "_addToolStripButton";
    this._addToolStripButton.Size = new Size(23, 22);
    this._addToolStripButton.Text = "Добавить";
    this._addToolStripButton.Click += new EventHandler(this.AddToolStripButton_Click);
    this._createToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._createToolStripButton.Image = (Image) componentResourceManager.GetObject("_createToolStripButton.Image");
    this._createToolStripButton.ImageTransparentColor = Color.Magenta;
    this._createToolStripButton.Name = "_createToolStripButton";
    this._createToolStripButton.Size = new Size(23, 22);
    this._createToolStripButton.Text = "Создать";
    this._createToolStripButton.Click += new EventHandler(this.CreateToolStripButton_Click);
    this._removeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._removeToolStripButton.Image = (Image) componentResourceManager.GetObject("_removeToolStripButton.Image");
    this._removeToolStripButton.ImageTransparentColor = Color.Magenta;
    this._removeToolStripButton.Name = "_removeToolStripButton";
    this._removeToolStripButton.Size = new Size(23, 22);
    this._removeToolStripButton.Text = "Удалить";
    this._removeToolStripButton.Click += new EventHandler(this.RemoveToolStripButton_Click);
    this._cardToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._cardToolStripButton.Image = (Image) componentResourceManager.GetObject("_cardToolStripButton.Image");
    this._cardToolStripButton.ImageTransparentColor = Color.Magenta;
    this._cardToolStripButton.Name = "_cardToolStripButton";
    this._cardToolStripButton.Size = new Size(23, 22);
    this._cardToolStripButton.Text = "Карточка (Свойства)";
    this._cardToolStripButton.Click += new EventHandler(this.CardToolStripButton_Click);
    this._navigatorGrid.ContextMenuStrip = this._contextMenuStrip;
    this._navigatorGrid.Dock = DockStyle.Fill;
    this._navigatorGrid.Location = new Point(0, 25);
    this._navigatorGrid.Name = "_navigatorGrid";
    this._navigatorGrid.ShowIconColumn = true;
    this._navigatorGrid.Size = new Size(783, 283);
    this._navigatorGrid.TabIndex = 5;
    this._navigatorGrid.SelectionChanged += new EventHandler(this.NavigatorGrid_SelectionChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._navigatorGrid);
    this.Controls.Add((Control) this._toolStrip);
    this.Name = nameof (AnalogsControl);
    this.Size = new Size(783, 308);
    this._contextMenuStrip.ResumeLayout(false);
    this._toolStrip.ResumeLayout(false);
    this._toolStrip.PerformLayout();
    this._navigatorGrid.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
