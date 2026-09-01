// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Variant
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

internal sealed class Variant(IProjectVariant proxy) : 
  Parametrable<IProjectVariant>(proxy),
  IVariant,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IDisposable
{
  public string Description => this.parametrableObject.DM_Description();

  protected override Parameter[] GetParameters()
  {
    List<Parameter> parameterList = new List<Parameter>();
    for (int argIndex = 0; argIndex < this.parametrableObject.DM_ParameterCount(); ++argIndex)
    {
      Parameter parameter = VariantParametersHelper.GetParameter(this.parametrableObject.DM_Parameters(argIndex));
      parameterList.Add(parameter);
    }
    return parameterList.ToArray();
  }

  public override string InternalId => this.Description;

  protected override void WriteNewParameter(Parameter parameter)
  {
    this.parametrableObject.DM_AddParameter(parameter.Name, Convert.ToString(parameter.Value));
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    for (int argIndex = 0; argIndex < this.parametrableObject.DM_ParameterCount(); ++argIndex)
    {
      IParameter parameter1 = this.parametrableObject.DM_Parameters(argIndex);
      if (parameter1.DM_Name().Equals(parameter.Name))
      {
        parameter1.DM_SetValue(Convert.ToString(parameter.Value));
        break;
      }
    }
  }

  public List<IVariation> Variations
  {
    get
    {
      List<IVariation> variations = new List<IVariation>();
      for (int argIndex = 0; argIndex < this.parametrableObject.DM_VariationCount(); ++argIndex)
        variations.Add((IVariation) new Variation(this.parametrableObject.DM_Variations(argIndex)));
      return variations;
    }
  }
}
