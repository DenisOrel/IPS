// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Variation
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using System;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin;

internal sealed class Variation(IComponentVariation component) : 
  Parametrable<IComponentVariation>(component),
  IVariation,
  ISchComponent,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IDisposable
{
  public int VariationKind => (int) this.parametrableObject.DM_VariationKind();

  public string DesignatorText => this.parametrableObject.DM_PhysicalDesignator();

  public int VariationCount => this.parametrableObject.DM_VariationCount();

  public string AlternatePart => this.parametrableObject.DM_AlternatePart();

  protected override Parameter[] GetParameters()
  {
    List<Parameter> parameterList = new List<Parameter>();
    for (int argIndex = 0; argIndex < this.parametrableObject.DM_VariationCount(); ++argIndex)
    {
      IParameterVariation parameterVariation = this.parametrableObject.DM_Variations(argIndex);
      parameterList.Add(new Parameter(parameterVariation.DM_ParameterName(), (object) parameterVariation.DM_VariedValue(), false, typeof (string)));
    }
    return parameterList.ToArray();
  }

  public override string InternalId => this.parametrableObject.DM_UniqueId();

  protected override void WriteNewParameter(Parameter parameter)
  {
    IParameterVariation parameterVariation = this.parametrableObject.DM_AddParameterVariation();
    parameterVariation.DM_SetParameterName(parameter.Name);
    parameterVariation.DM_SetVariedValue(Convert.ToString(parameter.Value));
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    this.parametrableObject.DM_FindParameterVariation(parameter.Name)?.DM_SetVariedValue(Convert.ToString(parameter.Value));
  }
}
