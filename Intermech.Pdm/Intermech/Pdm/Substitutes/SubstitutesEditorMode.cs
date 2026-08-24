// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Substitutes.SubstitutesEditorMode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Pdm.Substitutes;

[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Pdm_6")]
[Category("Misc")]
internal enum SubstitutesEditorMode
{
  [CustomDescription("Attribute.Pdm_7")] AdminMode,
  [CustomDescription("Attribute.Pdm_8")] UserMode,
  [CustomDescription("Attribute.Pdm_9")] ReadOnly,
}
