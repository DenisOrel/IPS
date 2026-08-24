// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Adapter.Interbase.IBDataBaseType
// Assembly: Intermech.ImpExp.Adapter.Interbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97FBD89-71A5-4417-A5DC-2CB918616870
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Adapter.Interbase.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.Adapter.Interbase;

internal sealed class IBDataBaseType : IDataBaseType
{
  public static string DBType = "IntermechConnection.Interbase";

  public string DataBaseType() => IBDataBaseType.DBType;

  public IDataBase GetNewDataBase() => (IDataBase) new IBDataBase();
}
