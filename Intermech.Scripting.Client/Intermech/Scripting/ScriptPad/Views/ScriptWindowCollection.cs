// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.ScriptWindowCollection
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp.Components;
using WeifenLuo.WinFormsUI.Docking;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal sealed class ScriptWindowCollection(DockPanel dockPanel) : 
  DockPanelWindowCollection<ScriptWindow, IScriptWindow>(dockPanel),
  IScriptWindowCollection,
  IWindowCollection<IScriptWindow>
{
  protected override ScriptWindow DoCreateWindowControl() => new ScriptWindow();
}
