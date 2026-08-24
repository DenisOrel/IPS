// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.ClassificatorsFactory
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal class ClassificatorsFactory : PumpItemFactory
{
  public static string TableName = "CLASS_LIST";
  public static string TableColumns = "FOLDER_KEY, FOLDERNAME, OWNER, NOTE, FILEBODY, FORMULA, FOLDER_LEV, NOTALPHA, ORDER_ID, BITMAPTYPE";
  private static int idxFolderKey = -1;
  private static int idxFolderName = -1;
  private static int idxOwner = -1;
  private static int idxNote = -1;
  private static int idxFileBody = -1;
  private static int idxFormula = -1;
  private static int idxFolderLev = -1;
  private static int idxNotalpha = -1;
  private static int idxOrderId = -1;
  private static int idxBitmapType = -1;

  public ClassificatorsFactory(IDataReader dataReader, IAppManager appManager)
    : base(ClassificatorsFactory.TableName, dataReader, appManager)
  {
    string fieldName1 = "FOLDER_KEY";
    string fieldName2 = "FOLDERNAME";
    string fieldName3 = "OWNER";
    string fieldName4 = "NOTE";
    string fieldName5 = "FILEBODY";
    string fieldName6 = "FORMULA";
    string fieldName7 = "FOLDER_LEV";
    string fieldName8 = "NOTALPHA";
    string fieldName9 = "ORDER_ID";
    string fieldName10 = "BITMAPTYPE";
    ClassificatorsFactory.idxFolderKey = this.getFieldIndex(fieldName1);
    ClassificatorsFactory.idxFolderName = this.getFieldIndex(fieldName2);
    ClassificatorsFactory.idxOwner = this.getFieldIndex(fieldName3);
    ClassificatorsFactory.idxNote = this.getFieldIndex(fieldName4);
    ClassificatorsFactory.idxFileBody = this.getFieldIndex(fieldName5);
    ClassificatorsFactory.idxFormula = this.getFieldIndex(fieldName6);
    ClassificatorsFactory.idxFolderLev = this.getFieldIndex(fieldName7);
    ClassificatorsFactory.idxNotalpha = this.getFieldIndex(fieldName8);
    ClassificatorsFactory.idxOrderId = this.getFieldIndex(fieldName9);
    ClassificatorsFactory.idxBitmapType = this.getFieldIndex(fieldName10);
  }

  public IClassificatorItem NewItem(IDataReader idr)
  {
    ClassificatorsFactory.ClassificatorItem classificatorItem = new ClassificatorsFactory.ClassificatorItem();
    classificatorItem.folderKey = this.getString(idr, ClassificatorsFactory.idxFolderKey);
    classificatorItem.folderName = this.getString(idr, ClassificatorsFactory.idxFolderName);
    classificatorItem.owner = this.getInt32(idr, ClassificatorsFactory.idxOwner);
    classificatorItem.note = this.getString(idr, ClassificatorsFactory.idxNote);
    if (!idr.IsDBNull(ClassificatorsFactory.idxFileBody))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(ClassificatorsFactory.idxFileBody, (long) fieldOffset, buffer, 0, length);
          if (bytes > 0)
          {
            fieldOffset += bytes;
            memoryStream.Write(buffer, 0, bytes);
          }
          else
            break;
        }
        classificatorItem.fileBody = memoryStream.ToArray();
      }
      finally
      {
        memoryStream.Close();
      }
    }
    classificatorItem.formula = this.getString(idr, ClassificatorsFactory.idxFormula);
    classificatorItem.folderLev = this.getInt32(idr, ClassificatorsFactory.idxFolderLev);
    string str = this.getString(idr, ClassificatorsFactory.idxNotalpha);
    if (str != string.Empty && str.Length == 1)
      classificatorItem.notalpha = Convert.ToChar(str);
    classificatorItem.orderId = this.getInt32(idr, ClassificatorsFactory.idxOrderId);
    classificatorItem.bitmapType = this.getString(idr, ClassificatorsFactory.idxBitmapType);
    return (IClassificatorItem) classificatorItem;
  }

  private class ClassificatorItem : IClassificatorItem
  {
    internal string folderKey = string.Empty;
    internal string folderName = string.Empty;
    internal int owner;
    internal string note = string.Empty;
    internal byte[] fileBody;
    internal string formula = string.Empty;
    internal int folderLev = -1;
    internal char notalpha = 'Y';
    internal int orderId = -1;
    internal string bitmapType = string.Empty;
    private long parentID;
    private int objTypeID = -1;
    private long objectID;
    private long imageObjectID;

    public string FolderKey
    {
      get => this.folderKey;
      set => this.folderKey = value;
    }

    public string FolderName => this.folderName;

    public int Owner
    {
      get => this.owner;
      set => this.owner = value;
    }

    public string Note => this.note;

    public byte[] FileBody => this.fileBody;

    public string Formula => this.formula;

    public int FolderLev => this.folderLev;

    public char Notalpha => this.notalpha;

    public int OrderId => this.orderId;

    public string BitmapType => this.bitmapType;

    public long ParentID
    {
      get => this.parentID;
      set => this.parentID = value;
    }

    public int ObjTypeID
    {
      get => this.objTypeID;
      set => this.objTypeID = value;
    }

    public long ObjectID
    {
      get => this.objectID;
      set => this.objectID = value;
    }

    public long ImageObjectID
    {
      get => this.imageObjectID;
      set => this.imageObjectID = value;
    }
  }
}
