// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ConfigurationLoader
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.Manager;

public static class ConfigurationLoader
{
  public static IConfiguration Load(string filename) => new ConfigurationImpl().Load(filename);
}
