// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.PumpWorkflowSettings
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal static class PumpWorkflowSettings
{
  public static int BigSchemeActivitiesCount = 150;
  public static int UnrealBigSchemeActivitiesCount = 500;
  public static WFOptions Options = WFOptions.PumpSchemes;
  public static DateTime StartDT;
  public static DateTime EndDT;
  public const string SettingsSection = "WFSETTINGS";

  public static string StrOptions
  {
    set
    {
      if (value.Contains("S"))
        PumpWorkflowSettings.Options |= WFOptions.PumpSchemes;
      if (value.Contains("P"))
        PumpWorkflowSettings.Options |= WFOptions.PumpProcesses;
      if (value.Contains("T"))
        PumpWorkflowSettings.Options |= WFOptions.PumpTerminated;
      if (value.Contains("C"))
        PumpWorkflowSettings.Options |= WFOptions.PumpCompleted;
      if (value.Contains("B"))
        PumpWorkflowSettings.Options |= WFOptions.PumpBig;
      if (!value.Contains("D"))
        return;
      PumpWorkflowSettings.Options |= WFOptions.PumpByDateTime;
    }
  }

  public static string ToString()
  {
    string str = "";
    if (PumpWorkflowSettings.HasOption(WFOptions.PumpSchemes))
      str += "S";
    if (PumpWorkflowSettings.HasOption(WFOptions.PumpProcesses))
      str += "P";
    if (PumpWorkflowSettings.HasOption(WFOptions.PumpTerminated))
      str += "T";
    if (PumpWorkflowSettings.HasOption(WFOptions.PumpCompleted))
      str += "C";
    if (PumpWorkflowSettings.HasOption(WFOptions.PumpBig))
      str += "B";
    if (PumpWorkflowSettings.HasOption(WFOptions.PumpByDateTime))
      str += $"D:({PumpWorkflowSettings.StartDT}, {PumpWorkflowSettings.EndDT})";
    return str;
  }

  public static bool HasOption(WFOptions option)
  {
    return (PumpWorkflowSettings.Options & option) == option;
  }
}
