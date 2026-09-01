// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.StringParameterHandler
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

#nullable disable
namespace CSharpPlugin;

internal sealed class StringParameterHandler : ParameterHandler<string>, IObligatoryParameterHandler
{
  public StringParameterHandler()
    : base(string.Empty)
  {
  }

  public object Value(string text) => text != null ? (object) text : (object) this.unknownValue;
}
