// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.BrowseFileFolderDialog.BrowseFileFolderDialog
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Infralution.Controls.VirtualTree;
using Intermech.GTC.Interfaces;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.BrowseFileFolderDialog;

public class BrowseFileFolderDialog : Form
{
  private IBrowseFileFolder _browser;
  private string _filter;
  private IContainer components;
  private Button btnOK;
  private Button btnCancel;
  private Intermech.Search.UI.VirtualTree.VirtualTree treeView;
  private Column column1;

  public BrowseFileFolderDialog(IBrowseFileFolder browser, string filter = "*")
  {
    this.InitializeComponent();
    this._browser = browser;
    this._filter = filter;
    this.treeView.DataSource = (object) this._browser.DataSource;
  }

  public string Path
  {
    get
    {
      return this.treeView.SelectedItem == null ? string.Empty : ((IFileFolderInfo) this.treeView.SelectedItem).FullPath;
    }
  }

  private void extendedVirtualTree_GetChildren(object sender, GetChildrenEventArgs e)
  {
    if (!(e.Row.Item is IFileFolderInfo parentItem))
      return;
    ArrayList arrayList = new ArrayList();
    foreach (IFileFolderInfo childrenItem in this._browser.GetChildrenItems(parentItem, this._filter))
      arrayList.Add((object) childrenItem);
    e.Children = (IList) arrayList;
  }

  private void extendedVirtualTree_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (!(e.Row.Item is IFileFolderInfo fileFolderInfo))
      return;
    e.CellData.Value = (object) fileFolderInfo.Name;
  }

  private void extendedVirtualTree_GetRowData(object sender, GetRowDataEventArgs e)
  {
    if (!(e.Row.Item is IFileFolderInfo fileFolderInfo) || fileFolderInfo.Image == null)
      return;
    e.RowData.Icon = fileFolderInfo.Image;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.treeView = new Intermech.Search.UI.VirtualTree.VirtualTree();
    this.column1 = new Column();
    this.treeView.BeginInit();
    this.SuspendLayout();
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(149, 432);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(121, 27);
    this.btnOK.TabIndex = 0;
    this.btnOK.Text = "OK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(276, 432);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(121, 27);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.treeView.AllowDrop = true;
    this.treeView.AllowMultiSelect = false;
    this.treeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.treeView.AutoFitColumns = true;
    this.treeView.Columns.Add(this.column1);
    this.treeView.IconWidth = 0;
    this.treeView.ImageList = (ImageList) null;
    this.treeView.LineStyle = LineStyle.Dot;
    this.treeView.Location = new Point(12, 12);
    this.treeView.Name = "treeView";
    this.treeView.SelectionMode = Infralution.Controls.VirtualTree.SelectionMode.MainCellText;
    this.treeView.ShowColumnHeaders = false;
    this.treeView.ShowRootRow = false;
    this.treeView.Size = new Size(385, 414);
    this.treeView.TabIndex = 3;
    this.treeView.GetCellData += new GetCellDataHandler(this.extendedVirtualTree_GetCellData);
    this.treeView.GetChildren += new GetChildrenHandler(this.extendedVirtualTree_GetChildren);
    this.treeView.GetRowData += new GetRowDataHandler(this.extendedVirtualTree_GetRowData);
    this.column1.Caption = (string) null;
    this.column1.Name = "column1";
    this.column1.Width = 381;
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(409, 471);
    this.Controls.Add((Control) this.treeView);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (BrowseFileFolderDialog);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.SizeGripStyle = SizeGripStyle.Show;
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Выбор источника";
    this.treeView.EndInit();
    this.ResumeLayout(false);
  }
}
