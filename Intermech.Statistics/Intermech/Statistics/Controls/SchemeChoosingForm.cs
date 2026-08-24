// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Controls.SchemeChoosingForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Infralution.Controls.VirtualTree;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Workflow;
using Intermech.Workflow.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Statistics.Controls;

public class SchemeChoosingForm : Form
{
  private ISelectedItemsHost _selhost;
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private SplitContainer splitContainer;
  private Button btnCancel;
  private Button btnOk;
  private TreeViewsBridge treeViewsBridge;
  private SchemesTreeView schemesView;
  private PageViewsManager pageViewsManager1;

  public List<IDBObjectID> Schemes { get; } = new List<IDBObjectID>();

  public SchemeChoosingForm(bool multiChoose)
  {
    this.InitializeComponent();
    Intermech.Workflow.Design.Holder.СanShowAllVersions = true;
    ServiceContainer serviceContainer = new ServiceContainer();
    serviceContainer.AddService(typeof (INotificationService), (object) BaseHolder.NotificationService);
    serviceContainer.AddService(typeof (ValidSchemesOnlyFlag), (object) new ValidSchemesOnlyFlag());
    serviceContainer.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.InDialog));
    serviceContainer.AddService(typeof (VersionsRule), (object) Intermech.Workflow.Design.Holder.AllVersionsRule);
    this.schemesView.Services = (IServiceProvider) serviceContainer;
    this.pageViewsManager1.Services = (IServiceProvider) serviceContainer;
    this.schemesView.SetColumns(Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
    this.schemesView.DisableIMContextMenu = true;
    IDescriptor rootDescriptor = (IDescriptor) new TopObjectsDescriptor(Intermech.Workflow.Design.Holder.CategorySchemesID, 0, "Шаблоны процессов", wfConsts.SchemeCategoriesID);
    if (!wfFunx.RestoreTreePath((NavigatorTreeView) this.schemesView))
      this.schemesView.Build(rootDescriptor);
    if (this.pageViewsManager1.ActiveViewPage != null)
    {
      IView view = this.pageViewsManager1.ActiveViewPage.View;
      foreach (object control in (ArrangedElementCollection) this.pageViewsManager1.ActiveViewPage.Control.Controls)
      {
        if (control is iGrid iGrid)
        {
          if (!multiChoose)
          {
            iGrid.SelectionMode = iGSelectionMode.One;
            break;
          }
          break;
        }
      }
      this._selhost = view as ISelectedItemsHost;
      if (this._selhost != null)
        this._selhost.SelectedItemsChanged += new EventHandler(this.SchemesSelectedItemsChanged);
    }
    this.SchemesSelectedItemsChanged((object) null, (EventArgs) null);
  }

  private void SchemesSelectedItemsChanged(object sender, EventArgs e)
  {
    if (this._selhost == null)
      return;
    this.Schemes.Clear();
    ISelectedItems selectedItems = this._selhost.SelectedItems;
    if (selectedItems.Count <= 0)
      return;
    for (int index = 0; index < selectedItems.Count; ++index)
    {
      if (selectedItems.GetItemData(index, typeof (IDBObjectID)) is IDBObjectID itemData)
        this.Schemes.Add(itemData);
    }
  }

  private void btnOk_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this.DialogResult = DialogResult.Cancel;
    this.Schemes.Clear();
  }

  private void SchemeChoosingForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    Intermech.Workflow.Design.Holder.СanShowAllVersions = false;
    FormStorage.SaveLayout((Control) this, (IDictionary) new Dictionary<string, string>()
    {
      {
        "schemesView.Width",
        this.splitContainer.SplitterDistance.ToString()
      }
    });
  }

  private void SchemeChoosingForm_Load(object sender, EventArgs e)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    dictionary.Add("schemesView.Width", this.splitContainer.SplitterDistance.ToString());
    FormStorage.LoadLayout((Control) this, (IDictionary) dictionary);
    try
    {
      this.splitContainer.SplitterDistance = Convert.ToInt32(dictionary["schemesView.Width"]);
    }
    catch
    {
    }
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
    this.panel1 = new Panel();
    this.btnCancel = new Button();
    this.btnOk = new Button();
    this.panel2 = new Panel();
    this.splitContainer = new SplitContainer();
    this.schemesView = new SchemesTreeView();
    this.pageViewsManager1 = new PageViewsManager();
    this.treeViewsBridge = new TreeViewsBridge(this.components);
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.splitContainer.BeginInit();
    this.splitContainer.Panel1.SuspendLayout();
    this.splitContainer.Panel2.SuspendLayout();
    this.splitContainer.SuspendLayout();
    this.schemesView.BeginInit();
    this.SuspendLayout();
    this.panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Controls.Add((Control) this.btnOk);
    this.panel1.Location = new Point(0, 388);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(1059, 54);
    this.panel1.TabIndex = 0;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(935, 14);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(110, 27);
    this.btnCancel.TabIndex = 3;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOk.Location = new Point(819, 14);
    this.btnOk.Name = "btnOk";
    this.btnOk.Size = new Size(110, 27);
    this.btnOk.TabIndex = 2;
    this.btnOk.Text = "OK";
    this.btnOk.UseVisualStyleBackColor = true;
    this.btnOk.Click += new EventHandler(this.btnOk_Click);
    this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel2.AutoScroll = true;
    this.panel2.AutoSize = true;
    this.panel2.Controls.Add((Control) this.splitContainer);
    this.panel2.Location = new Point(0, 3);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(1059, 379);
    this.panel2.TabIndex = 1;
    this.splitContainer.Dock = DockStyle.Fill;
    this.splitContainer.Location = new Point(0, 0);
    this.splitContainer.Name = "splitContainer";
    this.splitContainer.Panel1.AutoScroll = true;
    this.splitContainer.Panel1.Controls.Add((Control) this.schemesView);
    this.splitContainer.Panel2.AutoScroll = true;
    this.splitContainer.Panel2.Controls.Add((Control) this.pageViewsManager1);
    this.splitContainer.Size = new Size(1059, 379);
    this.splitContainer.SplitterDistance = 251;
    this.splitContainer.TabIndex = 0;
    this.schemesView.AllowDrop = true;
    this.schemesView.AllowMultiSelect = false;
    this.schemesView.AllowUserPinnedColumns = false;
    this.schemesView.DisableCheckedOutColumn = true;
    this.schemesView.DisableIMContextMenu = true;
    this.schemesView.Dock = DockStyle.Fill;
    this.schemesView.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this.schemesView.ImageList = (ImageList) null;
    this.schemesView.LineStyle = LineStyle.Dot;
    this.schemesView.Location = new Point(0, 0);
    this.schemesView.Name = "schemesView";
    this.schemesView.RowEvenStyle.WordWrap = false;
    this.schemesView.RowOddStyle.WordWrap = false;
    this.schemesView.RowSelectedStyle.WordWrap = false;
    this.schemesView.RowStyle.BorderColor = SystemColors.Control;
    this.schemesView.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this.schemesView.RowStyle.BorderWidth = 1;
    this.schemesView.RowStyle.WordWrap = false;
    this.schemesView.SelectBeforeEdit = true;
    this.schemesView.ShowRootRow = false;
    this.schemesView.Size = new Size(251, 379);
    this.schemesView.SuppressErrorMessages = true;
    this.schemesView.TabIndex = 0;
    this.pageViewsManager1.ActiveViewPage = (IViewPage) null;
    this.pageViewsManager1.CausesValidation = false;
    this.pageViewsManager1.Dock = DockStyle.Fill;
    this.pageViewsManager1.Font = new Font("Tahoma", 8.25f);
    this.pageViewsManager1.Location = new Point(0, 0);
    this.pageViewsManager1.Name = "pageViewsManager1";
    this.pageViewsManager1.Padding = new Padding(10, 0, 0, 0);
    this.pageViewsManager1.Size = new Size(804, 379);
    this.pageViewsManager1.TabIndex = 0;
    this.treeViewsBridge.NavTreeView = (NavigatorTreeView) this.schemesView;
    this.treeViewsBridge.UseDelay = false;
    this.treeViewsBridge.ViewsManager = (IViewsManager) this.pageViewsManager1;
    this.AcceptButton = (IButtonControl) this.btnOk;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(1058, 441);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (SchemeChoosingForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выбор шаблона";
    this.FormClosed += new FormClosedEventHandler(this.SchemeChoosingForm_FormClosed);
    this.Load += new EventHandler(this.SchemeChoosingForm_Load);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.splitContainer.Panel1.ResumeLayout(false);
    this.splitContainer.Panel2.ResumeLayout(false);
    this.splitContainer.EndInit();
    this.splitContainer.ResumeLayout(false);
    this.schemesView.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
