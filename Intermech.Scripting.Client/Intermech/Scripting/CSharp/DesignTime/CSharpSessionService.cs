// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpSessionService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using Intermech.Mvp.Components.Dialogs;
using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpSessionService : ILanguageSessionService
{
  private Func<CSharpSession> sessionFactory;
  private CSharpSessionParameters defaultParameters;

  public CSharpSessionService(Func<CSharpSession> sessionFactory)
  {
    this.sessionFactory = sessionFactory != null ? sessionFactory : throw new ArgumentNullException(nameof (sessionFactory));
    this.defaultParameters = new CSharpSessionParameters();
  }

  public ILanguageSessionParameters CreateSessionParameters()
  {
    return (ILanguageSessionParameters) this.defaultParameters.Clone();
  }

  public ILanguageSessionParameters LoadSessionParameters(ISettingsContainer container)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    return (ILanguageSessionParameters) this.defaultParameters.Clone();
  }

  public void SaveSessionParameters(
    ISettingsContainer container,
    ILanguageSessionParameters parameters)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
  }

  public bool EditSessionParameters(ILanguageSessionParameters parameters)
  {
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
    MvpContext.ViewService.ShowModal((IPresenter) new SimpleMessagePresenter("У исполнителя C#-сценариев нет параметров, доступных для редактирования.", "Сообщение", MessageIcon.Information));
    return false;
  }

  public ILanguageSession CreateSession(ILanguageSessionParameters parameters)
  {
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
    CSharpSession session = this.sessionFactory();
    session.DebugStream = parameters.Stdout;
    return (ILanguageSession) session;
  }
}
