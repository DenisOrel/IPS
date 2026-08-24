// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SettingsSyncAttributes.ArticleAttributesSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.SettingsSyncAttributes;

public class ArticleAttributesSettings : UserControl, IPropertyPage, IPropertyPageSearchOptionEvents
{
  private List<string> haveIDDoc = new List<string>();
  private List<string> haveIDAttr = new List<string>();
  private List<Icon> _iconsList = new List<Icon>();
  private IArticleAttributesSyncService _substs;
  private bool _inEvent;
  private IServiceProvider _provider;
  private bool isModified;
  private IContainer components;
  private ListBoxWithIcons lbMainDocumentsTypes;
  private ListBoxWithIcons lbSyncAttributes;
  private GroupBox groupBox1;
  private GroupBox groupBox2;
  private ContextMenuStrip cmsEdit;
  private ToolStripMenuItem addItem;
  private ToolStripMenuItem deletedItem;
  private ContextMenuStrip cmsEditAttr;
  private ToolStripMenuItem addAttr;
  private ToolStripMenuItem deleteAttr;
  private Button delDocButton;
  private Button addDocButton;
  private Button delAttrButton;
  private Button addAttrButton;

  public ArticleAttributesSettings(IServiceProvider provider)
  {
    this.InitializeComponent();
    this.lbMainDocumentsTypes.Items.Clear();
    this.lbSyncAttributes.Items.Clear();
    this._iconsList.Clear();
    this.lbMainDocumentsTypes._myImageList = this._iconsList;
    this._provider = provider;
    this.LoadSettings();
    if (!(this._provider.GetService(typeof (IPropertyPagesService)) is IPropertyPagesService service))
      return;
    service.AddPage(LocalizationHolder.rm.GetString("Pdm_Sync_attr"), (IPropertyPage) this);
  }

  private void LoadSettings()
  {
    this._inEvent = false;
    this.isModified = false;
    this.lbMainDocumentsTypes.Items.Clear();
    this.lbSyncAttributes.Items.Clear();
    this._iconsList.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ArticleAttributesSyncSettings attributesSyncSettings = ((IArticleAttributesSyncService) sessionKeeper.Session.GetCustomService(typeof (IArticleAttributesSyncService))).ReadSyncSett(sessionKeeper.Session.SessionGUID);
      foreach (int mainDocumentsType in attributesSyncSettings.MainDocumentsTypes)
      {
        IDBObjectType objectType = sessionKeeper.Session.GetObjectType(mainDocumentsType, false);
        if (objectType != null)
        {
          if (Statics.IconSrv != null)
          {
            try
            {
              this._iconsList.Add(Statics.IconSrv.GetIcon(4, mainDocumentsType));
              this.lbMainDocumentsTypes.Items.Add((object) new ListItems(mainDocumentsType, objectType.ObjectTypeName, this._iconsList.Count - 1));
            }
            catch
            {
              this.lbMainDocumentsTypes.Items.Add((object) new ListItems(mainDocumentsType, objectType.ObjectTypeName, -1));
            }
          }
          else if (objectType.Icon.Length != 0)
          {
            using (MemoryStream memoryStream = new MemoryStream(objectType.Icon))
            {
              try
              {
                this._iconsList.Add(new Icon((Stream) memoryStream));
                this.lbMainDocumentsTypes.Items.Add((object) new ListItems(mainDocumentsType, objectType.ObjectTypeName, this._iconsList.Count - 1));
              }
              catch
              {
                this.lbMainDocumentsTypes.Items.Add((object) new ListItems(mainDocumentsType, objectType.ObjectTypeName, -1));
              }
            }
          }
          this.haveIDDoc.Add(mainDocumentsType.ToString());
        }
      }
      foreach (int syncAttribute in attributesSyncSettings.SyncAttributes)
      {
        IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(syncAttribute, true);
        this.lbSyncAttributes.Items.Add((object) new ListItems(syncAttribute, attributeType.Name));
        this.haveIDAttr.Add(syncAttribute.ToString());
      }
    }
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => LocalizationHolder.rm.GetString("Pdm_sync_attrCaption");

