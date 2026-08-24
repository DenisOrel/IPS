// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.PortalUserNameTransform
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class PortalUserNameTransform : INodeColumnTransform
{
  public Type DataType => typeof (string);

  public object Apply(object sourceValue, NodeColumn column, object adapter, object[] allValues)
  {
    string str = Convert.ToString(sourceValue);
    int num = str.IndexOf('\\');
    return num < 0 ? (object) str : (object) str.Substring(num + 1);
  }
}
