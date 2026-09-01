// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PhysicalComponent
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace CSharpPlugin;

internal sealed class PhysicalComponent : 
  Parametrable<IComponent>,
  ISchComponent,
  IParametrable,
  IValueBagContainer,
  IIdentification
{
  private readonly PhysicalDocument physicalDocument;

  public PhysicalComponent(IComponent component, PhysicalDocument document)
    : base(component)
  {
    this.physicalDocument = document;
  }

  public override string InternalId => this.DesignatorText;

  public string DesignatorText => this.parametrableObject.DM_FullPhysicalDesignator();

  protected override Parameter[] GetParameters()
  {
    List<Parameter> parameters = new List<Parameter>((IEnumerable<Parameter>) PhysicalParameterHelper.ReadParameters(this.parametrableObject))
    {
      new Parameter("ComponentKind", (object) (int) this.parametrableObject.DM_ComponentKind(), true, typeof (int))
    };
    this.physicalDocument.InvokeSchDocument((Action<SchDocument>) (doc =>
    {
      IEnumerable<Parameter> parameters1 = ((IEnumerable<Parameter>) this.FindComponent((ISchDocument) doc).Parameters).Where<Parameter>((Func<Parameter, bool>) (p1 => !parameters.Any<Parameter>((Func<Parameter, bool>) (p2 => p2.Name.Equals(p1.Name)))));
      if (!parameters1.Any<Parameter>())
        return;
      parameters.AddRange(parameters1);
    }));
    if (!parameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Name == "PosGuid")))
    {
      Parameter parameter = new Parameter("PosGuid", (object) Guid.NewGuid().ToString(), true, typeof (string));
      this.WriteNewParameter(parameter);
      parameters.Add(parameter);
    }
    return parameters.ToArray();
  }

  protected override void WriteNewParameter(Parameter parameter)
  {
    this.physicalDocument.InvokeSchDocument((Action<SchDocument>) (doc =>
    {
      ISchComponent component = this.FindComponent((ISchDocument) doc);
      try
      {
        component.AddNewParameter(parameter);
      }
      catch (ParameterAlreadyPresentException ex)
      {
        this.WriteParameterValue(component, parameter);
      }
      catch
      {
        throw;
      }
    }));
  }

  private ISchComponent FindComponent(ISchDocument document)
  {
    for (ISchComponent nextComponent = document.GetNextComponent(); nextComponent != null; nextComponent = document.GetNextComponent())
    {
      if (nextComponent.DesignatorText == this.parametrableObject.DM_LogicalDesignator())
        return nextComponent;
    }
    throw new Exception($"Компонент {this.DesignatorText} на схеме не найден");
  }

  private void WriteParameterValue(ISchComponent component, Parameter parameter)
  {
    component.SetParameterValue(parameter.Name, parameter.ParameterType, parameter.Value);
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    this.physicalDocument.InvokeSchDocument((Action<SchDocument>) (doc => this.WriteParameterValue(this.FindComponent((ISchDocument) doc), parameter)));
  }
}
