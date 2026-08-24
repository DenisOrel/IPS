// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ErrorState
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.PdmConfigurator;

internal enum ErrorState
{
  [Description("")] None,
  [CustomDescription("Attribute.PdmConfigurator_1")] Option,
  [CustomDescription("Attribute.PdmConfigurator_2")] Value,
  [CustomDescription("Attribute.PdmConfigurator_3")] IncompConflict,
  [CustomDescription("Attribute.PdmConfigurator_4")] LinkedConflict,
  [CustomDescription("Attribute.PdmConfigurator_5")] EmptyField,
  [CustomDescription("Attribute.PdmConfigurator_6")] ObsoleteOption,
  [CustomDescription("Attribute.PdmConfigurator_7")] ObsoleteOptionValue,
}
