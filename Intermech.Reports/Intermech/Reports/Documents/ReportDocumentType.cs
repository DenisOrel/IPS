// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.ReportDocumentType
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

#nullable disable
namespace Intermech.Reports.Documents;

/// <summary>Тип документа</summary>
internal enum ReportDocumentType
{
  /// <summary>Тип не определен</summary>
  Unknown,
  /// <summary>"Стандартный" документ IPS</summary>
  ImDocument,
  /// <summary>Ссылочный (внешний) документ IPS</summary>
  ImExternalDocument,
}
