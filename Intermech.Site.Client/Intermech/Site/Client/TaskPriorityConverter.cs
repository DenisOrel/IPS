// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.TaskPriorityConverter
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.Site.Client;

public class TaskPriorityConverter : TypeConverter
{
  private static TypeConverter.StandardValuesCollection _values;

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
  }

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (TaskPriority) || base.CanConvertTo(context, destinationType);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    object obj;
    return (obj = value) is TaskPriority ? (object) EnumDescConverter.GetEnumDescription((Enum) (TaskPriority) obj) : base.ConvertTo(context, culture, value, destinationType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    if (value is string str)
    {
      foreach (TaskPriority taskPriority in Enum.GetValues(typeof (TaskPriority)))
      {
        if (EnumDescConverter.GetEnumDescription((Enum) taskPriority).Equals(str))
          return (object) taskPriority;
      }
    }
    return base.ConvertFrom(context, culture, value);
  }

  public override TypeConverter.StandardValuesCollection GetStandardValues(
    ITypeDescriptorContext context)
  {
    if (TaskPriorityConverter._values == null)
      TaskPriorityConverter._values = new TypeConverter.StandardValuesCollection((ICollection) Enum.GetValues(typeof (TaskPriority)));
    return TaskPriorityConverter._values;
  }

  public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

  public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
}
