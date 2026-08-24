// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Services.ScriptPadServiceNinjectModule
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.CSharp.DesignTime;
using Intermech.Scripting.Projects.DBScripts;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using System;

#nullable disable
namespace Intermech.Scripting.Services;

internal sealed class ScriptPadServiceNinjectModule : NinjectModule
{
  public override void Load()
  {
    this.Bind<DBScriptFactory>().ToMethod(new Func<IContext, DBScriptFactory>(this.CreateDBScriptFactory)).InSingletonScope();
    this.Bind<DBScriptRepository>().ToSelf().InSingletonScope();
    this.Bind<ScriptPadService, IScriptPadService>().To<ScriptPadService>().InSingletonScope();
  }

  private DBScriptFactory CreateDBScriptFactory(IContext context)
  {
    return new DBScriptFactory(context.Kernel.Get<CSharpLanguageExtension>().LanguageInfo);
  }
}
