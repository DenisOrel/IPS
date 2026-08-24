// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.RelationTypeProrerties
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System.ComponentModel;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class RelationTypeProrerties
{
  public RelationTypeProrerties(RelationMigrateType migrateType) => this.MigrateType = migrateType;

  [DisplayName("Передача связей через портал")]
  [Description("Настройка передачи связей этого типа через портал. Настройка влияет на формирование передаваемого через портал состава.")]
  public RelationMigrateType MigrateType { get; set; }
}
