// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepSettingsSaver
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class StepSettingsSaver
{
  private List<StepControl> _controls;
  private List<int> _savedSteps;

  public StepSettingsSaver(List<StepControl> controls)
  {
    this._controls = controls;
    this._savedSteps = new List<int>(controls.Count);
  }

  public SaveSettingsResult Save()
  {
    int num = 0;
    foreach (StepControl control in this._controls)
    {
      if (control.StepPrevAllowed && !this._savedSteps.Contains(num))
      {
        SaveSettingsResult saveSettingsResult = control.SaveSettings();
        if (saveSettingsResult != SaveSettingsResult.ssrOk)
          return saveSettingsResult;
        this._savedSteps.Add(num);
        control.StepPrevAllowed = false;
      }
      ++num;
    }
    return SaveSettingsResult.ssrOk;
  }

  private void SaveSettingsThreadMethod()
  {
  }
}
