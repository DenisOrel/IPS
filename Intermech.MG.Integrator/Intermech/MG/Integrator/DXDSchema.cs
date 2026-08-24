// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDSchema
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using Interop.Viewdraw;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDSchema : MGProjectItem<IVdObjs>
{
  private List<Tuple<int, IElectricalComponent, FunctionalGroup>> _sheets;
  private IVdApp _app;
  private string _cdbDesign;
  private IElectricalComponent _assemblyComponent;
  private List<IVdObjs> _openedBoards = new List<IVdObjs>();

  public DXDSchema(
    DXDProject project,
    IVdApp app,
    MGIntegratorSettings integratorSettings,
    string cdbDesign)
    : base((IMGProject) project, (IVdObjs) null, integratorSettings)
  {
    this._sheets = new List<Tuple<int, IElectricalComponent, FunctionalGroup>>();
    this._app = app;
    this._cdbDesign = cdbDesign;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IVdObjs vdObjs = app.DesignComponents(string.Empty, project.Instance.GetiCDBDesignRootBlock(cdbDesign), "-1", string.Empty);
    for (int index = 0; index < vdObjs.Count; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      if (vdObjs.GetType(index + 1) == VdObjectType.VDTS_COMPONENT)
      {
        // ISSUE: reference to a compiler-generated method
        DXDComponent component = this.GetComponent(vdObjs.Item(index + 1) as IVdComp);
        IComponentProperty property = component.GetProperty(this.integratorSettings.Sheet);
        if (property != null)
        {
          int result = int.MaxValue;
          FunctionalGroup functionalGroup = FunctionalGroupHelper.ReadFunctionalGroupFromComponent(this.integratorSettings, (IPropertiesCollection) component);
          if (!string.IsNullOrEmpty(Convert.ToString(property.Value)))
            int.TryParse(Convert.ToString(property.Value), out result);
          this._sheets.Add(new Tuple<int, IElectricalComponent, FunctionalGroup>(result, (IElectricalComponent) component, functionalGroup));
        }
      }
    }
    this.Instance = vdObjs;
  }

  public override List<IElectricalComponent> Components
  {
    get
    {
      if (this._sheets.Exists((Predicate<Tuple<int, IElectricalComponent, FunctionalGroup>>) (x => x.Item1 == int.MaxValue)) || !this._sheets.Exists((Predicate<Tuple<int, IElectricalComponent, FunctionalGroup>>) (x => x.Item2 != null)))
        return this.GetComponentsFromSheet(this.Instance);
      List<IElectricalComponent> components = new List<IElectricalComponent>();
      foreach (Tuple<int, IElectricalComponent, FunctionalGroup> sheet in this._sheets)
      {
        IVdObjs board = this._app.DesignComponents(string.Empty, ((MGObject<IProjectData>) this.parent).Instance.GetiCDBDesignRootBlock(this._cdbDesign), sheet.Item1.ToString(), string.Empty, false);
        List<IElectricalComponent> componentsFromSheet = this.GetComponentsFromSheet(board);
        if (componentsFromSheet.Count > 0)
        {
          foreach (IElectricalComponent electricalComponent in componentsFromSheet)
          {
            IElectricalComponent sheetComponent = electricalComponent;
            if (!components.Exists((Predicate<IElectricalComponent>) (x => x.UID == sheetComponent.UID)))
            {
              sheetComponent.FunctionalGroup = sheet.Item3;
              components.Add(sheetComponent);
            }
          }
        }
        this._openedBoards.Add(board);
      }
      return components;
    }
  }

  private List<IElectricalComponent> GetComponentsFromSheet(IVdObjs board)
  {
    List<IElectricalComponent> componentsFromSheet = new List<IElectricalComponent>(board.Count);
    for (int index = 0; index < board.Count; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      if (board.GetType(index + 1) == VdObjectType.VDTS_COMPONENT)
      {
        // ISSUE: reference to a compiler-generated method
        DXDComponent component = this.GetComponent(board.Item(index + 1) as IVdComp);
        if (component.GetProperty(this.integratorSettings.Sheet) == null)
          componentsFromSheet.Add((IElectricalComponent) component);
      }
    }
    return componentsFromSheet;
  }

  private DXDComponent GetComponent(IVdComp component)
  {
    return this.GetComponent(component, (FunctionalGroup) null);
  }

  private DXDComponent GetComponent(IVdComp component, FunctionalGroup functionalGroup)
  {
    DXDComponent component1 = new DXDComponent(component, this.integratorSettings, functionalGroup);
    this.relatedObjects.Add((IDisposable) component1);
    return component1;
  }

  public override IElectricalComponent AssemblyComponent
  {
    get
    {
      if (this._sheets.Count == 1)
        return this._sheets[0].Item2;
      if (this._sheets.Count <= 1)
        return (IElectricalComponent) null;
      Tuple<int, IElectricalComponent, FunctionalGroup> tuple = this._sheets.Find((Predicate<Tuple<int, IElectricalComponent, FunctionalGroup>>) (x => x.Item1 == 1));
      if (tuple != null)
        return tuple.Item2;
      this._sheets.Sort((Comparison<Tuple<int, IElectricalComponent, FunctionalGroup>>) ((x, y) => x.Item1.CompareTo(y.Item1)));
      return this._sheets[0].Item2;
    }
  }

  public override void Dispose()
  {
    foreach (object openedBoard in this._openedBoards)
      Marshal.FinalReleaseComObject(openedBoard);
    base.Dispose();
  }
}
