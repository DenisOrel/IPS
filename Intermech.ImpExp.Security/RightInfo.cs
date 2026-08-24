// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.RightInfo
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

#nullable disable
namespace Intermech.ImpExp.Security;

internal class RightInfo
{
  public readonly ActionType[] Types;
  public readonly int Category;

  public RightInfo(ActionType t, int cat)
    : this(new ActionType[1]{ t }, cat)
  {
  }

  public RightInfo(ActionType t)
    : this(t, 1)
  {
  }

  public RightInfo(ActionType[] types, int cat)
  {
    this.Types = types;
    this.Category = cat;
  }

  public RightInfo(ActionType[] types)
    : this(types, 1)
  {
  }
}
