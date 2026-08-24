// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.GtcPropertiesTabCustom
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.PropertyGridInternal;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class GtcPropertiesTabCustom : PropertiesTab, IObjectPropertyGridTab
{
  private static Bitmap _bitmap;
  internal static readonly Guid PropertyTabGuid = new Guid("{9777C7C3-B761-46D9-BBB6-28F1E26660D7}");

  public override string TabName => ServiceHolder.Rm.GetString("GTC_29");

  public override Bitmap Bitmap
  {
    get
    {
      string resource = typeof (PropertiesTab).Name + ".bmp";
      return GtcPropertiesTabCustom._bitmap ?? (GtcPropertiesTabCustom._bitmap = new Bitmap(typeof (PropertiesTab), resource));
    }
  }

  public PropertyDescriptorCollection PropDescriptorCollection(object component)
  {
    return component is IGtcObjectPropDescriptorHolder ? ((IGtcObjectPropDescriptorHolder) component).PropDescriptorCollection : (PropertyDescriptorCollection) null;
  }

  public Guid TabGuid => GtcPropertiesTabCustom.PropertyTabGuid;

  public GetAttributeValuesModes TabAttributeValuesModes => GetAttributeValuesModes.None;

  public void InitTab(GetAttributeValuesModes avm)
  {
  }
}
