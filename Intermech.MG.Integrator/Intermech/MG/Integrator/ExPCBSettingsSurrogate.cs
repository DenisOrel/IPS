// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBSettingsSurrogate
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.PropertyEditors.ChangeHighlighting;
using Intermech.Tools.Integrators.Electrical;
using System.ComponentModel;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBSettingsSurrogate(MGIntegratorSettings settings) : MGSettingsSurrogate(settings)
{
  [Browsable(false)]
  public new ChangeTrackingListAdapter<AttributeTableItemSurrogate> AssemblyAttributesTable
  {
    get => (ChangeTrackingListAdapter<AttributeTableItemSurrogate>) null;
    set
    {
    }
  }

  [Browsable(false)]
  public new ChangeTrackingListAdapter<AttributeTableItemSurrogate> DocumentAttributesTable
  {
    get => (ChangeTrackingListAdapter<AttributeTableItemSurrogate>) null;
    set
    {
    }
  }

  [Browsable(false)]
  public new string PartPostDesignationAttribute
  {
    get => (string) null;
    set
    {
    }
  }

  [Browsable(false)]
  public string PartNameAttribute
  {
    get => (string) null;
    set
    {
    }
  }

  public override object Clone() => (object) new ExPCBSettingsSurrogate(this.Settings);
}
