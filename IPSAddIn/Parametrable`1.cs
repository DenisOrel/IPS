// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Parametrable`1
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace CSharpPlugin;

internal abstract class Parametrable<TComponent> : 
  LongLifeObject,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IDisposable
{
  protected TComponent parametrableObject;
  private Parameter[] _parameters;

  public Parametrable(TComponent parametrableObject)
  {
    this.parametrableObject = parametrableObject;
  }

  protected abstract Parameter[] GetParameters();

  public virtual Parameter[] Parameters
  {
    get
    {
      if (this._parameters == null)
      {
        this._parameters = this.GetParameters();
        if (this._parameters != null && this._parameters.Length != 0)
          FormulaParser.Parse(this._parameters);
      }
      return this._parameters;
    }
    set
    {
      foreach (Parameter parameter in value)
      {
        if (parameter.Modified != ModifiedTypes.None)
        {
          if (parameter.Modified == ModifiedTypes.Changed)
            this.WriteParameterValue(parameter);
          else if (parameter.Modified == ModifiedTypes.Added)
            this.WriteNewParameter(parameter);
        }
      }
      this._parameters = value;
      this.SetModified();
    }
  }

  public void AddNewParameter(Parameter parameter)
  {
    if (parameter == null)
      throw new ArgumentNullException(nameof (parameter));
    List<Parameter> list = ((IEnumerable<Parameter>) this.Parameters).ToList<Parameter>();
    if (list.Any<Parameter>((Func<Parameter, bool>) (item => item.Name == parameter.Name)))
      throw new ParameterAlreadyPresentException(parameter.Name);
    this.WriteNewParameter(parameter);
    list.Add(parameter);
    this._parameters = list.ToArray();
    this.SetModified();
  }

  public virtual void SetModified()
  {
  }

  public abstract string InternalId { get; }

  public void SetParameterValue(string name, Type type, object parameterValue)
  {
    this.WriteParameterValue(new Parameter(name, parameterValue, false, type));
    this.SetModified();
  }

  protected abstract void WriteNewParameter(Parameter parameter);

  protected abstract void WriteParameterValue(Parameter parameter);

  public virtual void Dispose()
  {
  }
}
