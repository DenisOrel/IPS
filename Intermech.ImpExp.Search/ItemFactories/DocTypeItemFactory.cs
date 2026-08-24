// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.DocTypeItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class DocTypeItemFactory : PumpItemFactory
{
  public static string TableName = "DOCTYPES";
  public static string TableColumns = "DOC_TYPE, DOC_CODE, DOC_NAME, DOC_EXT,BITMAP, DOC_COLOR, DRAWSHTAMP, SUFFIX, LINKEDEXT, REFSETUP, FILEBODY, PROTONAME,CLASSIF, DT_NAME, DT_CODE, STRONGSIGN, SIGNSTAMP";
  private static int idxDocType = -1;
  private static int idxDocCode = -1;
  private static int idxDocName = -1;
  private static int idxDocExt = -1;
  private static int idxBitmap = -1;
  private static int idxDocColor = -1;
  private static int idxDrawStamp = -1;
  private static int idxSuffix = -1;
  private static int idxLinkedExt = -1;
  private static int idxRefSetup = -1;
  private static int idxFileBody = -1;
  private static int idxProtoName = -1;
  private static int idxClassif = -1;
  private static int idxDTName = -1;
  private static int idxDTCode = -1;
  private static int idxStrongSign = -1;
  private static int idxSignStamp = -1;

  public DocTypeItemFactory(string tableName, IDataReader dataReader, IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "DOC_TYPE";
    string fieldName2 = "DOC_CODE";
    string fieldName3 = "DOC_NAME";
    string fieldName4 = "DOC_EXT";
    string fieldName5 = "BITMAP";
    string fieldName6 = "DOC_COLOR";
    string fieldName7 = "DRAWSHTAMP";
    string fieldName8 = "SUFFIX";
    string fieldName9 = "LINKEDEXT";
    string fieldName10 = "REFSETUP";
    string fieldName11 = "FILEBODY";
    string fieldName12 = "PROTONAME";
    string fieldName13 = "CLASSIF";
    string fieldName14 = "DT_NAME";
    string fieldName15 = "DT_CODE";
    string fieldName16 = "STRONGSIGN";
    string fieldName17 = "SIGNSTAMP";
    DocTypeItemFactory.idxDocType = this.getFieldIndex(fieldName1);
    DocTypeItemFactory.idxDocCode = this.getFieldIndex(fieldName2);
    DocTypeItemFactory.idxDocName = this.getFieldIndex(fieldName3);
    DocTypeItemFactory.idxDocExt = this.getFieldIndex(fieldName4);
    DocTypeItemFactory.idxBitmap = this.getFieldIndex(fieldName5);
    DocTypeItemFactory.idxDocColor = this.getFieldIndex(fieldName6);
    DocTypeItemFactory.idxDrawStamp = this.getFieldIndex(fieldName7);
    DocTypeItemFactory.idxSuffix = this.getFieldIndex(fieldName8);
    DocTypeItemFactory.idxLinkedExt = this.getFieldIndex(fieldName9);
    DocTypeItemFactory.idxRefSetup = this.getFieldIndex(fieldName10);
    DocTypeItemFactory.idxFileBody = this.getFieldIndex(fieldName11);
    DocTypeItemFactory.idxProtoName = this.getFieldIndex(fieldName12);
    DocTypeItemFactory.idxClassif = this.getFieldIndex(fieldName13);
    DocTypeItemFactory.idxDTName = this.getFieldIndex(fieldName14);
    DocTypeItemFactory.idxDTCode = this.getFieldIndex(fieldName15);
    DocTypeItemFactory.idxStrongSign = this.getFieldIndex(fieldName16);
    DocTypeItemFactory.idxSignStamp = this.getFieldIndex(fieldName17);
  }

  public IDocTypeItem NewItem(IDataReader idr, Guid newGuid)
  {
    DocTypeItem docTypeItem = new DocTypeItem();
    docTypeItem.docType = this.getInt32(idr, DocTypeItemFactory.idxDocType);
    docTypeItem.docCode = this.getString(idr, DocTypeItemFactory.idxDocCode).Trim();
    docTypeItem.docName = this.getString(idr, DocTypeItemFactory.idxDocName).Trim();
    if (docTypeItem.docName == string.Empty)
      docTypeItem.docName = $"Новый тип документов {Guid.NewGuid()}";
    docTypeItem.docExt = this.getString(idr, DocTypeItemFactory.idxDocExt).Trim();
    docTypeItem.bitmap = this.getString(idr, DocTypeItemFactory.idxBitmap).Trim();
    docTypeItem.docColor = 0;
    docTypeItem.drawStamp = this.getInt32(idr, DocTypeItemFactory.idxDrawStamp);
    docTypeItem.suffix = this.getInt32(idr, DocTypeItemFactory.idxSuffix);
    docTypeItem.linkedExt = this.getString(idr, DocTypeItemFactory.idxLinkedExt).Trim();
    docTypeItem.refSetup = this.getInt32(idr, DocTypeItemFactory.idxRefSetup);
    if (!idr.IsDBNull(DocTypeItemFactory.idxFileBody))
    {
      int length = 4096 /*0x1000*/;
      byte[] buffer = new byte[length];
      int fieldOffset = 0;
      MemoryStream memoryStream = new MemoryStream();
      try
      {
        while (true)
        {
          int bytes = (int) idr.GetBytes(DocTypeItemFactory.idxFileBody, (long) fieldOffset, buffer, 0, length);
          if (bytes > 0)
          {
            fieldOffset += bytes;
            memoryStream.Write(buffer, 0, bytes);
          }
          else
            break;
        }
        docTypeItem.fileBody = memoryStream.ToArray();
      }
      finally
      {
        memoryStream.Close();
      }
    }
    else
      docTypeItem.fileBody = (byte[]) null;
    docTypeItem.protoName = this.getString(idr, DocTypeItemFactory.idxProtoName).Trim();
    docTypeItem.classif = this.getString(idr, DocTypeItemFactory.idxClassif).Trim();
    docTypeItem.dTName = this.getString(idr, DocTypeItemFactory.idxDTName).Trim();
    docTypeItem.dTCode = this.getString(idr, DocTypeItemFactory.idxDTCode).Trim();
    docTypeItem.strongSign = this.getInt32(idr, DocTypeItemFactory.idxStrongSign);
    docTypeItem.signStamp = this.getString(idr, DocTypeItemFactory.idxSignStamp).Trim();
    docTypeItem.Guid = newGuid;
    return (IDocTypeItem) docTypeItem;
  }
}
