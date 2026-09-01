// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PhysicalParameterHelper
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin;

internal sealed class PhysicalParameterHelper : ParametersHelper<IParameter>
{
  private static readonly PhysicalParameterHelper Instance = new PhysicalParameterHelper();

  public static Parameter GetParameter(IParameter parameter)
  {
    return PhysicalParameterHelper.Instance.GetParameter(parameter.DM_Name(), typeof (string), parameter.DM_Value(), false);
  }

  public static Parameter[] ReadParameters(IComponent container)
  {
    List<Parameter> parameterList = new List<Parameter>();
    for (int argIndex = 0; argIndex < container.DM_ParameterCount(); ++argIndex)
    {
      IParameter parameter = container.DM_Parameters(argIndex);
      parameterList.Add(PhysicalParameterHelper.GetParameter(parameter));
    }
    return parameterList.ToArray();
  }
}
