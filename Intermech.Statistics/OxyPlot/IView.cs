// Decompiled with JetBrains decompiler
// Type: OxyPlot.IView
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public interface IView
{
  Model ActualModel { get; }

  IController ActualController { get; }

  OxyRect ClientArea { get; }

  void SetCursorType(CursorType cursorType);

  void HideZoomRectangle();

  void ShowZoomRectangle(OxyRect rectangle);
}
