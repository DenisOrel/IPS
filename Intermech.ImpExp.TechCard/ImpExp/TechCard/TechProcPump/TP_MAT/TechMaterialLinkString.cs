// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_MAT.TechMaterialLinkString
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_MAT;

[Serializable]
internal class TechMaterialLinkString
{
  private readonly int _parentType;
  private readonly int _parentKey;
  private readonly int _childType;
  private readonly int _childKey;

  public TechMaterialLinkString(int parentType, int parentKey, int childType, int childKey)
  {
    this._parentType = parentType;
    this._parentKey = parentKey;
    this._childType = childType;
    this._childKey = childKey;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (!(obj is TechMaterialLinkString materialLinkString))
      return this == obj;
    return materialLinkString.ChildKey == this.ChildKey && materialLinkString.ChildType == this.ChildType && materialLinkString.ParentKey == this.ParentKey && materialLinkString.ParentType == this.ParentType;
  }

  public override int GetHashCode()
  {
    int num1 = this.ChildKey;
    int hashCode1 = num1.GetHashCode();
    num1 = this.ChildType;
    int hashCode2 = num1.GetHashCode();
    int num2 = hashCode1 ^ hashCode2;
    num1 = this.ParentKey;
    int hashCode3 = num1.GetHashCode();
    int num3 = num2 ^ hashCode3;
    num1 = this.ParentType;
    int hashCode4 = num1.GetHashCode();
    return num3 ^ hashCode4;
  }

  public int ParentType => this._parentType;

  public int ParentKey => this._parentKey;

  public int ChildType => this._childType;

  public int ChildKey => this._childKey;
}
