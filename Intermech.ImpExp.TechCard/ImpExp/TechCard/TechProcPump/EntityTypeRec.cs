// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.EntityTypeRec
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[Serializable]
internal class EntityTypeRec
{
  private readonly Dictionary<string, Entity> _codeList;
  private readonly Dictionary<string, Dictionary<int, Entity>> _dopTypeList;

  public EntityTypeRec()
  {
    this._codeList = new Dictionary<string, Entity>();
    this._dopTypeList = new Dictionary<string, Dictionary<int, Entity>>();
  }

  public virtual void AddEntity(Entity entity)
  {
    if (entity == null || this.CodeList.ContainsKey(entity.Code))
      return;
    Entity entity1 = entity.Clone();
    this.CodeList.Add(entity1.Code, entity1);
    if (entity1.Tag <= 0)
      return;
    Dictionary<int, Entity> entListByDopType = this.GetEntListByDopType(EntityTypeRec.GetDopTypeByEntType(entity1.Type));
    if (entListByDopType.ContainsKey(entity1.Tag))
      return;
    entListByDopType.Add(entity1.Tag, entity1);
  }

  public virtual Dictionary<int, Entity> GetEntListByDopType(string dopType)
  {
    Dictionary<int, Entity> entListByDopType;
    if (!this.DopTypeList.TryGetValue(dopType, out entListByDopType))
    {
      entListByDopType = new Dictionary<int, Entity>();
      this.DopTypeList.Add(dopType, entListByDopType);
    }
    return entListByDopType;
  }

  public Dictionary<string, Entity> CodeList => this._codeList;

  public Dictionary<string, Dictionary<int, Entity>> DopTypeList => this._dopTypeList;

  public static string GetDopTypeByEntType(string entType)
  {
    string dopTypeByEntType = string.Empty;
    switch (entType)
    {
      case "B":
        dopTypeByEntType = "I";
        break;
      case "D":
        dopTypeByEntType = "R";
        break;
      case "E":
        dopTypeByEntType = "S";
        break;
      case "I":
        dopTypeByEntType = "I";
        break;
      case "K":
        dopTypeByEntType = "S";
        break;
      case "P":
        dopTypeByEntType = "I";
        break;
      case "R":
        dopTypeByEntType = "F";
        break;
      case "S":
        dopTypeByEntType = "S";
        break;
      default:
        string Message = "Методу GetEntTypeByDopType не известен тип записи: " + dopTypeByEntType;
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
        break;
    }
    return dopTypeByEntType;
  }
}
