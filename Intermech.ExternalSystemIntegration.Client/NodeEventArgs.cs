// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.NodeEventArgs
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Infralution.Controls.VirtualTree;
using System;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class NodeEventArgs : EventArgs
{
  public NodeEventArgs(Row Row) => this.Row = Row;

  public Row Row { get; private set; }
}
