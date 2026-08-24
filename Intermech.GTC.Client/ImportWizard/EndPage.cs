// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ImportWizard.EndPage
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.GTC.Client.BrowseFileFolderDialog;
using Intermech.GTC.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.CustomNode;
using Intermech.Navigator.Interfaces;
using Intermech.UI.Winforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.ImportWizard;

public class EndPage : UserControl, IWizardPage
{
  private IImportConfig _importSettings;
  private IContainer components;
  private ButtonedEdit edCatalogFolder;
  private ButtonedEdit edCatalogID;
  private CheckBox chbOnlyPlibAttributes;

  public EndPage() => this.InitializeComponent();

  public EndPage(IImportConfig importSettings)
    : this()
  {
    this._importSettings = importSettings;
  }

  public void Activate(IWizardPage prevPage, bool rollback)
  {
    this.EdCanPageComplete((object) this, new EventArgs());
  }

  public void Deactivate(IWizardPage nextPage, bool rollback)
  {
  }

  public bool ReallyComplete => true;

  public void DoMagic()
  {
    this._importSettings.Path = this.edCatalogFolder.Value;
    this._importSettings.CatalogId = (long) this.edCatalogID.Tag;
    this._importSettings.OnlyPlibAttributes = this.chbOnlyPlibAttributes.Checked;
  }

  Control IWizardPage.Control => (Control) this;

  public IWizard Wizard { get; set; }

  string IWizardPage.Name => Intermech.GTC.Client.Const.EndPageName;

  public string Caption => ServiceHolder.Rm.GetString("GTC_9");

  public string Description => ServiceHolder.Rm.GetString("GTC_10");

  public Image Image => (Image) null;

  public event EventHandler<PageCompleteEventArgs> PageComplete;

  private void edCatalogFolder_ButtonClick(object sender, EventArgs e)
  {
    string path = BrowseForServerFileFolder.SelectFileFolder("*.zip");
    if (!(path != string.Empty))
      return;
    this.edCatalogFolder.Value = Path.GetFullPath(path);
  }

  private void EdCanPageComplete(object sender, EventArgs e)
  {
    if (this.PageComplete == null)
      return;
    if (this.edCatalogID.Value.Length > 0 && this.edCatalogFolder.Value.Length > 0)
      this.PageComplete((object) this, new PageCompleteEventArgs(true));
    else
      this.PageComplete((object) this, new PageCompleteEventArgs(false));
  }

  private void _edCatalogFolder_KeyDown(object sender, KeyEventArgs e) => e.SuppressKeyPress = true;

  private void edCatalogName_KeyDown(object sender, KeyEventArgs e) => e.SuppressKeyPress = true;

  private void edCatalogName_ButtonClick(object sender, EventArgs e)
  {
    IDescriptor rootDescriptor = (IDescriptor) new ObjectsSelectionDescriptor(Intermech.GTC.Client.Const.ImbaseCatalogObjectTypeId, ServiceHolder.Rm.GetString("GTC_12"), (IReadOnlyCollection<ConditionStructure>) new ConditionStructure[1]
    {
      new ConditionStructure(Intermech.GTC.Client.Const.CatalogTypeAttributeTypeId, RelationalOperators.Equal, (object) "Каталоги GTC", LogicalOperators.NONE, 0, false)
    });
    object[] source = SelectionWindow.Select(ServiceHolder.Rm.GetString("GTC_33"), ServiceHolder.Rm.GetString("GTC_33"), rootDescriptor, typeof (IDBObjectID), SelectionOptions.SelectObjects | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (!(source is IDBObjectID[]) || (source as IDBObjectID[]).Length == 0)
      return;
    IDBObjectID dbObjectId = ((IEnumerable<IDBObjectID>) (source as IDBObjectID[])).First<IDBObjectID>();
    if (dbObjectId == null)
      return;
    this.edCatalogID.Value = dbObjectId.Caption;
    this.edCatalogID.Tag = (object) dbObjectId.Value;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.edCatalogFolder = new ButtonedEdit();
    this.edCatalogID = new ButtonedEdit();
    this.chbOnlyPlibAttributes = new CheckBox();
    this.SuspendLayout();
    this.edCatalogFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edCatalogFolder.ButtonImage = (Image) null;
    this.edCatalogFolder.ButtonText = "...";
    this.edCatalogFolder.Caption = "Расположение GTC каталога на сервере";
    this.edCatalogFolder.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edCatalogFolder.Image = (Image) null;
    this.edCatalogFolder.Location = new Point(72, 42);
    this.edCatalogFolder.MinimumSize = new Size(40, 20);
    this.edCatalogFolder.Name = "edCatalogFolder";
    this.edCatalogFolder.Size = new Size(615, 38);
    this.edCatalogFolder.TabIndex = 0;
    this.edCatalogFolder.ButtonClick += new EventHandler(this.edCatalogFolder_ButtonClick);
    this.edCatalogFolder.EditTextChanged += new EventHandler(this.EdCanPageComplete);
    this.edCatalogFolder.KeyDown += new KeyEventHandler(this._edCatalogFolder_KeyDown);
    this.edCatalogID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edCatalogID.ButtonImage = (Image) null;
    this.edCatalogID.ButtonText = "...";
    this.edCatalogID.Caption = "Каталог для импорта";
    this.edCatalogID.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edCatalogID.Image = (Image) null;
    this.edCatalogID.Location = new Point(72, 124);
    this.edCatalogID.MinimumSize = new Size(40, 20);
    this.edCatalogID.Name = "edCatalogID";
    this.edCatalogID.Size = new Size(615, 38);
    this.edCatalogID.TabIndex = 1;
    this.edCatalogID.ButtonClick += new EventHandler(this.edCatalogName_ButtonClick);
    this.edCatalogID.EditTextChanged += new EventHandler(this.EdCanPageComplete);
    this.edCatalogID.KeyDown += new KeyEventHandler(this.edCatalogName_KeyDown);
    this.chbOnlyPlibAttributes.AutoSize = true;
    this.chbOnlyPlibAttributes.Checked = true;
    this.chbOnlyPlibAttributes.CheckState = CheckState.Checked;
    this.chbOnlyPlibAttributes.Location = new Point(72, 204);
    this.chbOnlyPlibAttributes.Name = "chbOnlyPlibAttributes";
    this.chbOnlyPlibAttributes.Size = new Size(220, 17);
    this.chbOnlyPlibAttributes.TabIndex = 2;
    this.chbOnlyPlibAttributes.Text = "Импортировать только PLIB атрибуты";
    this.chbOnlyPlibAttributes.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.chbOnlyPlibAttributes);
    this.Controls.Add((Control) this.edCatalogID);
    this.Controls.Add((Control) this.edCatalogFolder);
    this.Name = nameof (EndPage);
    this.Size = new Size(770, 434);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
