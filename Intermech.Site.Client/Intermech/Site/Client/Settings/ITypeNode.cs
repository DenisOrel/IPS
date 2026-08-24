// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.ITypeNode
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal interface ITypeNode
{
  TreeNode[] Expand(IUserSession session);

  object Parameters { get; }

  bool Changed { get; set; }

  void Redraw(TreeNode node);
}
