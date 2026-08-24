// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.Techcard.ITechCardTypeService
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.Techcard;

/// <summary>TechCard object type service</summary>
public interface ITechCardTypeService
{
  /// <summary>Getting IPS object type by techcard TP type name</summary>
  /// <param name="techTypeName"></param>
  /// <returns></returns>
  Guid GetTPObjectType(string techTypeName);

  /// <summary>
  /// Получение идентификатора типа объекта IPS по инфорамции доп. файла search
  /// </summary>
  /// <param name="versionId"></param>
  /// <param name="fileName">имя файла/блоба</param>
  /// <param name="masterDocId"></param>
  /// <param name="objectCaption"></param>
  /// <param name="isBaseVersion"></param>
  /// <returns>Если Guid.Empty - объект качать не нужно, в противном случае - Guid типа объекта IPS</returns>
  Guid GetDraftObjectType(
    int masterDocId,
    int versionId,
    string fileName,
    out string objectCaption,
    out bool isBaseVersion);

  /// <summary>
  /// Получение списка ид. типов атрибутов для технологических объектов, которые следует не перекачивать сторонним памперам
  /// </summary>
  /// <param name="objTypeId">Идентификатор типа объекта</param>
  /// <remarks>Пока только для техпроцессов.
  /// Метод вызывать только после перекачки метаданных - в противном случае возвращается null</remarks>
  /// <returns></returns>
  IEnumerable<int> GetAttributes2Exclude(int objTypeId);
}
