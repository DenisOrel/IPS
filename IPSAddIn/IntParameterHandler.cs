// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.IntParameterHandler
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

#nullable disable
namespace CSharpPlugin;

internal sealed class IntParameterHandler : ParameterHandler<long>, IObligatoryParameterHandler
{
  public IntParameterHandler()
    : base(long.MinValue)
  {
  }

  public object Value(string text)
  {
    if (text == null || text == string.Empty)
      return (object) this.unknownValue;
    long result;
    return !long.TryParse(text, out result) ? (object) this.unknownValue : (object) result;
  }
}
