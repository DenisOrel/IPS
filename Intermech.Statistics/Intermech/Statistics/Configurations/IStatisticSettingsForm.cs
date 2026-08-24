// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.IStatisticSettingsForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

internal interface IStatisticSettingsForm
{
  event EventHandler OnApplied;

  event EventHandler OnModified;

  event EventHandler OnCancelModify;

  CommandSettings Settings { get; }

  void Save(object sender, EventArgs e);

  void SetAsControl(Control parentControl);

  void InitForm(CommandSettings commandSettings);
}
