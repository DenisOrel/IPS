// Decompiled with JetBrains decompiler
// Type: OxyPlot.ManipulatorBase`1
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public abstract class ManipulatorBase<T> where T : OxyInputEventArgs
{
  protected ManipulatorBase(IView view) => this.View = view;

  public IView View { get; private set; }

  public virtual void Completed(T e)
  {
  }

  public virtual void Delta(T e)
  {
  }

  public virtual void Started(T e)
  {
  }
}
