// Decompiled with JetBrains decompiler
// Type: OxyPlot.OxyMouseDownGesture
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class OxyMouseDownGesture : OxyInputGesture
{
  public OxyMouseDownGesture(OxyMouseButton mouseButton, OxyModifierKeys modifiers = OxyModifierKeys.None, int clickCount = 1)
  {
    this.MouseButton = mouseButton;
    this.Modifiers = modifiers;
    this.ClickCount = clickCount;
  }

  public OxyModifierKeys Modifiers { get; private set; }

  public OxyMouseButton MouseButton { get; private set; }

  public int ClickCount { get; private set; }

  public override bool Equals(OxyInputGesture other)
  {
    return other is OxyMouseDownGesture mouseDownGesture && mouseDownGesture.Modifiers == this.Modifiers && mouseDownGesture.MouseButton == this.MouseButton && mouseDownGesture.ClickCount == this.ClickCount;
  }
}
