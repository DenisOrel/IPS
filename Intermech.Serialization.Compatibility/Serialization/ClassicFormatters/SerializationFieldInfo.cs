// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SerializationFieldInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class SerializationFieldInfo : FieldInfo
{
  private readonly FieldInfo m_field;
  private readonly string m_serializationName;

  internal SerializationFieldInfo(FieldInfo field, string namePrefix)
  {
    this.m_field = field;
    this.m_serializationName = $"{namePrefix}+{this.m_field.Name}";
  }

  internal FieldInfo FieldInfo => this.m_field;

  public override string Name => this.m_serializationName;

  public override Module Module => this.m_field.Module;

  public override int MetadataToken => this.m_field.MetadataToken;

  public override Type DeclaringType => this.m_field.DeclaringType;

  public override Type ReflectedType => this.m_field.ReflectedType;

  public override object[] GetCustomAttributes(bool inherit)
  {
    return this.m_field.GetCustomAttributes(inherit);
  }

  public override object[] GetCustomAttributes(Type attributeType, bool inherit)
  {
    return this.m_field.GetCustomAttributes(attributeType, inherit);
  }

  public override bool IsDefined(Type attributeType, bool inherit)
  {
    return this.m_field.IsDefined(attributeType, inherit);
  }

  public override Type FieldType => this.m_field.FieldType;

  public override object GetValue(object obj) => this.m_field.GetValue(obj);

  public override void SetValue(
    object obj,
    object value,
    BindingFlags invokeAttr,
    Binder binder,
    CultureInfo culture)
  {
    this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
  }

  public override RuntimeFieldHandle FieldHandle => this.m_field.FieldHandle;

  public override FieldAttributes Attributes => this.m_field.Attributes;
}
