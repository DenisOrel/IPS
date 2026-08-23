// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.CertSheetData
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Search.Interfaces;

/// <summary>
/// Класс, содержащий краткую информацию по документу.
/// Содержит информацию, необходимую для генерации удостоверяющих листов.
/// </summary>
public class CertSheetData
{
  /// <summary>идентификатор версии объекта</summary>
  public long ObjectId;
  /// <summary>
  /// ObjectId сам является документом; Docs тогда не имеет смысла -&gt; смотреть CertSheetDataList
  /// </summary>
  public bool IsDocument;
  /// <summary>
  /// список документов на объект, имеет смысл при IsDocument = false
  /// </summary>
  public List<long> Docs;
  /// <summary>группирующий признак</summary>
  public DocGroupType DocGroupType;
  /// <summary>
  /// список сгруппированных объектов (в случае извещения или состава).
  /// для обычного ( DocGroupType = DocGroupType.None )  ObjectId = null
  /// </summary>
  public CertSheetDataList CertSheetDataList;

  public CertSheetData()
  {
    this.ObjectId = -1L;
    this.IsDocument = false;
    this.Docs = new List<long>();
  }

  public CertSheetData(long objectId)
    : this(objectId, true, (List<long>) null)
  {
  }

  public CertSheetData(long objectId, bool isDocument, List<long> docs)
  {
    this.ObjectId = objectId;
    this.IsDocument = isDocument;
    this.Docs = docs;
  }

  public void Init(DocGroupType docGroupType, CertSheetDataList certSheetDataList)
  {
    this.DocGroupType = docGroupType;
    this.CertSheetDataList = certSheetDataList;
  }
}
