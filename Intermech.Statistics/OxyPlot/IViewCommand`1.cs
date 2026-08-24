// Decompiled with JetBrains decompiler
// Type: OxyPlot.IViewCommand`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public interface IViewCommand<in T> : IViewCommand where T : OxyInputEventArgs
{
  void Execute(IView view, IController controller, T args);
}
