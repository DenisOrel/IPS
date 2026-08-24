// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.PdmCategoryOptionsNodeID
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Navigator.Interfaces;
using System.Diagnostics;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class PdmCategoryOptionsNodeID : INodeID
{
  internal long _pdmCategoryID;
  private object _cookie;

  public PdmCategoryOptionsNodeID()
  {
  }

  public PdmCategoryOptionsNodeID(long categoryID) => this._pdmCategoryID = categoryID;

  public override bool Equals(object obj)
  {
    return obj is PdmCategoryOptionsNodeID categoryOptionsNodeId && this._pdmCategoryID == categoryOptionsNodeId._pdmCategoryID;
  }

  public override int GetHashCode() => this._pdmCategoryID.GetHashCode();

  public int CategoryID
  {
    [DebuggerStepThrough] get => Intermech.PdmConfigurator.PdmConfigurator.CategoryAllCategoryOptionsNode;
  }

  public int TypeID
  {
    [DebuggerStepThrough] get => 0;
  }

  public object Cookie
  {
    [DebuggerStepThrough] get => this._cookie;
    [DebuggerStepThrough] set => this._cookie = value;
  }
}
