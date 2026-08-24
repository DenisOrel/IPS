// Decompiled with JetBrains decompiler
// Type: OxyPlot.DelegateViewCommand`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace OxyPlot;

public class DelegateViewCommand<T> : IViewCommand<T>, IViewCommand where T : OxyInputEventArgs
{
  private readonly Action<IView, IController, T> handler;

  public DelegateViewCommand(Action<IView, IController, T> handler) => this.handler = handler;

  public void Execute(IView view, IController controller, T args)
  {
    this.handler(view, controller, args);
  }

  public void Execute(IView view, IController controller, OxyInputEventArgs args)
  {
    this.handler(view, controller, (T) args);
  }
}
