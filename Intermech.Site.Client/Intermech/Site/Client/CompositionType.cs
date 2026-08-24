// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.CompositionType
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Site.Client;

[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Site.Client_15")]
[Category("Misc")]
internal enum CompositionType
{
  [CustomDescription("Attribute.Site.Client_16")] Full,
  [CustomDescription("Attribute.Site.Client_17")] First,
  [CustomDescription("Attribute.Site.Client_18")] None,
}
