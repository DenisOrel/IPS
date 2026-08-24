// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Attributes.DefaultConvertStrategyForTypeAttribute
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class DefaultConvertStrategyForTypeAttribute : Attribute
{
  public readonly string TypeName;

  public DefaultConvertStrategyForTypeAttribute(Type type) => this.TypeName = type.Name;
}
