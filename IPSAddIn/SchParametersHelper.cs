// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SchParametersHelper
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using SCH;
using System;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin;

internal sealed class SchParametersHelper : ParametersHelper<ISch_Parameter>
{
  public static Parameter GetParameter(ISch_Parameter parameter)
  {
    SchParametersHelper parametersHelper = new SchParametersHelper();
    return parametersHelper.GetParameter(parameter.GetState_Name(), parametersHelper.GetParameterType(parameter.GetState_ParamType()), parameter.GetState_Text(), parameter.GetState_ValueIsReadOnly());
  }

  private Type GetParameterType(TParameterType adType)
  {
    switch (adType)
    {
      case TParameterType.eParameterType_Boolean:
        return typeof (bool);
      case TParameterType.eParameterType_Integer:
        return typeof (long);
      case TParameterType.eParameterType_Float:
        return typeof (double);
      default:
        return typeof (string);
    }
  }

  public static Parameter[] ReadParameters(ISch_BasicContainer container)
  {
    return SchParametersHelper.ReadParameters(container, (string[]) null);
  }

  public static Parameter[] ReadParameters(ISch_BasicContainer container, string[] filter)
  {
    List<Parameter> parameterList = new List<Parameter>();
    ISch_Iterator parameterIterator = SchParametersHelper.GetParameterIterator(container);
    for (ISch_Parameter parameter = parameterIterator.FirstSchObject() as ISch_Parameter; parameter != null; parameter = parameterIterator.NextSchObject() as ISch_Parameter)
    {
      string name = parameter.GetState_Name();
      if (filter == null || Array.Exists<string>(filter, (Predicate<string>) (item => item == name)))
        parameterList.Add(SchParametersHelper.GetParameter(parameter));
    }
    container.SchIterator_Destroy(ref parameterIterator);
    return parameterList.ToArray();
  }

  public static void WriteNewParameter(ISch_BasicContainer container, Parameter parameter)
  {
    ISch_Parameter argObject = Helper.SCHServer.SchObjectFactory(TObjectId.eParameter, TObjectCreationMode.eCreate_Default) as ISch_Parameter;
    argObject.SetState_Name(parameter.Name);
    argObject.SetState_Text(Convert.ToString(parameter.Value));
    argObject.SetState_IsHidden(true);
    container.AddSchObject((object) argObject);
  }

  public static void WriteParameterValue(ISch_BasicContainer container, Parameter parameter)
  {
    ISch_Iterator parameterIterator = SchParametersHelper.GetParameterIterator(container);
    for (ISch_Parameter schParameter = parameterIterator.FirstSchObject() as ISch_Parameter; schParameter != null; schParameter = parameterIterator.NextSchObject() as ISch_Parameter)
    {
      if (schParameter.GetState_Name() == parameter.Name)
      {
        schParameter.SetState_Text(Convert.ToString(parameter.Value));
        break;
      }
    }
    container.SchIterator_Destroy(ref parameterIterator);
  }

  private static ISch_Iterator GetParameterIterator(ISch_BasicContainer container)
  {
    ISch_Iterator sch_iteratorIntf = container.SchIterator_Create();
    TObjectSet argObjectSet = new TObjectSet();
    argObjectSet.Add(TObjectId.eParameter);
    sch_iteratorIntf.AddFilter_ObjectSet(argObjectSet);
    sch_iteratorIntf.SetState_IterationDepth(TIterationDepth.eIterateFirstLevel);
    return sch_iteratorIntf;
  }
}
