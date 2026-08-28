// Decompiled with JetBrains decompiler
// Type: Intermech.Server.Service.Program
// Assembly: Intermech.Server.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E91FE21E-230A-49EC-A627-5E0B3AE2517E
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Server.Service.exe

using Intermech.ApplicationModel;

#nullable disable
namespace Intermech.Server.Service;

internal sealed class Program(string[] aruments) : ServiceApplicationBase<IntermechServerService>(aruments)
{
  private static void Main(string[] args) => new Program(args).Run();
}
