// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.NavigateToCodeEventArgs
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class NavigateToCodeEventArgs : EventArgs
{
  public NavigateToCodeEventArgs(NavigationItem navigationItem)
  {
    this.NavigationItem = navigationItem != null ? navigationItem : throw new ArgumentNullException(nameof (navigationItem));
  }

  public NavigationItem NavigationItem { get; }
}
