// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PublishedUserObligatoryColumnScheme
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal sealed class PublishedUserObligatoryColumnScheme : PublishedObjectObligatoryColumnScheme
{
  private bool _transformsCreated;

  public override string Name => LocalizationHolder.rm.GetString("Site.Client_42");

  protected override void CreateTransforms()
  {
    base.CreateTransforms();
    if (this._transformsCreated)
      return;
    PortalUserNameTransform userNameTransform = new PortalUserNameTransform();
    this.transforms.Add((object) new Guid("cad0001d-306c-11d8-b4e9-00304f19f545"), (object) userNameTransform);
    this.transforms.Add((object) new Guid("cad00018-306c-11d8-b4e9-00304f19f545"), (object) userNameTransform);
    this._transformsCreated = true;
  }
}
