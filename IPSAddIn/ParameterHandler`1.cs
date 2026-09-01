// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.ParameterHandler`1
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

#nullable disable
namespace CSharpPlugin;

internal abstract class ParameterHandler<T>
{
  protected readonly T unknownValue;

  public ParameterHandler(T unknownValue) => this.unknownValue = unknownValue;
}
