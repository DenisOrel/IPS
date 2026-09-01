// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.LongLifeObject
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using System;

#nullable disable
namespace CSharpPlugin;

public class LongLifeObject : MarshalByRefObject
{
  public override object InitializeLifetimeService() => (object) null;
}
