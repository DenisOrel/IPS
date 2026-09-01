// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.FormulaParser
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Extensions;
using System;
using System.Text;

#nullable disable
namespace CSharpPlugin;

internal static class FormulaParser
{
  private static readonly char[] formulaSymbols = new char[2]
  {
    '"',
    '\''
  };

  public static void Parse(Parameter[] pars)
  {
    foreach (Parameter par in pars)
    {
      string str1 = Convert.ToString(par.Value);
      if (str1.StartsWith("="))
      {
        StringBuilder stringBuilder = new StringBuilder();
        string str2 = str1.Remove(0, 1);
        char[] chArray = new char[1]{ '+' };
        foreach (string parameterName in str2.Split(chArray))
        {
          foreach (char formulaSymbol in FormulaParser.formulaSymbols)
          {
            if (parameterName.Length > 2 && parameterName.StartsWith(formulaSymbol.ToString()) && parameterName.EndsWith(formulaSymbol.ToString()))
              parameterName = parameterName.Substring(1, parameterName.Length - 2);
          }
          if (parameterName != par.Name)
          {
            string parameterValue = FormulaParser.GetParameterValue(parameterName, pars);
            stringBuilder.Append(parameterValue.IsNotNullOrWhiteSpace() ? parameterValue : parameterName);
          }
        }
        par.Value = (object) stringBuilder.ToString();
      }
    }
  }

  private static string GetParameterValue(string parameterName, Parameter[] pars)
  {
    Parameter parameter = Array.Find<Parameter>(pars, (Predicate<Parameter>) (p => p.Name == parameterName));
    return parameter == null ? string.Empty : Convert.ToString(parameter.Value);
  }
}
