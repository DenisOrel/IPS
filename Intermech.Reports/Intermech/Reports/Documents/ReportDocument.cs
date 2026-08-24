// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.ReportDocument
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Diagnostics;
using Intermech.Interfaces.Expert;
using System;
using System.Data;

#nullable disable
namespace Intermech.Reports.Documents;

/// <summary>Класс описание генерируемого документа</summary>
internal class ReportDocument
{
  /// <summary>
  /// 
  /// </summary>
  private DocRecord _docRecord;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="syncObject">Объект для синхронизации серверных операций</param>
  /// <param name="docRecord">Описание документа ЭС </param>
  public ReportDocument([NotNull] object syncObject, [NotNull] DocRecord docRecord)
  {
    this.SyncObject = syncObject;
    this.DocRecord = docRecord;
  }

  /// <summary>Объект для синхронизации серверных операций</summary>
  /// <remarks>
  /// Для избежания одноговременной модификации одного и того же объекта в раззных потоках
  /// </remarks>
  /// &gt;
  public object SyncObject { get; private set; }

  /// <summary>Описание документа ЭС</summary>
  public DocRecord DocRecord
  {
    get => this._docRecord;
    set
    {
      this._docRecord = value != null ? value : throw new NoNullAllowedException("DocRecord can't be null");
    }
  }

  /// <summary>
  /// 
  /// </summary>
  internal ReportDocumentType DocumentType
  {
    get
    {
      return !this.DocRecord.state.HasFlag((Enum) DocState.DocLink) ? ReportDocumentType.ImDocument : ReportDocumentType.ImExternalDocument;
    }
  }
}
