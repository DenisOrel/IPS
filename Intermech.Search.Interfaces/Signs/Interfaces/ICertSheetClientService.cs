// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.ICertSheetClientService
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Document.Model;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Служба работы с Удостоверяющими листами</summary>
public interface ICertSheetClientService
{
  /// <summary>Сформировать удостоверяющие листы на документы</summary>
  /// <param name="docIdList">список идентификаторов версий документов</param>
  /// <param name="silentMode">тихий режим</param>
  /// <param name="expiredAuthFileUsingMode">что делать с просроченными аутентичными файлами. в тихом режиме None=YesForAll</param>
  /// <returns></returns>
  List<ImDocument> CreateCertSheets(
    List<long> docIdList,
    bool silentMode,
    ref ExpiredAuthFileUsing expiredAuthFileUsingMode);
}
