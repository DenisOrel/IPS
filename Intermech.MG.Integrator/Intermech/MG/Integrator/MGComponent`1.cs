// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGComponent`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Interfaces;
using Intermech.Tools.Data;
using Intermech.Tools.Integrators.Electrical;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal abstract class MGComponent<TComponent> : 
  MGObject<TComponent>,
  IElectricalComponent,
  IPropertiesCollection,
  IFunctionalGroupComponent,
  IImbaseComponent,
  IValueBagContainer
{
  protected MGIntegratorSettings integratorSettings;
  protected IDocumentFile parent;

  public MGComponent(TComponent component, MGIntegratorSettings integratorSettings)
    : base(component)
  {
    this.integratorSettings = integratorSettings;
  }

  public abstract string UID { get; }

  public abstract FunctionalGroup FunctionalGroup { get; set; }

  public object GetPropertyValue(string parameterName)
  {
    return CompoundHelper.isCompound(parameterName) ? (object) ElectricalComponentCompoundValue.HandleValue((IElectricalComponent) this, parameterName) : (object) this.InternalGetPropertyValue(parameterName);
  }

  protected abstract string InternalGetPropertyValue(string parameterName);

  public abstract void SetPropertyValue(string parameterName, object value);

  public abstract IComponentProperty GetProperty(string parameterName);

  public Guid PosGuid
  {
    get
    {
      string str = Convert.ToString(this.GetPropertyValue(ElectricalConsts.PosGuidAttribute));
      Guid empty = Guid.Empty;
      Guid posGuid;
      if (!string.IsNullOrEmpty(str))
      {
        Guid guid = GuidHelper.IsGuid(str) ? new Guid(str) : Guid.Empty;
        if (guid != Guid.Empty)
        {
          posGuid = guid;
        }
        else
        {
          posGuid = Guid.NewGuid();
          this.SetPropertyValue(ElectricalConsts.PosGuidAttribute, (object) posGuid.ToString());
        }
      }
      else
      {
        posGuid = Guid.NewGuid();
        this.SetPropertyValue(ElectricalConsts.PosGuidAttribute, (object) posGuid.ToString());
      }
      return posGuid;
    }
  }

  public bool ImbaseBinding()
  {
    string imbaseKey = Convert.ToString(this.GetPropertyValue(IDCache.Default.ImbaseKey.Text));
    string componentValue = this.GetComponentValue(this.integratorSettings, IDCache.Default.Name.Text);
    if (string.IsNullOrEmpty(componentValue))
      return false;
    string str = ImbaseSynchronizationHepler.ImbaseBinding(imbaseKey, componentValue);
    if (!string.IsNullOrEmpty(str) && !str.Equals(imbaseKey))
      this.SetPropertyValue(IDCache.Default.ImbaseKey.Text, (object) str);
    return true;
  }

  protected string GetComponentValue(MGIntegratorSettings integratorSettings, string attributeName)
  {
    Tuple<StringKey, StringKey, bool> tuple = integratorSettings.PartAttributesTable.Find((Predicate<Tuple<StringKey, StringKey, bool>>) (x => x.Item1.Equals(attributeName)));
    string parameterName = tuple != null ? tuple.Item2.ToString() : string.Empty;
    return !string.IsNullOrEmpty(parameterName) ? Convert.ToString(this.GetPropertyValue(parameterName)) : string.Empty;
  }

  public abstract string PartNumber { get; }

  public abstract string PosDesignation { get; }

  public IDocumentFile Parent
  {
    get => this.parent;
    set => this.parent = value;
  }

  public string ASPosDesignation
  {
    get
    {
      return !string.IsNullOrEmpty(this.integratorSettings.ASPosDesignation) ? Convert.ToString(this.GetPropertyValue(this.integratorSettings.ASPosDesignation)) : (string) null;
    }
  }
}
