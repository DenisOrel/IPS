// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SmdoBook
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Office.Client;

public class SmdoBook
{
  protected XmlDocument xmlDocument;
  protected DataTable dataTable;
  protected Guid bookGuid = Guid.Empty;
  protected string bookName = string.Empty;
  protected string innerBookName = string.Empty;
  protected string bookId = string.Empty;
  protected string createDate = string.Empty;
  protected string actualDate = string.Empty;

  public DataTable DataTable => this.dataTable;

  public Guid BookGuid => this.bookGuid;

  public string BookName => this.bookName;

  public string InnerBookName => this.innerBookName;

  public string BookId => this.bookId;

  public string CreateDate => this.createDate;

  public string ActualDate => this.actualDate;

  public virtual void LoadBook() => throw new Exception();

  protected void LoadBook(Guid smdoBookGuid)
  {
    this.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(smdoBookGuid, false);
      this.bookName = dbObject != null ? dbObject.Caption : throw new Exception($"Не найден справочник СМДО \"{smdoBookGuid.ToString()}\"");
      IDBAttribute byGuid = dbObject.Attributes.FindByGUID(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
      if (byGuid == null)
      {
        this.ThrowBookNotLoaded();
      }
      else
      {
        IBlobReader blobReader = (IBlobReader) byGuid;
        if (blobReader == null)
          this.ThrowBookNotLoaded();
        if (blobReader.OpenBlob(-1).RealFileSize == 0L)
          this.ThrowBookNotLoaded();
        using (MemoryStream memoryStream = new MemoryStream())
        {
          new BlobProcReader(byGuid, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
          memoryStream.Position = 0L;
          this.xmlDocument.Load((Stream) memoryStream);
        }
      }
      this.bookGuid = smdoBookGuid;
    }
    this.ParseBook();
  }

  private void Clear()
  {
    this.bookGuid = Guid.Empty;
    this.bookName = string.Empty;
    this.xmlDocument = new XmlDocument();
    this.dataTable = new DataTable();
  }

  public virtual void ParseBook()
  {
    if (this.xmlDocument.SelectSingleNode("/" + Tag.dictionaryData) == null)
      this.ThrowTagNotFound(Tag.dictionaryData);
    if (this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.model}") == null)
      this.ThrowTagNotFound(Tag.model);
    XmlNode xmlNode1 = this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.model}/{Tag.header}");
    if (xmlNode1 == null)
      this.ThrowTagNotFound(Tag.header);
    if (this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.data}") == null)
      this.ThrowTagNotFound(Tag.data);
    XmlNode xmlNode2 = this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.data}/{Tag.rows}");
    if (xmlNode2 == null)
      this.ThrowTagNotFound(Tag.rows);
    this.innerBookName = this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.model}/{Tag.name}").InnerText;
    this.bookId = this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.model}/{Tag.dictionaryId}").InnerText;
    this.createDate = this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.model}/{Tag.createDate}").InnerText;
    this.actualDate = this.xmlDocument.SelectSingleNode($"/{Tag.dictionaryData}/{Tag.model}/{Tag.actualDate}").InnerText;
    this.dataTable.Columns.Add(Tag.rowId, typeof (Guid));
    XmlNodeList xmlNodeList1 = xmlNode1.SelectNodes(Tag.field);
    for (int i = 0; i < xmlNodeList1.Count; ++i)
    {
      XmlNode xmlNode3 = xmlNodeList1[i];
      this.dataTable.Columns.Add(xmlNode3.SelectSingleNode(Tag.name).InnerText, SmdoBookProc.ConvertSmdoFieldType(xmlNode3.SelectSingleNode(Tag.type).InnerText));
    }
    XmlNodeList xmlNodeList2 = xmlNode2.SelectNodes(Tag.row);
    for (int i1 = 0; i1 < xmlNodeList2.Count; ++i1)
    {
      XmlNode xmlNode4 = xmlNodeList2[i1];
      List<object> objectList = new List<object>((IEnumerable<object>) new object[1]
      {
        (object) new Guid(xmlNode4.SelectSingleNode(Tag.rowId).InnerText)
      });
      XmlNodeList xmlNodeList3 = xmlNode4.SelectSingleNode(Tag.columns).SelectNodes(Tag.column);
      for (int i2 = 0; i2 < xmlNodeList3.Count; ++i2)
      {
        string innerText = xmlNodeList3[i2].InnerText;
        DataColumn column = this.dataTable.Columns[i2 + 1];
        object obj = !(column.DataType == typeof (long)) ? (!(column.DataType == typeof (Guid)) ? (object) innerText : (object) new Guid(innerText)) : (object) Convert.ToInt64(innerText);
        objectList.Add(obj);
      }
      this.dataTable.Rows.Add(objectList.ToArray());
    }
    this.dataTable.AcceptChanges();
  }

  private void ThrowBookNotLoaded()
  {
    throw new Exception($"Не загружен справочник СМДО \"{this.bookName}\"");
  }

  private void ThrowTagNotFound(string tagName)
  {
    throw new Exception($"В справочнике \"{this.bookName}\" не обнаружен тэг \"{tagName}\"");
  }
}