  public void Apply()
  {
    if (!this.isModified)
      return;
    int[] mainDocumentsTypes = new int[this.lbMainDocumentsTypes.Items.Count];
    int[] syncAttributes = new int[this.lbSyncAttributes.Items.Count];
    for (int index = 0; index < this.lbMainDocumentsTypes.Items.Count; ++index)
    {
      if (this.lbMainDocumentsTypes.Items[index] is ListItems listItems)
        mainDocumentsTypes[index] = listItems.AttrID;
    }
    for (int index = 0; index < this.lbSyncAttributes.Items.Count; ++index)
    {
      if (this.lbSyncAttributes.Items[index] is ListItems listItems)
        syncAttributes[index] = listItems.AttrID;
    }
    ArticleAttributesSyncSettings settings = new ArticleAttributesSyncSettings(syncAttributes, mainDocumentsTypes);
    this.isModified = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ((IArticleAttributesSyncService) sessionKeeper.Session.GetCustomService(typeof (IArticleAttributesSyncService))).WriteSyncSettings(settings, sessionKeeper.Session.SessionGUID);
  }

  public void Cancel() => this.LoadSettings();

  public string HelpTopicID => "";

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public List<string> GetOptionNames()
  {
    return !(this.Control is System.Windows.Forms.Control control) ? new List<string>() : IPropertyPageHelper.GetOptionNames(control);
  }

  private void OnChanged()
  {
    if (this._inEvent)
      return;
    this.isModified = true;
    if (this.Changed == null)
      return;
    this._inEvent = true;
    this.Changed((object) this, new EventArgs());
  }

