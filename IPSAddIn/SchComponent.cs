// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SchComponent
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using SCH;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace CSharpPlugin;

internal sealed class SchComponent : 
  Parametrable<ISch_Component>,
  ISchComponent,
  IParametrable,
  IValueBagContainer,
  IIdentification
{
  private readonly string _schemaFileName;

  public SchComponent(ISch_Component component, string schemaFileName)
    : base(component)
  {
    this._schemaFileName = schemaFileName;
  }

  public string DesignatorText
  {
    get
    {
      ISch_Designator stateSchDesignator = this.parametrableObject.GetState_SchDesignator();
      try
      {
        return stateSchDesignator.GetState_Text();
      }
      finally
      {
        Marshal.FinalReleaseComObject((object) stateSchDesignator);
      }
    }
  }

  public override string InternalId => this.DesignatorText;

  public override void SetModified() => Helper.SetModified(this._schemaFileName);

  protected override Parameter[] GetParameters()
  {
    return new List<Parameter>((IEnumerable<Parameter>) SchParametersHelper.ReadParameters((ISch_BasicContainer) this.parametrableObject))
    {
      new Parameter("ComponentKind", (object) (int) this.parametrableObject.GetState_ComponentKind(), true, typeof (int))
    }.ToArray();
  }

  protected override void WriteNewParameter(Parameter parameter)
  {
    SchParametersHelper.WriteNewParameter((ISch_BasicContainer) this.parametrableObject, parameter);
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    SchParametersHelper.WriteParameterValue((ISch_BasicContainer) this.parametrableObject, parameter);
  }
}
