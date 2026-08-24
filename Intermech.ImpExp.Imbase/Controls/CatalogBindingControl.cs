// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.CatalogBindingControl
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal class CatalogBindingControl : StepControl
{
  private TreeNode _nodeCatalogs;
  private TreeNode _nodeCtlRefs;
  private TreeNode _nodeTechRefs;
  private Dictionary<string, SaveSettingsAttribute[]> _saveSettings;
  private Image _image;
  private IContainer components;
  private SplitContainer splitContainer1;
  private GroupBox groupBox1;
  private TreeView treeView1;
  private PropertyGrid propertyGrid1;
  private Panel panel1;
  private GroupBox groupBox2;
  private RadioButton rbAdd;
  private RadioButton rbReplace;
  private ImageList imageList1;

  public CatalogBindingControl()
  {
    this.InitializeComponent();
    this._nodeCatalogs = this.treeView1.Nodes.Add("Каталоги");
    this._nodeCatalogs.StateImageIndex = 1;
    this._nodeCtlRefs = this.treeView1.Nodes.Add("Справочники");
    this._nodeCtlRefs.StateImageIndex = 1;
    this._nodeTechRefs = this.treeView1.Nodes.Add("Технологические справочники");
    this._nodeTechRefs.StateImageIndex = 1;
    this._saveSettings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings("IMBASECATALOG");
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseCatalogBindingType);
    try
    {
      if (cache.GetNewKey(ImportingCategory.ImbaseCatalogBindingType, (object) "Replace") == 1L)
        this.rbReplace.Checked = true;
      else
        this.rbAdd.Checked = true;
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseCatalogBindingType);
    }
  }

  public override void Cancel()
  {
    base.Cancel();
    this.LeaveControl();
  }

  public override SaveSettingsResult SaveSettings()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(ImportingCategory.ImbaseCatalogBinding);
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseCatalogBinding);
    try
    {
      this.SaveSaveSettings(this._nodeCatalogs, cache);
      this.SaveSaveSettings(this._nodeCtlRefs, cache);
      this.SaveSaveSettings(this._nodeTechRefs, cache);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseCatalogBinding);
    }
    return SaveSettingsResult.ssrOk;
  }

  protected override string getCaption()
  {
    return "Привязка каталогов Imbase к уже существующим в новой системе";
  }

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgImbaseCatalogsBinding")];
    return this._image;
  }

  public override bool LeaveControl()
  {
    if (this._saveSettings == null)
      this._saveSettings = new Dictionary<string, SaveSettingsAttribute[]>();
    else
      this._saveSettings.Clear();
    this.SaveSaveSettings(this._nodeCatalogs);
    this.SaveSaveSettings(this._nodeCtlRefs);
    this.SaveSaveSettings(this._nodeTechRefs);
    (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).SetSettings("IMBASECATALOG", this._saveSettings);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(ImportingCategory.ImbaseCatalogBindingType);
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseCatalogBindingType);
    try
    {
      cache.AddValue(ImportingCategory.ImbaseCatalogBindingType, (object) "Replace", this.rbReplace.Checked ? 1L : 0L);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseCatalogBindingType);
    }
    return true;
  }

  private void SaveSaveSettings(TreeNode parentNode, IImportingData cacheData)
  {
    if (parentNode.Nodes == null || parentNode.Nodes.Count == 0)
      return;
    for (int index = 0; index < parentNode.Nodes.Count; ++index)
    {
      if (parentNode.Nodes[index].Tag is CatalogBinding tag)
      {
        string caption = tag.BindingCatalog == null || !(tag.BindingCatalog.CatalogID != Guid.Empty) ? string.Empty : Convert.ToString((object) tag.BindingCatalog.CatalogID);
        cacheData.AddValue((object) tag.TableName, tag.Importing ? 1L : 0L, caption);
      }
    }
  }

  private void SaveSaveSettings(TreeNode parentNode)
  {
    if (parentNode.Nodes == null || parentNode.Nodes.Count == 0)
      return;
    for (int index = 0; index < parentNode.Nodes.Count; ++index)
    {
      if (parentNode.Nodes[index].Tag is CatalogBinding tag && tag.BindingCatalog != null && tag.BindingCatalog.CatalogID != Guid.Empty)
        this._saveSettings.Add(tag.TableName, new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("GUID", Convert.ToString((object) tag.BindingCatalog.CatalogID))
        });
    }
  }

  private void AddNode(TreeNode parentNode, TreeNode node) => parentNode.Nodes.Add(node);

  private void CatalogBindingChange(object sender, CatalogBindingEventArgs args)
  {
    for (int index1 = 0; index1 < this.treeView1.Nodes.Count; ++index1)
    {
      TreeNodeCollection nodes = this.treeView1.Nodes[index1].Nodes;
      if (nodes != null && nodes.Count > 0)
      {
        for (int index2 = 0; index2 < nodes.Count; ++index2)
        {
          CatalogBinding tag = nodes[index1].Tag as CatalogBinding;
          if (tag.TableName.Equals(args.TableName))
          {
            nodes[index1].StateImageIndex = tag.BindingCatalog == null ? 0 : 2;
            return;
          }
        }
      }
    }
  }

  internal void AddCatalog(IImTablesItem item, Dictionary<Guid, CatalogPres> catalogsPres)
  {
    CatalogBinding catalogBinding = new CatalogBinding(item.Description, item.TableName, item.Created, item.User, item.TableType, true);
    Guid key = Guid.Empty;
    if (this._saveSettings != null && this._saveSettings.ContainsKey(item.TableName))
    {
      foreach (SaveSettingsAttribute settingsAttribute in this._saveSettings[item.TableName])
      {
        if (settingsAttribute.AttributeName == "GUID")
        {
          key = new Guid(settingsAttribute.AttributeValue);
          break;
        }
      }
    }
    CatalogImbaseAttProxy catalogImbaseAttProxy = (CatalogImbaseAttProxy) null;
    if (key != Guid.Empty)
    {
      CatalogPres catalogPres = (CatalogPres) null;
      if (catalogsPres.TryGetValue(key, out catalogPres))
        catalogImbaseAttProxy = new CatalogImbaseAttProxy(catalogPres.ID, catalogPres.Name);
    }
    if (catalogImbaseAttProxy == null)
    {
      foreach (CatalogPres catalogPres in catalogsPres.Values)
      {
        if (catalogPres.Name.ToUpper() == item.Description.ToUpper() && (catalogPres.ID.ToString().Substring(0, 3).ToUpper() == "CAD" || catalogPres.Type == item.TableType))
        {
          catalogImbaseAttProxy = new CatalogImbaseAttProxy(catalogPres.ID, catalogPres.Name);
          break;
        }
      }
    }
    catalogBinding.BindingCatalog = catalogImbaseAttProxy;
    catalogBinding.BindingChanged += new Intermech.ImpExp.Imbase.Controls.CatalogBindingChange(this.CatalogBindingChange);
    TreeNode treeNode = new TreeNode(catalogBinding.Name);
    treeNode.Tag = (object) catalogBinding;
    treeNode.StateImageIndex = catalogImbaseAttProxy == null ? 0 : 2;
    switch (item.TableType)
    {
      case ImTablesType.IMTT_CATALOG:
        this.Invoke((Delegate) new CatalogBindingControl.AddNodeDelegate(this.AddNode), (object) this._nodeCatalogs, (object) treeNode);
        break;
      case ImTablesType.IMTT_CTLREF:
        this.Invoke((Delegate) new CatalogBindingControl.AddNodeDelegate(this.AddNode), (object) this._nodeCtlRefs, (object) treeNode);
        break;
      case ImTablesType.IMTT_TECHREF:
        this.Invoke((Delegate) new CatalogBindingControl.AddNodeDelegate(this.AddNode), (object) this._nodeTechRefs, (object) treeNode);
        break;
    }
  }

  private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
  {
    if (e.Node.Tag != null)
      this.propertyGrid1.SelectedObject = (object) (CatalogBinding) e.Node.Tag;
    else
      this.propertyGrid1.SelectedObject = (object) null;
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    this.treeView1.ExpandAll();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CatalogBindingControl));
    this.splitContainer1 = new SplitContainer();
    this.groupBox1 = new GroupBox();
    this.treeView1 = new TreeView();
    this.imageList1 = new ImageList(this.components);
    this.propertyGrid1 = new PropertyGrid();
    this.panel1 = new Panel();
    this.groupBox2 = new GroupBox();
    this.rbAdd = new RadioButton();
    this.rbReplace = new RadioButton();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.groupBox1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.propertyGrid1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.panel1);
    this.splitContainer1.Size = new Size(563, 366);
    this.splitContainer1.SplitterDistance = 229;
    this.splitContainer1.TabIndex = 0;
    this.groupBox1.Controls.Add((Control) this.treeView1);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(229, 366);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Каталоги и справочники Imbase";
    this.treeView1.Dock = DockStyle.Fill;
    this.treeView1.Location = new Point(3, 16 /*0x10*/);
    this.treeView1.Name = "treeView1";
    this.treeView1.Size = new Size(223, 347);
    this.treeView1.StateImageList = this.imageList1;
    this.treeView1.TabIndex = 0;
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "blank.gif");
    this.imageList1.Images.SetKeyName(1, "Folder-Closed.png");
    this.imageList1.Images.SetKeyName(2, "document_exchange.png");
    this.propertyGrid1.Dock = DockStyle.Fill;
    this.propertyGrid1.Location = new Point(0, 0);
    this.propertyGrid1.Name = "propertyGrid1";
    this.propertyGrid1.Size = new Size(330, 295);
    this.propertyGrid1.TabIndex = 1;
    this.panel1.Controls.Add((Control) this.groupBox2);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 295);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(330, 71);
    this.panel1.TabIndex = 0;
    this.groupBox2.Controls.Add((Control) this.rbAdd);
    this.groupBox2.Controls.Add((Control) this.rbReplace);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(330, 71);
    this.groupBox2.TabIndex = 0;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Способ импорта каталогов";
    this.rbAdd.AutoSize = true;
    this.rbAdd.Checked = true;
    this.rbAdd.Location = new Point(16 /*0x10*/, 42);
    this.rbAdd.Name = "rbAdd";
    this.rbAdd.Size = new Size(273, 17);
    this.rbAdd.TabIndex = 1;
    this.rbAdd.TabStop = true;
    this.rbAdd.Text = "Добавлять новую информацию к существующей";
    this.rbAdd.UseVisualStyleBackColor = true;
    this.rbReplace.AutoSize = true;
    this.rbReplace.Location = new Point(16 /*0x10*/, 19);
    this.rbReplace.Name = "rbReplace";
    this.rbReplace.Size = new Size(297, 17);
    this.rbReplace.TabIndex = 0;
    this.rbReplace.Text = "Заменять существующие каталоги импортируемыми";
    this.rbReplace.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitContainer1);
    this.Name = nameof (CatalogBindingControl);
    this.Size = new Size(563, 366);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.ResumeLayout(false);
  }

  private delegate void AddNodeDelegate(TreeNode parentNode, TreeNode node);
}
