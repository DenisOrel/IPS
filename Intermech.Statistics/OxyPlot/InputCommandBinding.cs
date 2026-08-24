// Decompiled with JetBrains decompiler
// Type: OxyPlot.InputCommandBinding
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot;

public class InputCommandBinding
{
  public InputCommandBinding(OxyInputGesture gesture, IViewCommand command)
  {
    this.Gesture = gesture;
    this.Command = command;
  }

  public InputCommandBinding(OxyKey key, OxyModifierKeys modifiers, IViewCommand command)
    : this((OxyInputGesture) new OxyKeyGesture(key, modifiers), command)
  {
  }

  public InputCommandBinding(
    OxyMouseButton mouseButton,
    OxyModifierKeys modifiers,
    IViewCommand command)
    : this((OxyInputGesture) new OxyMouseDownGesture(mouseButton, modifiers), command)
  {
  }

  public OxyInputGesture Gesture { get; private set; }

  public IViewCommand Command { get; private set; }
}
