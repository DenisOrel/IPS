// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IScriptWindow
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface IScriptWindow
{
  event EventHandler<NavigateToCodeEventArgs> NavigateToCode;

  string Text { get; set; }

  IScriptCodeEditorControl CodeEditor { get; set; }

  OpenScriptData Script { get; set; }

  bool EnableNavigationPanel { get; set; }

  List<NavigationItem> NavigationTypes { get; }

  List<NavigationItem> NavigationMembers { get; }

  NavigationItem SelectedType { get; }

  NavigationItem SelectedMember { get; }

  void UpdateNavigationTypesSelection(NavigationItem typeToSelect);

  void UpdateNavigationMembersSelection(NavigationItem memberToSelect);

  void UpdateNavigationTypes(IList<NavigationItem> types, NavigationItem typeToSelect);

  void UpdateNavigationMembers(IList<NavigationItem> members, NavigationItem memberToSelect);
}
