// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Services.PublicationService
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Services;

internal class PublicationService : IPublicationService
{
  public bool PublishWithDialog(ISelectedItems items)
  {
    return UnitedPublishForm.ShowForm(items) == DialogResult.OK;
  }

  public bool PublishWithDialog(List<Tuple<long, int>> items)
  {
    return UnitedPublishForm.ShowForm(items) == DialogResult.OK;
  }

  public bool ShowPublishOptions(List<Tuple<long, int>> items, ExtendedPublishOptions options)
  {
    return UnitedPublishForm.ShowForm(items, options) == DialogResult.OK;
  }
}
