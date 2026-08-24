// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDProject
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Interop.Viewdraw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDProject(
  IProjectData project,
  MGIntegratorSettings integratorSettings,
  IIntegratorOutput outputSvc,
  IVdApp application) : MGProject<IProjectData, IVdApp>(project, integratorSettings, outputSvc, application)
{
  public override Dictionary<string, IMGProjectItem> GetProjectItems()
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IStringList o = this.Instance.GetiCDBDesigns();
    try
    {
      // ISSUE: reference to a compiler-generated method
      int count = o.GetCount();
      Dictionary<string, IMGProjectItem> projectItems = new Dictionary<string, IMGProjectItem>(count);
      for (int index = 0; index < count; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        string str = o.GetItem(index + 1);
        DXDSchema schema = new DXDSchema(this, this.app, this.integratorSettings, str);
        this.relatedObjects.Add((IDisposable) schema);
        if (!this.FilterSchema(schema))
          projectItems.Add(str, (IMGProjectItem) schema);
      }
      return projectItems;
    }
    finally
    {
      Marshal.FinalReleaseComObject((object) o);
    }
  }

  private bool FilterSchema(DXDSchema schema)
  {
    if (this.integratorSettings.NotImportetBoardSettings == null || this.integratorSettings.NotImportetBoardSettings.Count == 0)
      return false;
    IElectricalComponent assemblyComponent = schema.AssemblyComponent;
    if (assemblyComponent == null)
      return false;
    foreach (Tuple<StringKey, StringKey> importetBoardSetting in this.integratorSettings.NotImportetBoardSettings)
    {
      string other = Convert.ToString(assemblyComponent.GetPropertyValue((string) importetBoardSetting.Item1));
      if (importetBoardSetting.Item2.Equals(other))
        return true;
    }
    return false;
  }

  protected override IValueBagContainer GetProperties()
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IStringList o = this.Instance.GetiCDBDesigns();
    try
    {
      // ISSUE: reference to a compiler-generated method
      int count = o.GetCount();
      if (count == 0)
        return (IValueBagContainer) null;
      List<Tuple<int, IElectricalComponent>> tupleList = new List<Tuple<int, IElectricalComponent>>();
      for (int index = 0; index < count; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        DXDSchema dxdSchema = new DXDSchema(this, this.app, this.integratorSettings, o.GetItem(index + 1));
        this.relatedObjects.Add((IDisposable) dxdSchema);
        IElectricalComponent assemblyComponent = dxdSchema.AssemblyComponent;
        if (assemblyComponent != null)
        {
          if (assemblyComponent.GetProperty(this.integratorSettings.MainSchemeId) != null)
            return (IValueBagContainer) assemblyComponent;
          IComponentProperty property = assemblyComponent.GetProperty(this.integratorSettings.Sheet);
          if (property != null)
          {
            int result = int.MaxValue;
            if (!string.IsNullOrEmpty(Convert.ToString(property.Value)))
              int.TryParse(Convert.ToString(property.Value), out result);
            tupleList.Add(new Tuple<int, IElectricalComponent>(result, assemblyComponent));
          }
        }
      }
      tupleList.Sort((Comparison<Tuple<int, IElectricalComponent>>) ((x, y) => x.Item1.CompareTo(y.Item1)));
      return tupleList.Count != 0 ? (IValueBagContainer) tupleList[0].Item2 : throw new Exception("В проекте не найдено ни одного листа схемы! Проверьте настройку параметра штампа, указывающего номер листа.");
    }
    finally
    {
      Marshal.FinalReleaseComObject((object) o);
    }
  }

  public override bool IsValid()
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IStringList o = this.Instance.GetiCDBDesigns();
    try
    {
      // ISSUE: reference to a compiler-generated method
      return o.GetCount() > 0;
    }
    finally
    {
      Marshal.FinalReleaseComObject((object) o);
    }
  }

  protected override BoardReader<IMGProjectItem> GetBoardsReader(
    MGIntegratorSettings integratorSettings)
  {
    return (BoardReader<IMGProjectItem>) new DXDProjectBoardsReader((ECADIntegratorSettings) integratorSettings);
  }
}
