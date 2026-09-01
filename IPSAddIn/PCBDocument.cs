// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PCBDocument
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using System;
using System.IO;

#nullable disable
namespace CSharpPlugin;

internal sealed class PCBDocument : 
  LongLifeObject,
  IPCBDocument,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IDisposable
{
  private readonly string _fileName;

  public PCBDocument(string fileName) => this._fileName = fileName;

  public void Dispose()
  {
  }

  public void AddNewParameter(Parameter parameter)
  {
  }

  public void SetParameterValue(string name, Type type, object parameterValue)
  {
  }

  public string InternalId => this._fileName;

  public string Name => Path.GetFileNameWithoutExtension(this._fileName);

  public string FilePath => this._fileName;

  public Parameter[] Parameters
  {
    get => new Parameter[0];
    set
    {
    }
  }
}
