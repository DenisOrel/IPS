// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.RepDocTraceInfo
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Document.Model;
using Intermech.Expert.User;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using Intermech.Localization;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>Класс с информацией о трассировке генерации документа</summary>
internal class RepDocTraceInfo : DocTraceInfo
{
  /// <summary>Информация по документу</summary>
  private DocRecord _docRecord;

  /// <summary>Конструктор</summary>
  /// <param name="text"></param>
  public RepDocTraceInfo(string text)
    : base(text, (ImDocument) null)
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="docRecord"></param>
  /// <param name="traceInfo"></param>
  public RepDocTraceInfo(DocRecord docRecord, byte[] traceInfo)
    : base(string.Empty, (ImDocument) null)
  {
    if (docRecord != null)
    {
      this._text = docRecord.docName;
      if (docRecord.errorMsg != string.Empty)
        this._report = new string[1]
        {
          string.Format(LocalizationHolder.rm.GetString("Reports_5"), (object) docRecord.errorMsg)
        };
      this._docRecord = docRecord;
    }
    if (traceInfo == null)
      return;
    this._traceData = traceInfo;
  }

  /// <summary>Копирование данных из трассировки задачи</summary>
  /// <param name="reportTraceInfo"></param>
  public void CopyFrom(ReportTraceInfo reportTraceInfo)
  {
    if (reportTraceInfo == null)
      return;
    if (reportTraceInfo.TraceInfo != null && reportTraceInfo.TraceInfo.Length != 0)
      this.TraceData = reportTraceInfo.TraceInfo;
    this.Report = reportTraceInfo.ReportInfo;
  }

  /// <summary>Поиск записи документа по его номеру</summary>
  /// <param name="docNumber">Номер документа</param>
  /// <returns></returns>
  public RepDocTraceInfo GetTraceInfo(int docNumber)
  {
    if (this._docRecord != null && this._docRecord.docNumber == docNumber)
      return this;
    RepDocTraceInfo traceInfo = (RepDocTraceInfo) null;
    if (this.ChildItems != null)
    {
      foreach (RepDocTraceInfo childItem in this.ChildItems)
      {
        traceInfo = childItem.GetTraceInfo(docNumber);
        if (traceInfo != null)
          break;
      }
    }
    return traceInfo;
  }

  /// <summary>Удаление мусора</summary>
  public override void ClearData()
  {
    base.ClearData();
    this._docRecord = (DocRecord) null;
  }

  /// <summary>Информация по документу</summary>
  public DocRecord DocRecord => this._docRecord;
}
