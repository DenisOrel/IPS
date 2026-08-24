// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishTypeAttEditor
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

internal class PublishTypeAttEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    Guid selectedType = Guid.Empty;
    IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    switch (value)
    {
      case PublishTypeAttProxy _:
        selectedType = ((PublishTypeAttProxy) value).Guid;
        break;
      case int typeID:
        PortalObjectType publishObjectType = service.GetPublishObjectType(typeID);
        if (publishObjectType != null)
        {
          selectedType = new Guid(publishObjectType.GUID);
          break;
        }
        break;
    }
    return (object) PublishTypeSelectForm.SelectType(service, selectedType) ?? value;
  }
}
