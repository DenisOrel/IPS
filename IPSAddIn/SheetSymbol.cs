// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SheetSymbol
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using SCH;
using System.Collections.Generic;

#nullable disable
namespace CSharpPlugin;

internal sealed class SheetSymbol(ISch_SheetSymbol symbol) : 
  Parametrable<ISch_SheetSymbol>(symbol),
  ISchSheetSymbol,
  ISchComponent,
  IParametrable,
  IValueBagContainer,
  IIdentification
{
  public string FileName => this.parametrableObject.GetState_SchSheetFileName().GetState_Text();

  protected override Parameter[] GetParameters()
  {
    return new List<Parameter>((IEnumerable<Parameter>) SchParametersHelper.ReadParameters((ISch_BasicContainer) this.parametrableObject)).ToArray();
  }

  public override string InternalId => this.parametrableObject.GetState_UniqueId();

  protected override void WriteNewParameter(Parameter parameter)
  {
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
  }

  public string DesignatorText => this.parametrableObject.GetState_Text();
}
