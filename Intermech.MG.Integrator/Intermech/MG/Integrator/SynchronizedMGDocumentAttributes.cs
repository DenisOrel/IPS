// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.SynchronizedMGDocumentAttributes
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class SynchronizedMGDocumentAttributes : SynchronizedDocumentAttributes
{
  private readonly AttributeTable attrTable;

  public SynchronizedMGDocumentAttributes(MGSettingsService settingsService)
    : base((IIntegratorSettingsService) settingsService)
  {
    this.attrTable = new AttributeTable(settingsService, AttributeTable.TableKind.DocumentAttributes);
  }

  protected override ICollection<StringKey> GetUserDefinedAttributes()
  {
    ICollection<StringKey> definedAttributes = base.GetUserDefinedAttributes();
    if (this.attrTable.Rows != null)
    {
      foreach (Tuple<StringKey, StringKey, bool> row in this.attrTable.Rows)
        definedAttributes.Add(row.Item1);
    }
    return definedAttributes;
  }

  protected override ICollection<StringKey> GetVirtualAttributes()
  {
    ICollection<StringKey> virtualAttributes = base.GetVirtualAttributes();
    virtualAttributes.Add((StringKey) "Document type");
    virtualAttributes.Add((StringKey) "Document code");
    return virtualAttributes;
  }
}
