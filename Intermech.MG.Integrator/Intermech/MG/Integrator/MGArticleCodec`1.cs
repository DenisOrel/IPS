// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGArticleCodec`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Integrators.Mechanical;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGArticleCodec<TComponent> : ArticleAttributesCodec where TComponent : IElectricalComponent
{
  private readonly AttributeTable attrTable;

  public MGArticleCodec(MGSettingsService settingsSvc, bool isAssemblyCodec)
    : base((IValueBagFormatter) new SchemaPropertiesFormatter<TComponent>())
  {
    this.attrTable = new AttributeTable(settingsSvc, isAssemblyCodec ? AttributeTable.TableKind.AssemblyAttributes : AttributeTable.TableKind.PartAttributes);
  }

  protected override StringKey GetContainerValueKey(StringKey attributeKey)
  {
    return this.attrTable.GetFormatterValueKey(attributeKey, base.GetContainerValueKey(attributeKey));
  }
}
