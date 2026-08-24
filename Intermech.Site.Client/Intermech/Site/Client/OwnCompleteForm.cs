// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.OwnCompleteForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Site.Client.PortalNavigator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class OwnCompleteForm : Form
{
  private string _parentSites = string.Empty;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private GroupBox groupBox1;
  private ListView lvSites;
  private ColumnHeader columnHeader2;
  private RadioButton rbAllComposition;
  private RadioButton rbObjectOnly;
  private GroupBox groupBox2;
  private Panel panel5;
  private SplitContainer splitContainer1;
  private Panel panel2;
  private ListView lvObjects;
  private ColumnHeader columnHeader3;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader4;
  private CheckBox cbAutoUpdate;

  public OwnCompleteForm()
  {
    this.InitializeComponent();
    ICategoryTypeIconService service = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this.lvObjects.SmallImageList = service.ImageList;
    this.lvSites.SmallImageList = service.ImageList;
    FormStorage.LoadLayout((Control) this);
  }

  public string ParentSites => this._parentSites;

  public long[] Objects
  {
    get
    {
      List<long> longList = new List<long>();
      foreach (ListViewItem listViewItem in this.lvObjects.Items)
        longList.Add(((IPublishTypedID) listViewItem.Tag).ObjectID);
      return longList.ToArray();
    }
  }

  public Guid[] ObjectGuids
  {
    get
    {
      List<Guid> guidList = new List<Guid>();
      foreach (ListViewItem listViewItem in this.lvObjects.Items)
        guidList.Add(((IPublishObjectID) listViewItem.Tag).ObjectGuid);
      return guidList.ToArray();
    }
  }

  public bool AutoUpdate => this.cbAutoUpdate.Checked;

  public SelectCompositionType CompositionType
  {
    get
    {
      return this.rbObjectOnly.Checked || !this.rbAllComposition.Checked ? SelectCompositionType.None : SelectCompositionType.RecursiveComposition;
    }
  }

  public void Init(IUserSession session, List<IPublishObjectID> objs)
  {
    IDBConfigurations configurations = session.Configurations;
    ICategoryTypeIconService service1 = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    string str = configurations.ReadString(PortalConsts.PortalClientModuleName, "PUBLISH_SETTINGS", "OwnComplete_ParentSites", string.Empty, DBConfigMode.UserOnly);
    ISitesCacheService customService = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    List<SiteInfo> sites = customService.Sites;
    char code = customService.Info.Code;
    IPortalMetadata service2 = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    int num = service1.IndexOf(4, MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeSites));
    for (int index = 0; index < sites.Count; ++index)
    {
      ListViewItem listViewItem = this.lvSites.Items.Add(sites[index].Caption);
      listViewItem.Tag = (object) sites[index];
      listViewItem.Checked = str.IndexOf(sites[index].Code) >= 0;
      listViewItem.ImageIndex = num;
      if (str == string.Empty && (int) sites[index].Code == (int) code)
        listViewItem.Checked = true;
    }
    for (int index = 0; index < objs.Count; ++index)
      this.lvObjects.Items.Add(new ListViewItem(new string[3]
      {
        objs[index].ObjectID.ToString(),
        objs[index].Caption,
        service2.GetPublishObjectTypeName(objs[index].TypeID)
      })
      {
        Tag = (object) objs[index],
        ImageIndex = service1.IndexOf(SiteClientConsts.CategoryPublishType, objs[index].TypeID)
      });
    this.cbAutoUpdate.Checked = configurations.ReadBool(PortalConsts.PortalClientModuleName, "PUBLISH_SETTINGS", "OwnComplete_AutoUpdate", false, DBConfigMode.UserOnly);
    this.rbAllComposition.Checked = configurations.ReadBool(PortalConsts.PortalClientModuleName, "PUBLISH_SETTINGS", "OwnComplete_WithComposition", true, DBConfigMode.UserOnly);
    this.RefreshControls();
  }

  private bool _anyChecked
  {
    get
    {
      bool anyChecked = false;
      foreach (ListViewItem listViewItem in this.lvSites.Items)
      {
        if (listViewItem.Checked)
        {
          anyChecked = true;
          break;
        }
      }
      return anyChecked;
    }
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    this._parentSites = string.Empty;
    foreach (ListViewItem listViewItem in this.lvSites.Items)
    {
      if (listViewItem.Checked)
        this._parentSites += ((SiteInfo) listViewItem.Tag).Code.ToString();
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.Configurations.WriteString(PortalConsts.PortalClientModuleName, "PUBLISH_SETTINGS", "OwnComplete_ParentSites", this._parentSites, sessionKeeper.Session.UserID);
      sessionKeeper.Session.Configurations.WriteBool(PortalConsts.PortalClientModuleName, "PUBLISH_SETTINGS", "OwnComplete_AutoUpdate", this.cbAutoUpdate.Checked, sessionKeeper.Session.UserID);
      sessionKeeper.Session.Configurations.WriteBool(PortalConsts.PortalClientModuleName, "PUBLISH_SETTINGS", "OwnComplete_WithComposition", this.rbAllComposition.Checked, sessionKeeper.Session.UserID);
    }
    this.Close();
  }

  private void RefreshControls() => this.bOK.Enabled = this._anyChecked;

  private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
  {
    this.RefreshControls();
  }

  private void OwnCompleteForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OwnCompleteForm));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.groupBox1 = new GroupBox();
    this.lvSites = new ListView();
    this.columnHeader2 = new ColumnHeader();
    this.rbAllComposition = new RadioButton();
    this.rbObjectOnly = new RadioButton();
    this.groupBox2 = new GroupBox();
    this.lvObjects = new ListView();
    this.columnHeader3 = new ColumnHeader();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader4 = new ColumnHeader();
    this.panel5 = new Panel();
    this.cbAutoUpdate = new CheckBox();
    this.splitContainer1 = new SplitContainer();
    this.panel2 = new Panel();
    this.panel1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel5.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.groupBox1.Controls.Add((Control) this.lvSites);
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    this.lvSites.CheckBoxes = true;
    this.lvSites.Columns.AddRange(new ColumnHeader[1]
    {
      this.columnHeader2
    });
    componentResourceManager.ApplyResources((object) this.lvSites, "lvSites");
    this.lvSites.GridLines = true;
    this.lvSites.Name = "lvSites";
    this.lvSites.UseCompatibleStateImageBehavior = false;
    this.lvSites.View = View.Details;
    componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
    componentResourceManager.ApplyResources((object) this.rbAllComposition, "rbAllComposition");
    this.rbAllComposition.Checked = true;
    this.rbAllComposition.Name = "rbAllComposition";
    this.rbAllComposition.TabStop = true;
    this.rbAllComposition.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.rbObjectOnly, "rbObjectOnly");
    this.rbObjectOnly.Name = "rbObjectOnly";
    this.rbObjectOnly.UseVisualStyleBackColor = true;
    this.groupBox2.Controls.Add((Control) this.lvObjects);
    componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.TabStop = false;
    this.lvObjects.Columns.AddRange(new ColumnHeader[3]
    {
      this.columnHeader3,
      this.columnHeader1,
      this.columnHeader4
    });
    componentResourceManager.ApplyResources((object) this.lvObjects, "lvObjects");
    this.lvObjects.FullRowSelect = true;
    this.lvObjects.GridLines = true;
    this.lvObjects.Name = "lvObjects";
    this.lvObjects.UseCompatibleStateImageBehavior = false;
    this.lvObjects.View = View.Details;
    componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
    componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
    componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
    this.panel5.Controls.Add((Control) this.cbAutoUpdate);
    this.panel5.Controls.Add((Control) this.rbAllComposition);
    this.panel5.Controls.Add((Control) this.rbObjectOnly);
    componentResourceManager.ApplyResources((object) this.panel5, "panel5");
    this.panel5.Name = "panel5";
    componentResourceManager.ApplyResources((object) this.cbAutoUpdate, "cbAutoUpdate");
    this.cbAutoUpdate.Name = "cbAutoUpdate";
    this.cbAutoUpdate.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel2);
    this.splitContainer1.Panel1.Controls.Add((Control) this.panel5);
    this.splitContainer1.Panel2.Controls.Add((Control) this.groupBox1);
    this.panel2.Controls.Add((Control) this.groupBox2);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (OwnCompleteForm);
    this.FormClosing += new FormClosingEventHandler(this.OwnCompleteForm_FormClosing);
    this.panel1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.panel5.ResumeLayout(false);
    this.panel5.PerformLayout();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
