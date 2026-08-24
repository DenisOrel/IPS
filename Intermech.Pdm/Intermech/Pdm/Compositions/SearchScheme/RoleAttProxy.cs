// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.SearchScheme.RoleAttProxy
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions.SearchScheme;

internal sealed class RoleAttProxy
{
  private string _name;

  public RoleAttProxy(Guid guid)
    : this(guid, string.Empty)
  {
  }

  public RoleAttProxy(Guid guid, string name)
  {
    this.Guid = guid;
    this._name = name;
  }

  public override string ToString()
  {
    if (this.Guid == Guid.Empty)
      return LocalizationHolder.rm.GetString("Client.Core_929");
    if (this._name.Length == 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        this._name = sessionKeeper.Session.GetObjectInfo(this.Guid).Caption;
    }
    return this._name;
  }

  public Guid Guid { get; }
}
