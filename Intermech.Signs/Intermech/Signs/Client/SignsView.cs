// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsView
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraGrid;
using DevExpress.IM.XtraGrid.Columns;
using DevExpress.IM.XtraGrid.Views.Base;
using DevExpress.IM.XtraGrid.Views.Grid;
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

[ViewDescriptionProvider(typeof (SignsView.SignsViewDescriptionProvider))]
internal class SignsView : UserControl, IView
{
  private GridControl _grid;
  private GridView _view;
  private Panel bottom;
  private Button btnSign;
  private Button btnSignAs;
  private Button btnCryptoSign;
  private ImageList ilSignStatus;
  private IContainer components;
  private int _imageIndex = -1;
  private INotificationService _notificationService;
  private long _objectID;
  private bool isFirstLoad;
  private bool _activeView;
  private int statusColumnIndex = -1;
  private int certColumnIndex = -1;
  private int staffPositionColumnIndex = -1;
  private DateTime _modifyDate;
  private Label lbWarning;
  private Button btnDelete;
  private Button btnVerify;
  private IDBTypedObjectID typedObjectID;
  private bool staffPositionColumnEnabled;

  public int ImageIndex => this._imageIndex;

  public int OrderID => 51;

  public string Caption => LocalizationHolder.rm.GetString("Signs_54");

