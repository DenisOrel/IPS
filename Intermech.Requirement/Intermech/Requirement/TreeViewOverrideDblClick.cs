// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.TreeViewOverrideDblClick
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Windows.Forms;

#nullable disable
namespace Intermech.Requirement;

internal class TreeViewOverrideDblClick : TreeView
{
  protected override void WndProc(ref Message m)
  {
    if (m.Msg == 515)
      return;
    base.WndProc(ref m);
  }
}
