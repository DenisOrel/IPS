// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SheetNumberParameterHandler
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

#nullable disable
namespace CSharpPlugin;

internal sealed class SheetNumberParameterHandler : 
  ParameterHandler<long>,
  IObligatoryParameterHandler
{
  public SheetNumberParameterHandler()
    : base(long.MinValue)
  {
  }

  public object Value(string text)
  {
    if (text == null || text == string.Empty)
      return (object) this.unknownValue;
    int result;
    return !int.TryParse(text, out result) ? (object) this.unknownValue : (object) result;
  }
}
