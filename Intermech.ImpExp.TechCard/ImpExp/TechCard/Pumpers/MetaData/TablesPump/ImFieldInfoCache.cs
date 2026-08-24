// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImFieldInfoCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[Serializable]
internal class ImFieldInfoCache
{
  private readonly Dictionary<int, ImFieldInfo> _fieldKey2FieldInfo = new Dictionary<int, ImFieldInfo>();

  public virtual void Add(ImFieldInfo fieldInfo)
  {
    this._fieldKey2FieldInfo[fieldInfo.Key] = fieldInfo;
  }

  public IEnumerable<ImFieldInfo> GetAllFieldInfo()
  {
    return (IEnumerable<ImFieldInfo>) this._fieldKey2FieldInfo.Values;
  }

  public ImFieldInfo GetFieldInfo(int fieldKey)
  {
    ImFieldInfo fieldInfo;
    if (this._fieldKey2FieldInfo.TryGetValue(fieldKey, out fieldInfo))
      return fieldInfo;
    string Message = $"Поле с идентификатором {fieldKey.ToString()} не найдена в кэше ImFieldInfoCache";
    TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    return (ImFieldInfo) null;
  }

  public string GetFieldName(int fieldKey)
  {
    ImFieldInfo fieldInfo = this.GetFieldInfo(fieldKey);
    return fieldInfo != null ? fieldInfo.Field : string.Empty;
  }

  public Guid GetIpsImAttrGuid(int fieldKey)
  {
    ImFieldInfo fieldInfo = this.GetFieldInfo(fieldKey);
    return fieldInfo != null ? fieldInfo.IpsAttrTypeGuid : Guid.Empty;
  }
}
