// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.ParametersHelper`1
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using System;

#nullable disable
namespace CSharpPlugin;

internal abstract class ParametersHelper<TParameter>
{
  protected Parameter GetParameter(string name, Type paramType, string strValue, bool readOnly)
  {
    IObligatoryParameterHandler entity = (IObligatoryParameterHandler) null;
    switch (name)
    {
      case "DocumentNumber":
        entity = (IObligatoryParameterHandler) new StringParameterHandler();
        break;
      case "SheetNumber":
        entity = (IObligatoryParameterHandler) new SheetNumberParameterHandler();
        break;
      default:
        if (paramType == typeof (bool))
        {
          entity = (IObligatoryParameterHandler) new BoolParameterHandler();
          break;
        }
        if (paramType == typeof (double))
        {
          entity = (IObligatoryParameterHandler) new FloatParameterHandler();
          break;
        }
        if (paramType == typeof (long))
        {
          entity = (IObligatoryParameterHandler) new IntParameterHandler();
          break;
        }
        if (paramType == typeof (string))
        {
          entity = (IObligatoryParameterHandler) new StringParameterHandler();
          break;
        }
        break;
    }
    Helper.CheckEntity((object) entity, typeof (IObligatoryParameterHandler));
    return new Parameter(name, entity.Value(strValue), readOnly, paramType);
  }
}
