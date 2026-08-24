// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ArchivesItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class ArchivesItemFactory : PumpItemFactory
{
  public static string TableName = "ARCHIVES";
  public static string TableColumns = "ARCHIVE_ID, ALIAS, FILENAME, STRONGSIGN, SIGN_STAMP, DESCRIPTIO, MAKEREVIS, PERSONID, SIGN_TYPE, PARENT_ID, CHKRIGHTS, STORAGE_ID, CFG_DATA";
  private static int idxArchiveId = -1;
  private static int idxAlias = -1;
  private static int idxFileName = -1;
  private static int idxStrongSign = -1;
  private static int idxSignStamp = -1;
  private static int idxDescription = -1;
  private static int idxMakeRevis = -1;
  private static int idxCfgData = -1;
  private static int idxPersonId = -1;
  private static int idxSignType = -1;
  private static int idxParentId = -1;
  private static int idxChkRights = -1;
  private static int idxStorageId = -1;

  public ArchivesItemFactory(IDataReader idr, IAppManager appMgr)
    : base(ArchivesItemFactory.TableName, idr, appMgr)
  {
    string fieldName1 = "ARCHIVE_ID";
    string fieldName2 = "ALIAS";
    string fieldName3 = "FILENAME";
    string fieldName4 = "STRONGSIGN";
    string fieldName5 = "SIGN_STAMP";
    string fieldName6 = "DESCRIPTIO";
    string fieldName7 = "MAKEREVIS";
    string fieldName8 = "CFG_DATA";
    string fieldName9 = "PERSONID";
    string fieldName10 = "SIGN_TYPE";
    string fieldName11 = "PARENT_ID";
    string fieldName12 = "CHKRIGHTS";
    string fieldName13 = "STORAGE_ID";
    ArchivesItemFactory.idxArchiveId = this.getFieldIndex(fieldName1);
    ArchivesItemFactory.idxAlias = this.getFieldIndex(fieldName2);
    ArchivesItemFactory.idxFileName = this.getFieldIndex(fieldName3);
    ArchivesItemFactory.idxStrongSign = this.getFieldIndex(fieldName4);
    ArchivesItemFactory.idxSignStamp = this.getFieldIndex(fieldName5);
    ArchivesItemFactory.idxDescription = this.getFieldIndex(fieldName6);
    ArchivesItemFactory.idxMakeRevis = this.getFieldIndex(fieldName7);
    ArchivesItemFactory.idxCfgData = this.getFieldIndex(fieldName8);
    ArchivesItemFactory.idxPersonId = this.getFieldIndex(fieldName9);
    ArchivesItemFactory.idxSignType = this.getFieldIndex(fieldName10);
    ArchivesItemFactory.idxParentId = this.getFieldIndex(fieldName11);
    ArchivesItemFactory.idxChkRights = this.getFieldIndex(fieldName12);
    ArchivesItemFactory.idxStorageId = this.getFieldIndex(fieldName13);
  }

  public IArchivesItem NewItem(IDataReader idr)
  {
    ArchivesItemFactory.ArchivesItem archivesItem = new ArchivesItemFactory.ArchivesItem();
    archivesItem.archiveID = this.getInt32(idr, ArchivesItemFactory.idxArchiveId);
    archivesItem.alias = this.getString(idr, ArchivesItemFactory.idxAlias).Trim().ToUpper();
    archivesItem.fileName = this.getString(idr, ArchivesItemFactory.idxFileName).Trim();
    archivesItem.strongSign = this.getInt32(idr, ArchivesItemFactory.idxStrongSign);
    archivesItem.signStamp = this.getString(idr, ArchivesItemFactory.idxSignStamp).Trim();
    archivesItem.descriptio = this.getString(idr, ArchivesItemFactory.idxDescription).Trim();
    archivesItem.makeRvis = this.getInt32(idr, ArchivesItemFactory.idxMakeRevis);
    archivesItem.personId = this.getInt32(idr, ArchivesItemFactory.idxPersonId);
    archivesItem.signType = this.getInt32(idr, ArchivesItemFactory.idxSignType);
    archivesItem.parentID = this.getInt32(idr, ArchivesItemFactory.idxParentId);
    archivesItem.chkRights = this.getInt32(idr, ArchivesItemFactory.idxChkRights);
    archivesItem.storageId = this.getInt32(idr, ArchivesItemFactory.idxStorageId);
    if (!idr.IsDBNull(ArchivesItemFactory.idxCfgData))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(ArchivesItemFactory.idxCfgData, (long) fieldOffset, buffer, 0, length);
          if (bytes > 0)
          {
            fieldOffset += bytes;
            memoryStream.Write(buffer, 0, bytes);
          }
          else
            break;
        }
        memoryStream.Position = 0L;
        StreamReader streamReader = new StreamReader((Stream) memoryStream, this.dataBaseEncoding);
        try
        {
          while (streamReader.Peek() >= 0)
          {
            string str1 = streamReader.ReadLine();
            if (str1 != string.Empty)
            {
              string key = str1.Substring(0, str1.IndexOf("="));
              string str2 = str1.Substring(str1.IndexOf("=") + 1);
              if (!archivesItem.cfgData.ContainsKey(key) && key.IndexOf(".Lookup") == -1 && key.IndexOf(".Integer") == -1)
                archivesItem.cfgData.Add(key, str2);
            }
          }
        }
        finally
        {
          streamReader.Close();
        }
      }
      finally
      {
        memoryStream.Close();
      }
    }
    return (IArchivesItem) archivesItem;
  }

  internal class ArchivesItem : IArchivesItem
  {
    public int archiveID;
    public string alias = "";
    public string fileName = "";
    public int strongSign;
    public string signStamp = "";
    public string descriptio = "";
    public int makeRvis;
    public Dictionary<string, string> cfgData = new Dictionary<string, string>();
    public int personId;
    public int signType;
    public int parentID;
    public int chkRights;
    public int storageId;
    public long objectID;

    public int ArchiveID => this.archiveID;

    public string Alias => this.alias;

    public string FileName => this.fileName;

    public int StrongSign => this.strongSign;

    public string SignStamp => this.signStamp;

    public string Descriptio => this.descriptio;

    public int MakeRvis => this.makeRvis;

    public Dictionary<string, string> CfgData => this.cfgData;

    public int PersonId => this.personId;

    public int SignType => this.signType;

    public int ParentID => this.parentID;

    public int ChkRights => this.chkRights;

    public int StorageId => this.storageId;

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
  }
}
