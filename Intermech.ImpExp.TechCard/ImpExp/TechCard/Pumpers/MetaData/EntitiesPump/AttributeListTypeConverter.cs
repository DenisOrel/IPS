// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.AttributeListTypeConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class AttributeListTypeConverter : StringConverter
{
  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    return context.Instance is EntityDescriptor instance ? new TypeConverter.StandardValuesCollection((ICollection) EntityHelper.GetPosibleTypes(instance.Entity).Select<FieldTypes, string>((Func<FieldTypes, string>) (type => EnumDescConverter.GetEnumDescription((Enum) type))).ToArray<string>()) : base.GetStandardValues(context);
  }
}
