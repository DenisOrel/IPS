// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.SiteInfoItem
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal class SiteInfoItem : SiteInfo
{
  public SiteInfoItem()
  {
  }

  public SiteInfoItem(long id, Guid guid, char code, string caption, SystemTypes systemType)
    : base(id, guid, code, caption, systemType)
  {
  }

  public static SiteInfoItem NewItem(SiteInfo owner)
  {
    return new SiteInfoItem(owner.ID, owner.GUID, owner.Code, owner.Caption, owner.SystemType);
  }

  public override string ToString()
  {
    return $"{(ValueType) '«'}{this.Code}{(ValueType) '»'} ({this.Caption})";
  }
}
