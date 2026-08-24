// Decompiled with JetBrains decompiler
// Type: OxyPlot.DelegatePlotCommand`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public class DelegatePlotCommand<T>(Action<IPlotView, IController, T> handler) : 
  DelegateViewCommand<T>((Action<IView, IController, T>) ((v, c, e) => handler((IPlotView) v, c, e)))
  where T : OxyInputEventArgs
{
}
