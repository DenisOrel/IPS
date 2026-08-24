// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalObjectTypeNameTransform
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class PortalObjectTypeNameTransform : INodeColumnTransform
{
  public Type DataType => typeof (string);

  public object Apply(object sourceValue, NodeColumn column, object adapter, object[] allValues)
  {
    if (sourceValue is string)
      return sourceValue;
    IPortalMetadata service = (IPortalMetadata) ServicesManager.GetService(typeof (IPortalMetadata));
    return CellValue.GetValue(sourceValue, column, (object) service.GetPublishObjectTypeName(Convert.ToInt32(sourceValue)));
  }
}
