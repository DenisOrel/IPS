// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.LanguageServices
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class LanguageServices
{
  private ILanguageSessionService sessionService;
  private ITextEditorLanguageService textEditorService;
  private ServiceContainer customServices;

  public LanguageServices() => this.customServices = new ServiceContainer();

  public void AddSessionService(ILanguageSessionService service)
  {
    this.sessionService = service != null ? service : throw new ArgumentNullException(nameof (service));
  }

  public ILanguageSessionService GetSessionService(bool throwIfNotFound = true)
  {
    if (this.sessionService != null)
      return this.sessionService;
    if (!throwIfNotFound)
      return (ILanguageSessionService) null;
    throw this.ServiceNotAvailableException(typeof (ILanguageSessionService));
  }

  public void AddTextEditorService(ITextEditorLanguageService service)
  {
    this.textEditorService = service != null ? service : throw new ArgumentNullException(nameof (service));
  }

  public ITextEditorLanguageService GetTextEditorService(bool throwIfNotFound = true)
  {
    if (this.textEditorService != null)
      return this.textEditorService;
    if (!throwIfNotFound)
      return (ITextEditorLanguageService) null;
    throw this.ServiceNotAvailableException(typeof (ITextEditorLanguageService));
  }

  public void AddCustomService(Type serviceType, object serviceInstance)
  {
    if (serviceType == (Type) null)
      throw new ArgumentNullException(nameof (serviceType));
    if (serviceInstance == null)
      throw new ArgumentNullException(nameof (serviceInstance));
    this.customServices.AddService(serviceType, serviceInstance);
  }

  public object GetCustomService(Type serviceType, bool throwIfNotFound)
  {
    object customService = !(serviceType == (Type) null) ? this.customServices.GetService(serviceType) : throw new ArgumentNullException(nameof (serviceType));
    if (customService != null)
      return customService;
    if (!throwIfNotFound)
      return (object) null;
    throw this.ServiceNotAvailableException(serviceType);
  }

  private Exception ServiceNotAvailableException(Type serviceType)
  {
    return (Exception) new ScriptDesignTimeException($"Сервис '{serviceType}' не был предоставлен для языка сценариев.");
  }
}
