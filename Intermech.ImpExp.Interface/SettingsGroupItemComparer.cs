// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SettingsGroupItemComparer
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface;

internal class SettingsGroupItemComparer : IComparer<ISettingsGroupItem>
{
  public int Compare(ISettingsGroupItem x, ISettingsGroupItem y) => x.Caption.CompareTo(y.Caption);
}
