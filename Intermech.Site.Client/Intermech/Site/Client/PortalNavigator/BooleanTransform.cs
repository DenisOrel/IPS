// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.BooleanTransform
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class BooleanTransform : INodeColumnTransform
{
  public Type DataType => typeof (string);

  public object Apply(object sourceValue, NodeColumn column, object adapter, object[] allValues)
  {
    if (sourceValue == null || sourceValue == DBNull.Value)
      return sourceValue;
    if (Convert.ToInt64(sourceValue) == 1L)
      return CellValue.GetValue(sourceValue, column, (object) Consts.YesValue);
    return Convert.ToInt64(sourceValue) == 0L ? CellValue.GetValue(sourceValue, column, (object) Consts.NoValue) : sourceValue;
  }
}
