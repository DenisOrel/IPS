// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.PhysicalValuesTypeConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class PhysicalValuesTypeConverter : StringConverter
{
  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    string[] values = new string[EntityDescriptor.GetPhisicalValueList().Count];
    int num = 0;
    foreach (string str in EntityDescriptor.GetPhisicalValueList().Values)
      values[num++] = str;
    return new TypeConverter.StandardValuesCollection((ICollection) values);
  }
}
