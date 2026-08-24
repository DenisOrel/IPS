// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishTypeUITypeEditor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

internal class PublishTypeUITypeEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    return (object) PublishTypeSelectForm.SelectType((IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata)), value != null ? ((PublishTypeAttProxy) value).Guid : Guid.Empty) ?? value;
  }
}
