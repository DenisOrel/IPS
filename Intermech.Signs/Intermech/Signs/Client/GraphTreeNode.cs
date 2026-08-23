// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.GraphTreeNode
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

internal class GraphTreeNode : TreeNode
{
  private string graphID = string.Empty;

  public string GraphID => this.graphID;

  public GraphTreeNode(string text, string graphID)
    : base(text)
  {
    this.graphID = graphID;
  }
}
