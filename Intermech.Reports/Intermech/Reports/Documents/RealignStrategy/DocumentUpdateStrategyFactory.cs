// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.RealignStrategy.DocumentUpdateStrategyFactory
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Diagnostics;
using System;

#nullable disable
namespace Intermech.Reports.Documents.RealignStrategy;

internal class DocumentUpdateStrategyFactory
{
  /// <summary>
  /// 
  /// </summary>
  private static readonly Lazy<DocumentUpdateStrategyFactory> _lazyInstance = new Lazy<DocumentUpdateStrategyFactory>();

  public DocumentRealignStrategy CreateStrategy([NotNull] ReportDocument reportDocument)
  {
    switch (reportDocument.DocumentType)
    {
      case ReportDocumentType.ImDocument:
        return (DocumentRealignStrategy) new ImDocumentRealignStrategy();
      case ReportDocumentType.ImExternalDocument:
        return (DocumentRealignStrategy) new ImExternalDocumentRealignStrategy();
      default:
        return (DocumentRealignStrategy) null;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public static DocumentUpdateStrategyFactory Instance
  {
    get => DocumentUpdateStrategyFactory._lazyInstance.Value;
  }
}