  public SignsView()
  {
    this.InitializeComponent();
    this._imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("imgSign");
    this._notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    this._notificationService.Subscribe("RelationsCreated", new NotificationEventHandler(this.NotifyEvent));
    this._notificationService.Subscribe("RelationsRemoved", new NotificationEventHandler(this.NotifyEvent));
    this._notificationService.Subscribe("ObjectsCheckedIn", new NotificationEventHandler(this.NotifyEvent));
    this._notificationService.Subscribe("ObjectsChangesCancelled", new NotificationEventHandler(this.NotifyEvent));
    this._notificationService.Subscribe("ObjectsCheckedOut", new NotificationEventHandler(this.NotifyEvent));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      this._notificationService.Unsubscribe("RelationsCreated", new NotificationEventHandler(this.NotifyEvent));
      this._notificationService.Unsubscribe("RelationsRemoved", new NotificationEventHandler(this.NotifyEvent));
      this._notificationService.Unsubscribe("ObjectsCheckedIn", new NotificationEventHandler(this.NotifyEvent));
      this._notificationService.Unsubscribe("ObjectsChangesCancelled", new NotificationEventHandler(this.NotifyEvent));
      this._notificationService.Unsubscribe("ObjectsCheckedOut", new NotificationEventHandler(this.NotifyEvent));
      if (this.components != null)
        this.components.Dispose();
      if (this._view != null)
        this._view.Dispose();
    }
    base.Dispose(disposing);
  }

  private void NotifyEvent(object sender, NotificationEventArgs e)
  {
    if (sender != null && sender.Equals((object) this))
      return;
    DBObjectsEventArgs objectsEventArgs = e as DBObjectsEventArgs;
    DBObjectsCheckOutEventArgs checkOutEventArgs = e as DBObjectsCheckOutEventArgs;
    switch (e.EventName)
    {
      case "RelationsCreated":
      case "RelationsRemoved":
        if (this._activeView)
        {
          this.RefreshData();
          break;
        }
        this.isFirstLoad = true;
        break;
      case "ObjectsCheckedIn":
      case "ObjectsChangesCancelled":
        if (objectsEventArgs == null || !objectsEventArgs.ObjectIDs.Contains(this._objectID))
          break;
        this.isFirstLoad = true;
        this._objectID = Math.Abs(this._objectID);
        if (!this._activeView)
          break;
        this.Activate((IView) null);
        break;
      case "ObjectsCheckedOut":
        if (checkOutEventArgs == null || !checkOutEventArgs.ObjectIDs.Contains(this._objectID))
          break;
        int index = checkOutEventArgs.ObjectIDs.IndexOf(this._objectID);
        this._objectID = checkOutEventArgs.NewObjectIDs[index];
        this.isFirstLoad = true;
        if (!this._activeView)
          break;
        this.Activate((IView) null);
        break;
    }
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SignsView));
    this.bottom = new Panel();
    this.btnVerify = new Button();
    this.btnDelete = new Button();
    this.btnCryptoSign = new Button();
    this.btnSignAs = new Button();
    this.btnSign = new Button();
    this._grid = new GridControl();
    this._view = new GridView();
    this.ilSignStatus = new ImageList(this.components);
    this.lbWarning = new Label();
    this.bottom.SuspendLayout();
    this._grid.BeginInit();
    this._view.BeginInit();
    this.SuspendLayout();
    this.bottom.Controls.Add((Control) this.btnVerify);
    this.bottom.Controls.Add((Control) this.btnDelete);
    this.bottom.Controls.Add((Control) this.btnCryptoSign);
    this.bottom.Controls.Add((Control) this.btnSignAs);
    this.bottom.Controls.Add((Control) this.btnSign);
    componentResourceManager.ApplyResources((object) this.bottom, "bottom");
    this.bottom.Name = "bottom";
    componentResourceManager.ApplyResources((object) this.btnVerify, "btnVerify");
    this.btnVerify.Name = "btnVerify";
    this.btnVerify.Click += new EventHandler(this.btnVerify_Click);
    componentResourceManager.ApplyResources((object) this.btnDelete, "btnDelete");
    this.btnDelete.Name = "btnDelete";
    this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
    componentResourceManager.ApplyResources((object) this.btnCryptoSign, "btnCryptoSign");
    this.btnCryptoSign.Name = "btnCryptoSign";
    this.btnCryptoSign.Click += new EventHandler(this.btnCryptoSign_Click);
    componentResourceManager.ApplyResources((object) this.btnSignAs, "btnSignAs");
    this.btnSignAs.Name = "btnSignAs";
    this.btnSignAs.Click += new EventHandler(this.button2_Click);
    componentResourceManager.ApplyResources((object) this.btnSign, "btnSign");
    this.btnSign.Name = "btnSign";
    this.btnSign.Click += new EventHandler(this.button1_Click);
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    this._grid.EmbeddedNavigator.Name = "";
    this._grid.MainView = (BaseView) this._view;
    this._grid.Name = "_grid";
    this._grid.Styles.AddReplace("HideSelectionRow", (object) new ViewStyleEx("HideSelectionRow", "Grid", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, SystemColors.Control, SystemColors.ControlText, Color.Empty, LinearGradientMode.Horizontal));
    this._grid.Styles.AddReplace("FocusedCell", (object) new ViewStyleEx("FocusedCell", "Grid", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204), StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, SystemColors.Highlight, Color.White, Color.Empty, LinearGradientMode.Horizontal));
    this._grid.Styles.AddReplace("SelectedRow", (object) new ViewStyleEx("SelectedRow", "Grid", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor, SystemColors.Highlight, Color.White, SystemColors.Highlight, LinearGradientMode.Horizontal));
    this._view.FocusRectStyle = DrawFocusRectStyle.RowFocus;
    this._view.GridControl = this._grid;
    componentResourceManager.ApplyResources((object) this._view, "_view");
    this._view.Name = "_view";
    this._view.OptionsBehavior.Editable = false;
    this._view.OptionsMenu.EnableColumnMenu = false;
    this._view.OptionsMenu.EnableFooterMenu = false;
    this._view.OptionsMenu.EnableGroupPanelMenu = false;
    this._view.OptionsSelection.MultiSelect = true;
    this._view.OptionsView.ColumnAutoWidth = false;
    this._view.OptionsView.ShowGroupPanel = false;
    this._view.OptionsView.ShowIndicator = false;
    this._view.CustomDrawCell += new RowCellCustomDrawEventHandler(this._view_CustomDrawCell);
    this._view.Layout += new EventHandler(this._view_Layout);
    this.ilSignStatus.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilSignStatus.ImageStream");
    this.ilSignStatus.TransparentColor = Color.Transparent;
    this.ilSignStatus.Images.SetKeyName(0, "sign.ico");
    this.ilSignStatus.Images.SetKeyName(1, "sign_out.ico");
    this.ilSignStatus.Images.SetKeyName(2, "sign_cr.ico");
    this.ilSignStatus.Images.SetKeyName(3, "sign_cr_out.ico");
    this.ilSignStatus.Images.SetKeyName(4, "sign_false.ico");
    componentResourceManager.ApplyResources((object) this.lbWarning, "lbWarning");
    this.lbWarning.Name = "lbWarning";
    this.Controls.Add((Control) this._grid);
    this.Controls.Add((Control) this.bottom);
    this.Controls.Add((Control) this.lbWarning);
    this.Name = nameof (SignsView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Tag = (object) "   ";
    this.bottom.ResumeLayout(false);
    this._grid.EndInit();
    this._view.EndInit();
    this.ResumeLayout(false);
  }

  public void Initialize(ISelectedItems items, IServiceProvider services)
  {
    this.typedObjectID = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    this._objectID = this.typedObjectID.ObjectID;
    this.isFirstLoad = true;
  }

  public void Deactivate(IView nextView) => this._activeView = false;

  public void Activate(IView previousView)
  {
    if (this.isFirstLoad)
    {
      this.isFirstLoad = false;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IUserSession session = sessionKeeper.Session;
        QuickObjectInfo objectInfo = session.GetObjectInfo(this._objectID);
        IDBAttribute objectAttributeById = session.GetObjectAttributeByID(this._objectID, SignsHolder.ModifyDateAttrTypeID);
        if (objectAttributeById == null)
        {
          this.isFirstLoad = true;
          string attributeTypeName = MetaDataHelper.GetAttributeTypeName(SignsHolder.ModifyDateAttrTypeID);
          throw new Exception(string.Format(LocalizationHolder.rm.GetString(sc_18452.ssp_signs_18453()), (object) attributeTypeName));
        }
        this._modifyDate = objectAttributeById.AsDateTime;
        if (this._objectID < 0L)
        {
          this.btnSign.Enabled = this.btnSignAs.Enabled = this.btnCryptoSign.Enabled = false;
          this.lbWarning.Visible = true;
          this.btnDelete.Enabled = false;
          this.btnDelete.Visible = false;
          this.RefreshData();
          return;
        }
        this.btnSignAs.Enabled = true;
        this.lbWarning.Visible = false;
        this.btnDelete.Enabled = sessionKeeper.Session.IsAdmin;
        this.btnDelete.Visible = sessionKeeper.Session.IsAdmin;
        foreach (string graph in SignsCache.UserSignsCard.GetGraphs(objectInfo.ObjectTypeID))
        {
          if (SignsCache.PossibleGraphs.ContainsKey(graph))
          {
            this.btnSign.Enabled = this.btnCryptoSign.Enabled = true;
            break;
          }
        }
      }
      this.RefreshData();
    }
    this._activeView = true;
  }

  private ConditionStructure[] GetConditions()
  {
    return new ConditionStructure[1]
    {
      new ConditionStructure(0, RelationalOperators.EntersIn, (object) Math.Abs(this._objectID), LogicalOperators.AND, 0, false)
      {
        TypeID = (object) SignsHolder.SignRelationTypeID
      }
    };
  }

  private object[] GetColumns()
  {
    return new object[11]
    {
      (object) -2,
      (object) SignsHolder.ModifyDateAttrTypeID,
      (object) SignsHolder.RankAttrTypeID,
      (object) SignsHolder.GraphAttrTypeID,
      (object) SignsHolder.SignUpAttrTypeID,
      (object) SignsHolder.SignUpIOAttrTypeID,
      (object) SignsHolder.InArchiveAttrTypeID,
      (object) SignsHolder.SignVersionAttrTypeID,
      (object) SignsHolder.HashProtectionAttrTypeID,
      (object) SignsHolder.ResolutionAttrTypeID,
      (object) SignsHolder.DateOfSignatureID
    };
  }

  private DBRecordSetParams GetParams()
  {
    return new DBRecordSetParams((ConditionStructure[]) null)
    {
      Conditions = this.GetConditions(),
      Columns = this.GetColumns()
    };
  }

  private DataTable GetDataTable()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      DataTable dataTable = session.ObjectsSelect(SignsHolder.SignObjectTypeID, this.GetParams());
      IMSObjectType objectType1 = MetaDataHelper.GetObjectType(SignsHolder.SignObjectTypeID);
      IMSObjectType objectType2 = MetaDataHelper.GetObjectType(SignsHolder.CryptoSignObjectTypeID);
      if ((objectType1.Options & ObjectTypeOptions.LocalObjectType) != ObjectTypeOptions.None || (objectType2.Options & ObjectTypeOptions.LocalObjectType) != ObjectTypeOptions.None)
      {
        DataTable table = session.ObjectsSelect(SignsHolder.CryptoSignObjectTypeID, this.GetParams());
        dataTable.Merge(table);
      }
      return dataTable;
    }
  }

  private void RefreshData()
  {
    this.statusColumnIndex = -1;
    this.certColumnIndex = -1;
    this.staffPositionColumnIndex = -1;
    this.staffPositionColumnEnabled = (ServicesManager.GetService(typeof (IClientMetadataCache)) as IClientMetadataCache).GetObjectType(SignsHolder.SignObjectTypeID).Attributes.GetAttributeByGUID(SignsHolder.StaffPositionAttrGuid, false) != null;
    DataTable dataTable1 = this.GetDataTable();
    DataTable dataTable2 = new DataTable(string.Empty);
    List<IMSAttributeType> imsAttributeTypeList = new List<IMSAttributeType>(dataTable1.Columns.Count);
    foreach (DataColumn column in (InternalDataCollectionBase) dataTable1.Columns)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(MetaDataHelper.GetAttributeByTypeNameID(column.ColumnName));
      imsAttributeTypeList.Add(attributeType);
      Type type = column.DataType;
      if (attributeType != null)
      {
        switch (attributeType.FieldType)
        {
          case FieldTypes.ftInteger:
            if (attributeType.AttributeID == SignsHolder.SignVersionAttrTypeID)
            {
              type = typeof (string);
              break;
            }
            break;
          case FieldTypes.ftDateTime:
            type = typeof (DateTime);
            break;
          case FieldTypes.ftBoolean:
            type = typeof (bool);
            break;
        }
      }
      dataTable2.Columns.Add(column.ColumnName, type);
    }
    dataTable2.Columns.Add(LocalizationHolder.rm.GetString("Signs_56"), typeof (string));
    dataTable2.Columns.Add(LocalizationHolder.rm.GetString("Signs_CertOwner"), typeof (string));
    if (this.staffPositionColumnEnabled)
      dataTable2.Columns.Add("Должность по штатному расписанию", typeof (string));
    int num1 = dataTable1.Columns.Count + 1;
    int num2 = num1;
    int length = num1 + 1;
    if (this.staffPositionColumnEnabled)
      ++length;
    if (dataTable1.Rows.Count > 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        {
          object[] itemArray = row.ItemArray;
          object[] objArray1 = new object[length];
          IDBObject dbObject1 = sessionKeeper.Session.GetObject(Convert.ToInt64(itemArray[0]));
          for (int index1 = 0; index1 < itemArray.Length; ++index1)
          {
            IMSAttributeType imsAttributeType = imsAttributeTypeList[index1];
            if (imsAttributeType != null)
            {
              if (imsAttributeType.FieldType == FieldTypes.ftBoolean)
              {
                objArray1[index1] = (object) Convert.ToBoolean(itemArray[index1]);
              }
              else
              {
                int num3 = imsAttributeType.AttributeID;
                if (num3.Equals(SignsHolder.GraphAttrTypeID))
                {
                  string key = itemArray[index1] == null || !itemArray[index1].GetType().Equals(typeof (string)) ? string.Empty : itemArray[index1].ToString();
                  string str;
                  objArray1[index1] = !SignsCache.PossibleGraphs.TryGetValue(key, out str) ? itemArray[index1] : (object) str;
                }
                else
                {
                  num3 = imsAttributeType.AttributeID;
                  if (num3.Equals(SignsHolder.SignVersionAttrTypeID))
                  {
                    string empty = string.Empty;
                    string str1 = string.Empty;
                    string str2;
                    if (dbObject1.ObjectType == SignsHolder.CryptoSignObjectTypeID)
                    {
                      if (HashProcs.IsCompatibleSign(Convert.ToInt32(dbObject1.GetAttributeByID(SignsHolder.SignVersionAttrTypeID).Value)))
                        str1 = $" ({LocalizationHolder.rm.GetString("CompatibleSignLabel")})";
                      str2 = $"({LocalizationHolder.rm.GetString("ComplexSignLabel")})";
                    }
                    else
                      str2 = $"({LocalizationHolder.rm.GetString("SimpleSignLabel")})";
                    object[] objArray2 = objArray1;
                    int index2 = index1;
                    string str3;
                    if (!(str1 != string.Empty))
                    {
                      str3 = Convert.ToString(itemArray[index1]);
                    }
                    else
                    {
                      num3 = HashProcs.SimpleVersion(Convert.ToInt32(itemArray[index1]));
                      str3 = num3.ToString();
                    }
                    string str4 = str2;
                    string str5 = str1;
                    string str6 = $"{str3} {str4}{str5}";
                    objArray2[index2] = (object) str6;
                  }
                  else
                    objArray1[index1] = itemArray[index1];
                }
              }
            }
            else
              objArray1[index1] = itemArray[index1];
          }
          X509Certificate2Collection certificates = (X509Certificate2Collection) null;
          this.statusColumnIndex = num2 - 1;
          objArray1[this.statusColumnIndex] = dbObject1.ObjectType != SignsHolder.CryptoSignObjectTypeID || HashProcs.SimpleVersion(Convert.ToInt32(dbObject1.GetAttributeByID(SignsHolder.SignVersionAttrTypeID).Value)) < 4 ? (object) EnumDescConverter.GetEnumDescription((Enum) SignHelper.TranslateStatus(sessionKeeper.Session, this._objectID, dbObject1.ObjectID, dbObject1.ObjectType, this._modifyDate, Convert.ToDateTime(itemArray[1]), out certificates)) : (object) EnumDescConverter.GetEnumDescription((Enum) SignStatuses.SignNeedToVerify);
          this.certColumnIndex = num2;
          if (certificates != null && certificates.Count > 0)
            objArray1[this.certColumnIndex] = (object) certificates[0].SubjectName.Format(false);
          if (this.staffPositionColumnEnabled)
          {
            this.staffPositionColumnIndex = num2 + 1;
            IDBAttribute attributeById = dbObject1.GetAttributeByID(SignsHolder.StaffPositionAttrID);
            if (attributeById != null)
              objArray1[this.staffPositionColumnIndex] = (object) attributeById.AsString;
          }
          IDBAttribute attributeByGuid1 = dbObject1.GetAttributeByGuid(SignsHolder.SignUpAttrTypeGuid, false);
          string str7 = string.Empty;
          if (attributeByGuid1 != null)
          {
            long asInteger = attributeByGuid1.AsInteger;
            IDBObject dbObject2 = sessionKeeper.Session.GetObject(asInteger, false);
            if (dbObject2 != null)
            {
              IDBAttribute attributeByGuid2 = dbObject2.GetAttributeByGuid(SignsHolder.FIOInSignAttrTypeGuid, false);
              str7 = attributeByGuid2 == null || string.IsNullOrEmpty(attributeByGuid2.AsString) ? dbObject2.GetAttributeByGuid(SignsHolder.VisibleNameAttrTypeGuid).AsString : attributeByGuid2.AsString;
            }
          }
          objArray1[4] = (object) str7;
          dataTable2.Rows.Add(objArray1);
        }
      }
    }
    this._grid.BeginUpdate();
    this._grid.DataSource = (object) null;
    this._grid.DataSource = (object) dataTable2;
    this._view.Columns[num2 - 2].DisplayFormat.FormatString = "MM/dd/yyyy hh:mm:ss";
    foreach (GridColumn column in (CollectionBase) this._view.Columns)
    {
      if (SignsCache.SignsViewColumns.ContainsKey((object) column.FieldName))
        column.Width = Convert.ToInt32(SignsCache.SignsViewColumns[(object) column.FieldName]);
    }
    this._view.Columns[num2 - 2].VisibleIndex = 1;
    this._view.Columns[MetaDataHelper.GetAttributeTypeName(SignsHolder.ModifyDateAttrTypeID)].VisibleIndex = -1;
    this._view.Columns[SignsHolder.HashProtectionAttrTypeName].VisibleIndex = -1;
    this._view.Columns[MetaDataHelper.GetAttributeTypeName(SignsHolder.InArchiveAttrTypeID)].VisibleIndex = -1;
    this._grid.EndUpdate();
  }

  private void _view_Layout(object sender, EventArgs e)
  {
    foreach (GridColumn column in (CollectionBase) this._view.Columns)
    {
      if (column.VisibleIndex >= 0)
        SignsCache.SignsViewColumns[(object) column.FieldName] = (object) column.Width;
    }
  }

  private void button1_Click(object sender, EventArgs e)
  {
    SignsCommands.SignUpCommand(new List<IDBTypedObjectID>()
    {
      this.typedObjectID
    });
  }

  private void button2_Click(object sender, EventArgs e)
  {
    SignsCommands.SignAsCommand(new List<IDBTypedObjectID>()
    {
      this.typedObjectID
    });
  }

  private void btnCryptoSign_Click(object sender, EventArgs e)
  {
    SignsCommands.CryptoSignUp(new List<IDBTypedObjectID>()
    {
      this.typedObjectID
    });
    this.RefreshData();
  }

  private void _view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
  {
    if (!e.Column.FieldName.Equals(LocalizationHolder.rm.GetString("Signs_56")))
      return;
    string s = Convert.ToString(this._view.GetDataRow(e.RowHandle)[LocalizationHolder.rm.GetString("Signs_56")]);
    Image image = !(s == LocalizationHolder.rm.GetString("Signs_62")) ? (!(s == LocalizationHolder.rm.GetString("Signs_59")) ? (!(s == LocalizationHolder.rm.GetString("Signs_58")) ? (!(s == LocalizationHolder.rm.GetString("Signs_60")) ? this.ilSignStatus.Images[1] : this.ilSignStatus.Images[4]) : this.ilSignStatus.Images[3]) : this.ilSignStatus.Images[2]) : this.ilSignStatus.Images[0];
    Rectangle rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, image.Width, image.Height);
    e.Graphics.DrawImageUnscaled(image, rectangle);
    rectangle = new Rectangle(e.Bounds.Left + image.Width + 4, e.Bounds.Y, e.Bounds.Width - (image.Width + 4), e.Bounds.Height);
    e.Graphics.DrawString(s, e.Style.Font, e.Style.ForeBrush, (RectangleF) rectangle, e.Style.StrFormat);
    e.Handled = true;
  }

  private void btnDelete_Click(object sender, EventArgs e) => this.DeleteSign();

  private void DeleteSign()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!sessionKeeper.Session.IsAdmin)
        return;
      int[] selectedRows = this._view.GetSelectedRows();
      if (selectedRows == null || selectedRows.Length == 0 || MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("Signs_DeleteNSigns"), (object) selectedRows.Length), MessageDialogs.msgConfirmDelete, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      bool flag = false;
      for (int index = 0; index < selectedRows.Length; ++index)
      {
        DataRow dataRow = this._view.GetDataRow(selectedRows[index]);
        if (dataRow != null)
        {
          try
          {
            long int64 = Convert.ToInt64(dataRow[0]);
            IDBObject dbObject = sessionKeeper.Session.GetObject(int64);
            if (dbObject != null)
              sessionKeeper.Session.GetRelation(this._objectID, dbObject.ID)?.Delete((long) Consts.PurgeMode);
            flag = true;
          }
          catch (Exception ex)
          {
            ExceptionHelper.ExceptionService.ShowException(ex);
            break;
          }
        }
      }
      if (!flag)
        return;
      this.RefreshData();
    }
  }

  private void VerifySign()
  {
    int[] selectedRows = this._view.GetSelectedRows();
    if (selectedRows == null || selectedRows.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < selectedRows.Length; ++index)
      {
        DataRow dataRow = this._view.GetDataRow(selectedRows[index]);
        if (dataRow != null)
        {
          try
          {
            long int64 = Convert.ToInt64(dataRow[0]);
            IDBObject dbObject = sessionKeeper.Session.GetObject(int64);
            if (dbObject != null)
            {
              if (dbObject.ObjectType == SignsHolder.CryptoSignObjectTypeID)
              {
                X509Certificate2Collection certificates = (X509Certificate2Collection) null;
                if (this.statusColumnIndex != -1)
                {
                  string enumDescription = EnumDescConverter.GetEnumDescription((Enum) SignHelper.TranslateStatus(sessionKeeper.Session, this._objectID, dbObject.ObjectID, dbObject.ObjectType, DateTime.Now, DateTime.Now, out certificates));
                  this._view.SetRowCellValue(selectedRows[index], this._view.Columns[this.statusColumnIndex], (object) enumDescription);
                }
                if (this.certColumnIndex != -1)
                {
                  if (certificates != null)
                  {
                    if (certificates.Count > 0)
                    {
                      string str = certificates[0].SubjectName.Format(false);
                      this._view.SetRowCellValue(selectedRows[index], this._view.Columns[this.certColumnIndex], (object) str);
                    }
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            ExceptionHelper.ExceptionService.ShowException(ex);
          }
        }
      }
    }
  }

  private void btnVerify_Click(object sender, EventArgs e) => this.VerifySign();

  private sealed class SignsViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = LocalizationHolder.rm.GetString("Signs_54"),
        ImageIndex = namedImageList.ImageIndex("imgSign"),
        OrderID = 51
      };
    }
  }
}