  private void lbMainDocumentsTypes_MouseUp(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right)
      return;
    int index = this.lbMainDocumentsTypes.IndexFromPoint(e.X, e.Y);
    if (index != -1)
    {
      this.lbMainDocumentsTypes.SetSelected(index, true);
      this.deletedItem.Enabled = true;
      this.cmsEdit.Show(System.Windows.Forms.Control.MousePosition);
    }
    else
    {
      this.deletedItem.Enabled = false;
      this.cmsEdit.Show(System.Windows.Forms.Control.MousePosition);
    }
  }

  private void lbSyncAttributes_MouseUp(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right)
      return;
    int index = this.lbSyncAttributes.IndexFromPoint(e.X, e.Y);
    if (index != -1)
    {
      this.lbSyncAttributes.SetSelected(index, true);
      this.deleteAttr.Enabled = true;
      this.cmsEditAttr.Show(System.Windows.Forms.Control.MousePosition);
    }
    else
    {
      this.deleteAttr.Enabled = false;
      this.cmsEditAttr.Show(System.Windows.Forms.Control.MousePosition);
    }
  }

  private void addItem_Click(object sender, EventArgs e)
  {
    SelectorForm selectorForm = new SelectorForm(typeof (DocumentsTypesFolder), "Документы", new System.Type[2]
    {
      typeof (DocumentTypesFolder),
      typeof (DocumentsTypesFolder)
    }, true);
    if (selectorForm.ShowDialog() != DialogResult.OK)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._inEvent = false;
      for (int index = 0; index < selectorForm.IDList.Count; ++index)
      {
        ListItems listItems = new ListItems(Convert.ToInt32(selectorForm.IDList[index]), selectorForm.NameList[index].ToString());
        List<string> haveIdDoc1 = this.haveIDDoc;
        int attrId = listItems.AttrID;
        string str1 = attrId.ToString();
        if (!haveIdDoc1.Contains(str1))
        {
          IDBObjectType objectType = sessionKeeper.Session.GetObjectType(listItems.AttrID, true);
          if (Statics.IconSrv != null)
          {
            try
            {
              this._iconsList.Add(Statics.IconSrv.GetIcon(4, objectType.ObjectType));
              listItems.ImageIndex = this._iconsList.Count - 1;
            }
            catch
            {
              listItems.ImageIndex = -1;
            }
          }
          else if (objectType.Icon.Length != 0)
          {
            using (MemoryStream memoryStream = new MemoryStream(objectType.Icon))
              this._iconsList.Add(new Icon((Stream) memoryStream));
            listItems.ImageIndex = this._iconsList.Count - 1;
          }
          this.lbMainDocumentsTypes.Items.Add((object) listItems);
          List<string> haveIdDoc2 = this.haveIDDoc;
          attrId = listItems.AttrID;
          string str2 = attrId.ToString();
          haveIdDoc2.Add(str2);
        }
      }
    }
    this.OnChanged();
  }

  private void deletedItem_Click(object sender, EventArgs e)
  {
    this._inEvent = false;
    this.haveIDDoc.Remove(((ListItems) this.lbMainDocumentsTypes.SelectedItem).AttrID.ToString());
    this.lbMainDocumentsTypes.Items.Remove(this.lbMainDocumentsTypes.SelectedItem);
    this.OnChanged();
  }

  private void addAttr_Click(object sender, EventArgs e)
  {
    SelectorForm selectorForm = new SelectorForm(typeof (DocumentsAttributesFolder), "Атрибуты", typeof (DocumentsAttributeFolder), true);
    selectorForm.AdditionalRoot = true;
    if (selectorForm.ShowDialog() != DialogResult.OK)
      return;
    this._inEvent = false;
    for (int index = 0; index < selectorForm.IDList.Count; ++index)
    {
      ListItems listItems = new ListItems(Convert.ToInt32(selectorForm.IDList[index]), selectorForm.NameList[index].ToString());
      List<string> haveIdAttr1 = this.haveIDAttr;
      int attrId = listItems.AttrID;
      string str1 = attrId.ToString();
      if (!haveIdAttr1.Contains(str1))
      {
        this.lbSyncAttributes.Items.Add((object) listItems);
        List<string> haveIdAttr2 = this.haveIDAttr;
        attrId = listItems.AttrID;
        string str2 = attrId.ToString();
        haveIdAttr2.Add(str2);
      }
    }
    this.OnChanged();
  }

  private void deleteAttr_Click(object sender, EventArgs e)
  {
    this._inEvent = false;
    this.haveIDAttr.Remove(((ListItems) this.lbSyncAttributes.SelectedItem).AttrID.ToString());
    this.lbSyncAttributes.Items.Remove(this.lbSyncAttributes.SelectedItem);
    this.OnChanged();
  }

  private void lbMainDocumentsTypes_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.delDocButton.Enabled = this.lbMainDocumentsTypes.SelectedIndex != -1;
    this.delAttrButton.Enabled = this.lbSyncAttributes.SelectedIndex != -1;
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
    this.groupBox1 = new GroupBox();
    this.delDocButton = new Button();
    this.addDocButton = new Button();
    this.groupBox2 = new GroupBox();
    this.delAttrButton = new Button();
    this.addAttrButton = new Button();
    this.cmsEdit = new ContextMenuStrip(this.components);
    this.addItem = new ToolStripMenuItem();
    this.deletedItem = new ToolStripMenuItem();
    this.cmsEditAttr = new ContextMenuStrip(this.components);
    this.addAttr = new ToolStripMenuItem();
    this.deleteAttr = new ToolStripMenuItem();
    this.lbMainDocumentsTypes = new ListBoxWithIcons();
    this.lbSyncAttributes = new ListBoxWithIcons();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.cmsEdit.SuspendLayout();
    this.cmsEditAttr.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.delDocButton);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.addDocButton);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.lbMainDocumentsTypes);
    this.groupBox1.Location = new Point(3, 1);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(410, 184);
    this.groupBox1.TabIndex = 4;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Типы объектов, являющиеся главными конструкторскими документами";
    this.delDocButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.delDocButton.Enabled = false;
    this.delDocButton.Location = new Point(274, 154);
    this.delDocButton.Name = "delDocButton";
    this.delDocButton.Size = new Size(130, 23);
    this.delDocButton.TabIndex = 1;
    this.delDocButton.Text = "Удалить тип объекта";
    this.delDocButton.UseVisualStyleBackColor = true;
    this.delDocButton.Click += new EventHandler(this.deletedItem_Click);
    this.addDocButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.addDocButton.Location = new Point(131, 154);
    this.addDocButton.Name = "addDocButton";
    this.addDocButton.Size = new Size(137, 23);
    this.addDocButton.TabIndex = 1;
    this.addDocButton.Text = "Добавить тип объекта";
    this.addDocButton.UseVisualStyleBackColor = true;
    this.addDocButton.Click += new EventHandler(this.addItem_Click);
    this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.delAttrButton);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.addAttrButton);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.lbSyncAttributes);
    this.groupBox2.Location = new Point(3, 191);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(410, 178);
    this.groupBox2.TabIndex = 500;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Синхронизируемые атрибуты";
    this.delAttrButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.delAttrButton.Enabled = false;
    this.delAttrButton.Location = new Point(284, 149);
    this.delAttrButton.Name = "delAttrButton";
    this.delAttrButton.Size = new Size(120, 23);
    this.delAttrButton.TabIndex = 1;
    this.delAttrButton.Text = "Удалить атрибут";
    this.delAttrButton.UseVisualStyleBackColor = true;
    this.delAttrButton.Click += new EventHandler(this.deleteAttr_Click);
    this.addAttrButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.addAttrButton.Location = new Point(157, 149);
    this.addAttrButton.Name = "addAttrButton";
    this.addAttrButton.Size = new Size(121, 23);
    this.addAttrButton.TabIndex = 1;
    this.addAttrButton.Text = "Добавить атрибут";
    this.addAttrButton.UseVisualStyleBackColor = true;
    this.addAttrButton.Click += new EventHandler(this.addAttr_Click);
    this.cmsEdit.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.addItem,
      (ToolStripItem) this.deletedItem
    });
    this.cmsEdit.Name = "cmsEdit";
    this.cmsEdit.Size = new Size(196, 48 /*0x30*/);
    this.addItem.Name = "addItem";
    this.addItem.Size = new Size(195, 22);
    this.addItem.Text = "Добавить тип объекта";
    this.addItem.Click += new EventHandler(this.addItem_Click);
    this.deletedItem.Name = "deletedItem";
    this.deletedItem.Size = new Size(195, 22);
    this.deletedItem.Text = "Удалить тип объекта";
    this.deletedItem.Click += new EventHandler(this.deletedItem_Click);
    this.cmsEditAttr.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.addAttr,
      (ToolStripItem) this.deleteAttr
    });
    this.cmsEditAttr.Name = "cmsEdit";
    this.cmsEditAttr.Size = new Size(173, 48 /*0x30*/);
    this.addAttr.Name = "addAttr";
    this.addAttr.Size = new Size(172, 22);
    this.addAttr.Text = "Добавить атрибут";
    this.addAttr.Click += new EventHandler(this.addAttr_Click);
    this.deleteAttr.Name = "deleteAttr";
    this.deleteAttr.Size = new Size(172, 22);
    this.deleteAttr.Text = "Удалить атрибут";
    this.deleteAttr.Click += new EventHandler(this.deleteAttr_Click);
    this.lbMainDocumentsTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lbMainDocumentsTypes.DrawMode = DrawMode.OwnerDrawFixed;
    this.lbMainDocumentsTypes.FormattingEnabled = true;
    this.lbMainDocumentsTypes.ItemHeight = 21;
    this.lbMainDocumentsTypes.Items.AddRange(new object[1]
    {
      (object) "1"
    });
    this.lbMainDocumentsTypes.Location = new Point(6, 19);
    this.lbMainDocumentsTypes.Name = "lbMainDocumentsTypes";
    this.lbMainDocumentsTypes.Size = new Size(398, 130);
    this.lbMainDocumentsTypes.TabIndex = 0;
    this.lbMainDocumentsTypes.SelectedIndexChanged += new EventHandler(this.lbMainDocumentsTypes_SelectedIndexChanged);
    this.lbMainDocumentsTypes.MouseUp += new MouseEventHandler(this.lbMainDocumentsTypes_MouseUp);
    this.lbSyncAttributes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lbSyncAttributes.DrawMode = DrawMode.OwnerDrawFixed;
    this.lbSyncAttributes.FormattingEnabled = true;
    this.lbSyncAttributes.Items.AddRange(new object[1]
    {
      (object) "1"
    });
    this.lbSyncAttributes.Location = new Point(6, 19);
    this.lbSyncAttributes.Name = "lbSyncAttributes";
    this.lbSyncAttributes.Size = new Size(398, 121);
    this.lbSyncAttributes.TabIndex = 1;
    this.lbSyncAttributes.SelectedIndexChanged += new EventHandler(this.lbMainDocumentsTypes_SelectedIndexChanged);
    this.lbSyncAttributes.MouseUp += new MouseEventHandler(this.lbSyncAttributes_MouseUp);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.AutoScroll = true;
    this.Controls.Add((System.Windows.Forms.Control) this.groupBox1);
    this.Controls.Add((System.Windows.Forms.Control) this.groupBox2);
    this.MinimumSize = new Size(410, 375);
    this.Name = nameof (ArticleAttributesSettings);
    this.Size = new Size(418, 375);
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.cmsEdit.ResumeLayout(false);
    this.cmsEditAttr.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
