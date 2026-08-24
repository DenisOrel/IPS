// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Settings.ButtonEditEventArgs
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using System;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.Settings;

public class ButtonEditEventArgs : EventArgs
{
  public ButtonedEdit edit;

  public ButtonEditEventArgs(ButtonedEdit Aedit) => this.edit = Aedit;
}
