// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ArticleTypesItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class ArticleTypesItemFactory : PumpItemFactory
{
  public static string TableName = "SSECTIONS";
  public static string TableColumns = "SECTION_ID, SECTNAME, DOC_TYPE, ART_KIND,BITMAP, NOTE, TRANS_ACT, MU_ON, IMBASEONLY, ORDER_ID, PR_ID, F_VERSIONABLE,MULTIDESIGNATIO, F_CONTROL_DELETE, CFG_DATA";
  private static int idxSectionId = -1;
  private static int idxSectName = -1;
  private static int idxDocType = -1;
  private static int idxArtKind = -1;
  private static int idxBitmap = -1;
  private static int idxNote = -1;
  private static int idxCfgData = -1;
  private static int idxTransAct = -1;
  private static int idxMuOn = -1;
  private static int idxImbaseOnly = -1;
  private static int idxOrderId = -1;
  private static int idxPrId = -1;
  private static int idxVersionable = -1;
  private static int idxMultidesignatio = -1;
  private static int idxControlDelete = -1;

  public ArticleTypesItemFactory(string tableName, IDataReader dataReader, IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "SECTION_ID";
    string fieldName2 = "SECTNAME";
    string fieldName3 = "DOC_TYPE";
    string fieldName4 = "ART_KIND";
    string fieldName5 = "BITMAP";
    string fieldName6 = "NOTE";
    string fieldName7 = "CFG_DATA";
    string fieldName8 = "TRANS_ACT";
    string fieldName9 = "MU_ON";
    string fieldName10 = "IMBASEONLY";
    string fieldName11 = "ORDER_ID";
    string fieldName12 = "PR_ID";
    string fieldName13 = "F_VERSIONABLE";
    string fieldName14 = "MULTIDESIGNATIO";
    string fieldName15 = "F_CONTROL_DELETE";
    ArticleTypesItemFactory.idxSectionId = this.getFieldIndex(fieldName1);
    ArticleTypesItemFactory.idxSectName = this.getFieldIndex(fieldName2);
    ArticleTypesItemFactory.idxDocType = this.getFieldIndex(fieldName3);
    ArticleTypesItemFactory.idxArtKind = this.getFieldIndex(fieldName4);
    ArticleTypesItemFactory.idxBitmap = this.getFieldIndex(fieldName5);
    ArticleTypesItemFactory.idxNote = this.getFieldIndex(fieldName6);
    ArticleTypesItemFactory.idxCfgData = this.getFieldIndex(fieldName7);
    ArticleTypesItemFactory.idxTransAct = this.getFieldIndex(fieldName8);
    ArticleTypesItemFactory.idxMuOn = this.getFieldIndex(fieldName9);
    ArticleTypesItemFactory.idxImbaseOnly = this.getFieldIndex(fieldName10);
    ArticleTypesItemFactory.idxOrderId = this.getFieldIndex(fieldName11);
    ArticleTypesItemFactory.idxPrId = this.getFieldIndex(fieldName12);
    ArticleTypesItemFactory.idxVersionable = this.getFieldIndex(fieldName13);
    ArticleTypesItemFactory.idxMultidesignatio = this.getFieldIndex(fieldName14);
    ArticleTypesItemFactory.idxControlDelete = this.getFieldIndex(fieldName15);
  }

  public IArticleTypesItem NewItem(IDataReader idr, Guid guid)
  {
    ArticleTypesItemFactory.ArticleTypesItem articleTypesItem = new ArticleTypesItemFactory.ArticleTypesItem(guid);
    articleTypesItem.sectionId = this.getInt32(idr, ArticleTypesItemFactory.idxSectionId);
    articleTypesItem.sectName = this.getString(idr, ArticleTypesItemFactory.idxSectName);
    if (articleTypesItem.sectName == string.Empty)
      articleTypesItem.sectName = $"Новый тип изделий {Guid.NewGuid()}";
    articleTypesItem.docType = this.getInt32(idr, ArticleTypesItemFactory.idxDocType);
    articleTypesItem.artKind = this.getString(idr, ArticleTypesItemFactory.idxArtKind);
    articleTypesItem.bitmap = this.getString(idr, ArticleTypesItemFactory.idxBitmap);
    articleTypesItem.note = this.getString(idr, ArticleTypesItemFactory.idxNote);
    articleTypesItem.transAct = this.getInt32(idr, ArticleTypesItemFactory.idxTransAct);
    articleTypesItem.muOn = this.getString(idr, ArticleTypesItemFactory.idxMuOn);
    articleTypesItem.imbaseOnly = this.getString(idr, ArticleTypesItemFactory.idxImbaseOnly);
    articleTypesItem.orderId = this.getInt32(idr, ArticleTypesItemFactory.idxOrderId);
    articleTypesItem.prId = this.getInt32(idr, ArticleTypesItemFactory.idxPrId);
    articleTypesItem.VersionMode = this.getInt32(idr, ArticleTypesItemFactory.idxVersionable) == 0 ? ObjectVersionModes.Abstract : ObjectVersionModes.MultiVersion;
    articleTypesItem.multidesignatio = this.getInt32(idr, ArticleTypesItemFactory.idxMultidesignatio);
    articleTypesItem.controlDelete = this.getInt32(idr, ArticleTypesItemFactory.idxControlDelete);
    if (!idr.IsDBNull(ArticleTypesItemFactory.idxCfgData))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(ArticleTypesItemFactory.idxCfgData, (long) fieldOffset, buffer, 0, length);
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
          AtricleTypeField atricleTypeField = new AtricleTypeField();
          while (streamReader.Peek() >= 0)
          {
            string str1 = streamReader.ReadLine();
            if (str1 != string.Empty)
            {
              string empty = string.Empty;
              bool flag1 = false;
              string str2 = str1.Substring(0, str1.IndexOf("="));
              string str3 = str1.Substring(str1.IndexOf("=") + 1);
              bool flag2 = str2.IndexOf(".") != -1;
              string str4;
              if (str2.IndexOf(".ImBase") != -1)
              {
                str4 = str2.Substring(0, str1.IndexOf("."));
                int num = 0;
                try
                {
                  num = Convert.ToInt32(str3.Trim());
                }
                catch
                {
                }
                flag1 = num == 1;
              }
              else if (!flag2)
                str4 = str2;
              else
                continue;
              if (atricleTypeField.Name == string.Empty)
              {
                atricleTypeField.Name = str4;
                if (!flag2)
                  atricleTypeField.Caption = str3;
                else
                  atricleTypeField.ImbaseObject = flag1;
              }
              else
              {
                if (!atricleTypeField.Name.Equals(str4))
                {
                  articleTypesItem.cfgData.Add(atricleTypeField);
                  atricleTypeField = new AtricleTypeField();
                  atricleTypeField.Name = str4;
                }
                if (!flag2)
                  atricleTypeField.Caption = str3;
                else
                  atricleTypeField.ImbaseObject = flag1;
              }
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
    return (IArticleTypesItem) articleTypesItem;
  }

  private class ArticleTypesItem : IArticleTypesItem
  {
    internal int sectionId;
    internal string sectName = "";
    internal int docType;
    internal string artKind = "";
    internal string bitmap = "";
    internal string note = "";
    internal int transAct;
    internal string muOn = "";
    internal string imbaseOnly = "";
    internal int orderId;
    internal int prId;
    internal int multidesignatio;
    internal int controlDelete;
    internal List<AtricleTypeField> cfgData = new List<AtricleTypeField>();
    internal Dictionary<int, IArticleTypesItem> parents = new Dictionary<int, IArticleTypesItem>();
    internal Dictionary<int, IArticleTypesItem> childs = new Dictionary<int, IArticleTypesItem>();
    internal bool isTreeRoot;
    internal Dictionary<int, IArticleTypesItem> treeChilds = new Dictionary<int, IArticleTypesItem>();
    private Guid guid;
    private ObjectVersionModes versionMode;
    private byte[] icon;
    private Guid parentID;
    private Guid defRelation;
    private Guid lcScheme;
    private bool anyAttribute = true;

    public ArticleTypesItem(Guid newGuid) => this.guid = newGuid;

    public int SectionId => this.sectionId;

    public string SectName
    {
      get => this.sectName;
      set => this.sectName = value;
    }

    public int DocType => this.docType;

    public string ArtKind => this.sectName;

    public string Bitmap => this.bitmap;

    public string Note => this.note;

    public int TransAct => this.transAct;

    public string MuOn => this.muOn;

    public string ImbaseOnly => this.imbaseOnly;

    public int OrderId => this.orderId;

    public int PrId => this.prId;

    public int Multidesignatio => this.multidesignatio;

    public int ControlDelete => this.controlDelete;

    public List<AtricleTypeField> CfgData => this.cfgData;

    public IDictionary<int, IArticleTypesItem> Parents
    {
      get => (IDictionary<int, IArticleTypesItem>) this.parents;
    }

    public IDictionary<int, IArticleTypesItem> Childs
    {
      get => (IDictionary<int, IArticleTypesItem>) this.childs;
    }

    public bool IsTreeRoot
    {
      get => this.isTreeRoot;
      set
      {
        if (this.isTreeRoot.Equals(value))
          return;
        this.isTreeRoot = value;
      }
    }

    public IDictionary<int, IArticleTypesItem> TreeChilds
    {
      get => (IDictionary<int, IArticleTypesItem>) this.treeChilds;
    }

    public Guid Guid => this.guid;

    public byte[] Icon
    {
      get => this.icon;
      set => this.icon = value;
    }

    public ObjectVersionModes VersionMode
    {
      get => this.versionMode;
      set => this.versionMode = value;
    }

    public Guid ParentID
    {
      get => this.parentID;
      set => this.parentID = value;
    }

    public Guid DefRelation
    {
      get => this.defRelation;
      set => this.defRelation = value;
    }

    public Guid LCScheme
    {
      get => this.lcScheme;
      set => this.lcScheme = value;
    }

    public bool AnyAttribute
    {
      get => this.anyAttribute;
      set => this.anyAttribute = value;
    }
  }
}
