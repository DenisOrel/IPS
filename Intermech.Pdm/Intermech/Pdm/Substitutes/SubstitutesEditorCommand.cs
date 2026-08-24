// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Substitutes.SubstitutesEditorCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Pdm.Substitutes;

[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Pdm_1")]
[Category("Misc")]
internal enum SubstitutesEditorCommand
{
  [CustomDescription("Attribute.Pdm_2")] CreateGroup,
  [CustomDescription("Attribute.Pdm_3")] ActualizeSubstitute,
  [CustomDescription("Attribute.Pdm_4")] EditSubstitutes,
  [CustomDescription("Attribute.Pdm_5")] DeleteSubstitutes,
}
