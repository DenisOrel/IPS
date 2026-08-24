// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImTablesItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImTablesItemFactory : PumpItemFactory
{
  public static string TableName = "IM_TABLES";
  private static int idxKey = -1;
  private static int idxTable = -1;
  private static int idxType = -1;
  private static int idxState = -1;
  private static int idxDescr = -1;
  private static int idxCreated = -1;
  private static int idxModified = -1;
  private static int idxUser = -1;
  private static int idxOpenmode = -1;
  private static int idxOrder = -1;
  private static int idxNextkey = -1;
  private static int idxTextid = -1;
  private static int idxGraphid = -1;
  private static int idxAccess = -1;

  public ImTablesItemFactory(IDataReader idr, IAppManager appMgr)
    : base(ImTablesItemFactory.TableName, idr, appMgr)
  {
    string fieldName1 = "F_KEY";
    string fieldName2 = "F_TABLE";
    string fieldName3 = "F_TYPE";
    string fieldName4 = "F_STATE";
    string fieldName5 = "F_DESCR";
    string fieldName6 = "F_CREATED";
    string fieldName7 = "F_MODIFIED";
    string fieldName8 = "F_USER";
    string fieldName9 = "F_OPENMODE";
    string fieldName10 = "F_ORDER";
    string fieldName11 = "F_NEXTKEY";
    string fieldName12 = "F_TEXTID";
    string fieldName13 = "F_GRAPHID";
    string fieldName14 = "F_ACCESS";
    ImTablesItemFactory.idxKey = this.getFieldIndex(fieldName1);
    ImTablesItemFactory.idxTable = this.getFieldIndex(fieldName2);
    ImTablesItemFactory.idxType = this.getFieldIndex(fieldName3);
    ImTablesItemFactory.idxState = this.getFieldIndex(fieldName4);
    ImTablesItemFactory.idxDescr = this.getFieldIndex(fieldName5);
    ImTablesItemFactory.idxCreated = this.getFieldIndex(fieldName6);
    ImTablesItemFactory.idxModified = this.getFieldIndex(fieldName7);
    ImTablesItemFactory.idxUser = this.getFieldIndex(fieldName8);
    ImTablesItemFactory.idxOpenmode = this.getFieldIndex(fieldName9);
    ImTablesItemFactory.idxOrder = this.getFieldIndex(fieldName10);
    ImTablesItemFactory.idxNextkey = this.getFieldIndex(fieldName11);
    ImTablesItemFactory.idxTextid = this.getFieldIndex(fieldName12);
    ImTablesItemFactory.idxGraphid = this.getFieldIndex(fieldName13);
    ImTablesItemFactory.idxAccess = this.getFieldIndex(fieldName14);
  }

  public IImTablesItem NewItem(IDataReader idr)
  {
    ImTablesItemFactory.ImTablesItem imTablesItem = new ImTablesItemFactory.ImTablesItem();
    imTablesItem.key = this.getInt32(idr, ImTablesItemFactory.idxKey);
    imTablesItem.tableName = this.getString(idr, ImTablesItemFactory.idxTable).Trim();
    switch (this.getString(idr, ImTablesItemFactory.idxType).Trim())
    {
      case "CATALOG":
        imTablesItem.tableType = ImTablesType.IMTT_CATALOG;
        break;
      case "CTLREC":
        imTablesItem.tableType = ImTablesType.IMTT_CTLREC;
        break;
      case "CTLREF":
        imTablesItem.tableType = ImTablesType.IMTT_CTLREF;
        break;
      case "INDEX":
        imTablesItem.tableType = ImTablesType.IMTT_INDEX;
        break;
      case "TABLE":
      case "TBLREF":
        imTablesItem.tableType = ImTablesType.IMTT_TABLE;
        break;
      case "TCREF":
      case "TECHREF":
        imTablesItem.tableType = ImTablesType.IMTT_TECHREF;
        break;
      default:
        imTablesItem.tableType = ImTablesType.IMTT_UNKNOWN;
        break;
    }
    imTablesItem.state = (ImFileAtt) this.getInt32(idr, ImTablesItemFactory.idxState);
    imTablesItem.description = this.getString(idr, ImTablesItemFactory.idxDescr).Trim();
    imTablesItem.created = this.getDateTime(idr, ImTablesItemFactory.idxCreated);
    imTablesItem.modified = this.getDateTime(idr, ImTablesItemFactory.idxModified);
    imTablesItem.user = this.getString(idr, ImTablesItemFactory.idxUser).Trim();
    imTablesItem.openmode = this.getInt32(idr, ImTablesItemFactory.idxOpenmode);
    imTablesItem.order = this.getInt32(idr, ImTablesItemFactory.idxOrder);
    imTablesItem.nextkey = this.getInt32(idr, ImTablesItemFactory.idxNextkey);
    imTablesItem.textID = this.getInt32(idr, ImTablesItemFactory.idxTextid);
    imTablesItem.graphID = this.getInt32(idr, ImTablesItemFactory.idxGraphid);
    imTablesItem.access = this.getInt32(idr, ImTablesItemFactory.idxAccess);
    return (IImTablesItem) imTablesItem;
  }

  internal class ImTablesItem(string caption) : SettingsGroupItem(caption), IImTablesItem, ISettingsGroupItem
  {
    internal int key;
    internal string tableName = "";
    internal ImTablesType tableType;
    internal ImFileAtt state;
    internal string description = "";
    internal DateTime created = DateTime.Now;
    internal DateTime modified = DateTime.Now;
    internal string user = "";
    internal int openmode;
    internal int order;
    internal int nextkey;
    internal int textID;
    internal int graphID;
    internal int access;
    internal Guid recordsTypeGuid = Guid.Empty;
    internal List<ITableFieldInfo> existingFields = new List<ITableFieldInfo>();
    internal long objectID;
    internal List<string> usedInCatalogs = new List<string>();

    public ImTablesItem()
      : this("")
    {
    }

    public override string Caption
    {
      get => !(this.description != string.Empty) ? this.tableName : this.description;
    }

    public int Key => this.key;

    public string TableName => this.tableName;

    public ImTablesType TableType => this.tableType;

    public ImFileAtt State => this.state;

    public string Description => this.description;

    public DateTime Created => this.created;

    public DateTime Modified => this.modified;

    public string User => this.user;

    public int Openmode => this.openmode;

    public int Order => this.order;

    public int Nextkey => this.nextkey;

    public int TextID => this.textID;

    public int GraphID => this.graphID;

    public int Access => this.access;

    public Guid RecordsTypeGuid
    {
      get => this.recordsTypeGuid;
      set
      {
        if (!(this.recordsTypeGuid != value))
          return;
        this.recordsTypeGuid = value;
      }
    }

    public IList<ITableFieldInfo> ExistingFields => (IList<ITableFieldInfo>) this.existingFields;

    public bool FieldExistInBase(string fieldName)
    {
      bool flag = false;
      for (int index = 0; index < this.existingFields.Count && !flag; ++index)
        flag = this.existingFields[index].ColumnName.Equals(fieldName);
      return flag;
    }

    public ITableFieldInfo GetFieldInfo(string fieldName)
    {
      ITableFieldInfo fieldInfo = (ITableFieldInfo) null;
      for (int index = 0; index < this.existingFields.Count && fieldInfo == null; ++index)
      {
        if (this.existingFields[index].ColumnName.Equals(fieldName))
          fieldInfo = this.existingFields[index];
      }
      return fieldInfo;
    }

    public long ObjectID
    {
      get => this.objectID;
      set
      {
        if (this.objectID == value)
          return;
        this.objectID = value;
      }
    }

    public IList<string> UsedInCatalogs => (IList<string>) this.usedInCatalogs;
  }
}
