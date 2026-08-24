// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.CSharpNinjectModule
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.CSharp.DesignTime;
using Ninject.Modules;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal sealed class CSharpNinjectModule : NinjectModule
{
  public override void Load()
  {
    this.Bind<CSharpSession>().ToSelf();
    this.Bind<CSharpSessionService>().ToSelf();
    this.Bind<CSharpLanguageExtension>().ToSelf().InSingletonScope();
  }
}
