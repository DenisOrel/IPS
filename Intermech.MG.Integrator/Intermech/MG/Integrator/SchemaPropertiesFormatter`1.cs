// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.SchemaPropertiesFormatter`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class SchemaPropertiesFormatter<TComponent> : OpenMetadataValueBagFormatter where TComponent : IElectricalComponent
{
  public override bool IsContainerSupported(IValueBagContainer container)
  {
    return container is TComponent;
  }

  private TComponent GetSchemaComponent(IValueBagContainer container) => (TComponent) container;

  protected override ValueBag DoRead(IValueBagContainer container, ICollection<StringKey> valueKeys)
  {
    TComponent schemaComponent = this.GetSchemaComponent(container);
    ValueBag valueBag = new ValueBag(valueKeys.Count);
    foreach (StringKey valueKey in (IEnumerable<StringKey>) valueKeys)
    {
      string str = Convert.ToString(schemaComponent.GetPropertyValue((string) valueKey));
      if (!string.IsNullOrEmpty(str))
        valueBag.Add(valueKey, (object) str);
    }
    return valueBag;
  }

  protected override void DoWrite(
    IValueBagContainer container,
    ContainerValues values,
    ICollection<StringKey> changedValues)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    if (values == null)
      throw new ArgumentNullException(nameof (values));
    if (changedValues == null)
      throw new ArgumentNullException(nameof (changedValues));
    TComponent schemaComponent = this.GetSchemaComponent(container);
    foreach (ValueRecord valueRecord in values.Bag.FindAll((Predicate<ValueRecord>) (record => changedValues.Contains(record.Key))))
    {
      if (valueRecord.DataType == typeof (string))
        schemaComponent.SetPropertyValue((string) valueRecord.Key, (object) valueRecord.Read<string>(string.Empty));
    }
  }
}
