// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ComboPriority
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ComboPriority
{
  public TaskPriority Priority;

  public ComboPriority(TaskPriority priority) => this.Priority = priority;

  public override string ToString() => EnumDescConverter.GetEnumDescription((Enum) this.Priority);
}
