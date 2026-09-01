// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.ProjectParametersHelper
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;

#nullable disable
namespace CSharpPlugin;

internal sealed class ProjectParametersHelper : ParametersHelper<IParameter>
{
  public static Parameter GetParameter(IParameter parameter)
  {
    return new ProjectParametersHelper().GetParameter(parameter.DM_Name(), typeof (string), parameter.DM_Value(), false);
  }
}
