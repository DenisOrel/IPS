// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Addons.AddonsModule
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.ApplicationModel;
using Intermech.Client.Core;
using Intermech.Scripting.Services;
using System;

#nullable disable
namespace Intermech.Scripting.Addons;

internal sealed class AddonsModule : InitializerModule
{
  private IScriptPadService scriptPadService;

  public AddonsModule(IScriptPadService scriptPadService)
  {
    this.scriptPadService = scriptPadService != null ? scriptPadService : throw new ArgumentNullException(nameof (scriptPadService));
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.RegisterDBScriptTypes();
  }

  private void RegisterDBScriptTypes()
  {
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.Scheduler), (DBScriptProjectInitializer) new SchedulerScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.LCStep), (DBScriptProjectInitializer) new LCStepScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.ScriptsForButtons), (DBScriptProjectInitializer) new FormButtonScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.AutoSelection), (DBScriptProjectInitializer) new TechAutoselectionScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.TechNumeration), (DBScriptProjectInitializer) new TechNumerationScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.ExpertReport), (DBScriptProjectInitializer) new ExpertReportScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.ExpertComplectDoc), (DBScriptProjectInitializer) new ExpertComplectDocScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.ExpertScenario), (DBScriptProjectInitializer) new ExpertScenarioScriptProjectInitializer());
    this.scriptPadService.RegisterScriptProjectInitializer(ScriptTypeHelper.GetObjType4ScriptType(ScriptTypes.ReportCompareObjects), (DBScriptProjectInitializer) new ReportCompareObjectsScriptProjectInitializer());
  }
}
