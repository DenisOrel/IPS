// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGDocumentCodec`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGDocumentCodec<TComponent> : DocumentAttributesCodec where TComponent : IElectricalComponent
{
  private readonly AttributeTable attrTable;

  public MGDocumentCodec(MGSettingsService settingsSvc)
    : base((IValueBagFormatter) new SchemaPropertiesFormatter<TComponent>())
  {
    this.SaveDesignationSuffix = false;
    this.attrTable = new AttributeTable(settingsSvc, AttributeTable.TableKind.DocumentAttributes);
  }

  protected override StringKey GetContainerValueKey(StringKey attributeKey)
  {
    return this.attrTable.GetFormatterValueKey(attributeKey, base.GetContainerValueKey(attributeKey));
  }
}
