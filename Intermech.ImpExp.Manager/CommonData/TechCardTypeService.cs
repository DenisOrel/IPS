// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.TechCardTypeService
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Techcard;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData;

internal class TechCardTypeService : ITechCardTypeService
{
  private Dictionary<int, List<int>> _objectTypeAttributes2Exclude;

  public Guid GetDraftObjectType(
    int masterDocId,
    int versionId,
    string fileName,
    out string objectCaption,
    out bool isBaseVersion)
  {
    objectCaption = "Слайд 1";
    isBaseVersion = true;
    if (string.IsNullOrEmpty(fileName))
      return Guid.Empty;
    string[] strArray = fileName.ToUpper().Split('.');
    return strArray.Length < 2 || !(strArray[strArray.Length - 1] == "DWG") ? Guid.Empty : TechCardConsts.otDraftCadmechTGuid;
  }

  public Guid GetTPObjectType(string techTypeName)
  {
    if (((IEnumerable<string>) new string[2]
    {
      "Техпроцесс",
      "Техпроцесс единичный"
    }).Contains<string>(techTypeName, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))
      return TechCardConsts.otTpOneGuid;
    if (((IEnumerable<string>) new string[2]
    {
      "Типовой техпроцесс",
      "Техпроцесс типовой"
    }).Contains<string>(techTypeName, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))
      return TechCardConsts.otTPTypeGuid;
    if (techTypeName.Equals("Единичный техпроцесс по типовому", StringComparison.OrdinalIgnoreCase))
      return Guid.Empty;
    if (((IEnumerable<string>) new string[2]
    {
      "Групповой техпроцесс",
      "Техпроцесс групповой"
    }).Contains<string>(techTypeName, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))
      return TechCardConsts.otTpGroupGuid;
    if (techTypeName.Equals("Расцеховочный маршрут", StringComparison.OrdinalIgnoreCase))
      return TechCardConsts.otRouteObjGuid;
    if (techTypeName.Equals("Комплект технологических документов", StringComparison.OrdinalIgnoreCase) || techTypeName.Equals("Перевод техпроцесса", StringComparison.OrdinalIgnoreCase) || techTypeName.Equals("Перевод типового техпроцесса", StringComparison.OrdinalIgnoreCase) || techTypeName.Equals("Перевод группового техпроцесса", StringComparison.OrdinalIgnoreCase) || techTypeName.Equals("Комплект ведомостей", StringComparison.OrdinalIgnoreCase))
      return Guid.Empty;
    techTypeName.Equals("Заказ", StringComparison.OrdinalIgnoreCase);
    return Guid.Empty;
  }

  public IEnumerable<int> GetAttributes2Exclude(int objTypeId)
  {
    if (this._objectTypeAttributes2Exclude != null)
    {
      List<int> attributes2Exclude;
      if (!this._objectTypeAttributes2Exclude.TryGetValue(objTypeId, out attributes2Exclude))
        attributes2Exclude = new List<int>();
      return (IEnumerable<int>) attributes2Exclude;
    }
    IImportingData importingData1;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      importingData1 = (IImportingData) null;
    else
      importingData1 = service.GetCache(ImportingCategory.TechAttributes2Exclude);
    IImportingData importingData2 = importingData1;
    try
    {
      if (importingData2?.GetTag(ImportingCategory.TechAttributes2Exclude, (object) 1) is TechObjectTag tag)
        this._objectTypeAttributes2Exclude = tag.Object as Dictionary<int, List<int>>;
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.TechAttributes2Exclude);
    }
    return this._objectTypeAttributes2Exclude == null ? (IEnumerable<int>) new List<int>() : this.GetAttributes2Exclude(objTypeId);
  }
}
