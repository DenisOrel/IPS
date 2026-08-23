// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.ArchiveSigns
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using ImSSP;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

[ViewDescriptionProvider(typeof (ArchiveSigns.ArchiveSignsViewDescriptionProvider))]
internal class ArchiveSigns : UserControl, IView
{
  private System.ComponentModel.Container components;
  private Panel _bottom;
  private Panel _buttons;
  private Button _bApply;
  private Button _bCancel;
  private Panel panel1;
  private Panel _controlPanel;
  private SignControl _control;
  private GraphsSet _originalSet;
  private bool _modified;
  private int _imageIndex = -1;
  private long _objID;
  private bool _first;

  public ArchiveSigns()
  {
    this.InitializeComponent();
    this._imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgSign2");
    this._control = new SignControl();
    this._controlPanel.SuspendLayout();
    this._controlPanel.Controls.Add((Control) this._control);
    this._control.Dock = DockStyle.Fill;
    this._control.OnModified += new EventHandler(this._control_OnModified);
    this._controlPanel.ResumeLayout(false);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  public bool Modified
  {
    get => this._modified;
    set
    {
      this._modified = value;
      this._buttons.Enabled = value;
    }
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ArchiveSigns));
    this._controlPanel = new Panel();
    this.panel1 = new Panel();
    this._bottom = new Panel();
    this._buttons = new Panel();
    this._bCancel = new Button();
    this._bApply = new Button();
    this._controlPanel.SuspendLayout();
    this._bottom.SuspendLayout();
    this._buttons.SuspendLayout();
    this.SuspendLayout();
    this._controlPanel.Controls.Add((Control) this.panel1);
    componentResourceManager.ApplyResources((object) this._controlPanel, "_controlPanel");
    this._controlPanel.Name = "_controlPanel";
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this._bottom.Controls.Add((Control) this._buttons);
    componentResourceManager.ApplyResources((object) this._bottom, "_bottom");
    this._bottom.Name = "_bottom";
    this._buttons.Controls.Add((Control) this._bCancel);
    this._buttons.Controls.Add((Control) this._bApply);
    componentResourceManager.ApplyResources((object) this._buttons, "_buttons");
    this._buttons.Name = "_buttons";
    componentResourceManager.ApplyResources((object) this._bCancel, "_bCancel");
    this._bCancel.Name = "_bCancel";
    this._bCancel.Click += new EventHandler(this._bCancel_Click);
    componentResourceManager.ApplyResources((object) this._bApply, "_bApply");
    this._bApply.Name = "_bApply";
    this._bApply.Click += new EventHandler(this._bApply_Click);
    this.Controls.Add((Control) this._controlPanel);
    this.Controls.Add((Control) this._bottom);
    this.Name = nameof (ArchiveSigns);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Tag = (object) " ";
    this._controlPanel.ResumeLayout(false);
    this._bottom.ResumeLayout(false);
    this._buttons.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public int ImageIndex => this._imageIndex;

  public int OrderID => 70;

  public string Caption => LocalizationHolder.rm.GetString("Signs_37");

  public void Initialize(ISelectedItems items, IServiceProvider services)
  {
    this._objID = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    this._first = true;
  }

  public void Deactivate(IView nextView)
  {
    if (!this.Modified)
      return;
    if (MessageBox.Show(LocalizationHolder.rm.GetString(sc_18429.ssp_signs_18430()), LocalizationHolder.rm.GetString("Signs_39"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
      this._bApply_Click((object) null, (EventArgs) null);
    else
      this._bCancel_Click((object) null, (EventArgs) null);
  }

  public void Activate(IView previousView)
  {
    if (!this._first)
      return;
    this._first = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._objID);
      IDBAttribute aIDBAttribute = dbObject.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID) ?? dbObject.Attributes.AddAttribute(SignsHolder.SignsSetupAttrTypeID, false);
      MemoryStream memoryStream = new MemoryStream();
      new BlobProcReader(aIDBAttribute, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
      this._originalSet = new GraphsSet();
      if (memoryStream.Length > 0L)
      {
        this._originalSet = GraphsSet.Load((Stream) memoryStream);
        this._control.Set = GraphsSet.Clone(this._originalSet);
      }
      else
        this._control.Set = this._originalSet;
      this.Modified = false;
      this.Enabled = (dbObject as IDBSecurity).CheckAccess(ActionType.SetAccess, false, false);
    }
  }

  private void _control_OnModified(object sender, EventArgs e)
  {
    this.Modified = this._control.Modified;
  }

  private void _bApply_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      IDBAttribute objectAttributeById = session.GetObjectAttributeByID(this._objID, SignsHolder.SignsSetupAttrTypeID);
      bool flag = true;
      DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(SignsHolder.ArchAttrTypeID, RelationalOperators.Equal, (object) this._objID, (object) null, LogicalOperators.NONE, 0, false, AttributeSourceTypes.Auto, ColumnContents.ID)
      }, new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      });
      DataTable dataTable = session.ObjectsSelect(SignsHolder.DocumentObjectTypeID, dbRecordSetParams);
      if (dataTable != null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          long int64 = Convert.ToInt64(row[0]);
          if (!arrayList.Contains((object) int64))
            arrayList.Add((object) int64);
        }
        if (arrayList.Count > 0)
          flag = (session.GetCustomService(typeof (ISignsService)) as ISignsService).CheckSigns(arrayList.ToArray(typeof (long)) as long[], this._objID, this._control.Set, session.SessionGUID, true, true);
      }
      if (!flag)
        return;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this._control.Set.Save((Stream) memoryStream);
        BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "signs.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(objectAttributeById, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
      }
      this.Modified = false;
      SignsCache.ClearCache(session);
    }
  }

  private void _bCancel_Click(object sender, EventArgs e)
  {
    this._first = true;
    this.Activate((IView) null);
  }

  private void _rb_CheckedChanged(object sender, EventArgs e)
  {
    if (!(sender is RadioButton) || !(sender as RadioButton).Checked)
      return;
    this.Modified = true;
  }

  private sealed class ArchiveSignsViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = LocalizationHolder.rm.GetString("Signs_37"),
        ImageIndex = namedImageList.ImageIndex("imgSign2"),
        OrderID = 70
      };
    }
  }
}
