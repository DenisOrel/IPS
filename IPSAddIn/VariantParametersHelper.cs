// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.VariantParametersHelper
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using System;

#nullable disable
namespace CSharpPlugin;

internal sealed class VariantParametersHelper : ParametersHelper<IParameter>
{
  public static Parameter GetParameter(IParameter parameter)
  {
    VariantParametersHelper parametersHelper = new VariantParametersHelper();
    return parametersHelper.GetParameter(parameter.DM_Name(), parametersHelper.GetParameterType(parameter.DM_Kind()), parameter.DM_Value(), false);
  }

  private Type GetParameterType(TParameterKind adType)
  {
    switch (adType)
    {
      case TParameterKind.eParameterKind_Boolean:
        return typeof (bool);
      case TParameterKind.eParameterKind_Integer:
        return typeof (long);
      case TParameterKind.eParameterKind_Float:
        return typeof (double);
      default:
        return typeof (string);
    }
  }
}
