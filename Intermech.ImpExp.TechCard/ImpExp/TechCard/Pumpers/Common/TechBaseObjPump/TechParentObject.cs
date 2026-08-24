// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechParentObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal class TechParentObject
{
  protected long _objectVerId;

  public static void AddParametor2ThisTpObj(
    long objVerId,
    TechParentObject.TechParamType paramType,
    IImportingData masterImport,
    object paramValue,
    Type paramValueType)
  {
    try
    {
      ParentObjectParam parentObjectParam = new ParentObjectParam()
      {
        ParamType = paramType,
        ParamValue = paramValue
      };
      object obj = masterImport.GetTag(ImportingCategory.TechParentParametors, (object) objVerId) is TechObjectTag tag1 ? tag1.Object : (object) null;
      if (obj == null)
      {
        List<ParentObjectParam> techObject = new List<ParentObjectParam>();
        TechObjectTag tag2 = new TechObjectTag((object) techObject);
        techObject.Add(parentObjectParam);
        masterImport.AddValue(ImportingCategory.TechParentParametors, (object) objVerId, objVerId, (ITagImportObject) tag2);
      }
      else
      {
        if (!(obj is List<ParentObjectParam> parentObjectParamList))
          return;
        parentObjectParamList.Add(parentObjectParam);
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка добавления/обновления значения кэша {(Enum) ImportingCategory.TechParentParametors} для объекта {objVerId}: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  public object GetParametorByName(
    TechParentObject.TechParamType paramType,
    IImportingData masterImport)
  {
    try
    {
      if (!(masterImport.GetTag(ImportingCategory.TechParentParametors, (object) this._objectVerId) is TechObjectTag tag) || !(tag.Object is List<ParentObjectParam> parentObjectParamList))
        return (object) null;
      foreach (ParentObjectParam parentObjectParam in parentObjectParamList)
      {
        if (parentObjectParam.ParamType == paramType)
          return parentObjectParam.ParamValue;
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage(string.Format("Ошибка получения параметра \"{0}\" дерева ТП объекта {2}: {1}", (object) paramType, (object) ex.Message, (object) this._objectVerId));
      if (ex is OutOfMemoryException)
        throw;
    }
    return (object) null;
  }

  public TechParentObject(long parentObjectVerId) => this._objectVerId = parentObjectVerId;

  public long ObjectVerId
  {
    get => this._objectVerId;
    set => this._objectVerId = value;
  }

  public enum TechParamType
  {
    LcStep,
    Production,
  }
}
