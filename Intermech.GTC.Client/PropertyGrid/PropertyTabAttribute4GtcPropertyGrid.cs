// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.PropertyTabAttribute4GtcPropertyGrid
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using System;
using System.Collections;
using System.ComponentModel;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

internal class PropertyTabAttribute4GtcPropertyGrid : PropertyTabAttribute
{
  public PropertyTabAttribute4GtcPropertyGrid(Type[] tabTypes)
  {
    if (tabTypes == null)
      return;
    ArrayList arrayList1 = new ArrayList();
    ArrayList arrayList2 = new ArrayList();
    for (int index = 0; index < tabTypes.Length; ++index)
    {
      if (typeof (GtcObjectPropertyGridTab).IsAssignableFrom(tabTypes[index]))
        arrayList1.Add((object) tabTypes[index]);
    }
    for (int index = 0; index < arrayList1.Count; ++index)
      arrayList2.Add((object) PropertyTabScope.Component);
    this.InitializeArrays((Type[]) arrayList1.ToArray(typeof (Type)), (PropertyTabScope[]) arrayList2.ToArray(typeof (PropertyTabScope)));
  }
}
