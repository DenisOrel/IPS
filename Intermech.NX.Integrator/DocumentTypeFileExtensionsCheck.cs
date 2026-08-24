// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.DocumentTypeFileExtensionsCheck
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Settings;
using System;
using System.Collections.ObjectModel;
using System.IO;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class DocumentTypeFileExtensionsCheck : CADSettingsCheck
{
  private readonly IIntegrator integrator;

  public DocumentTypeFileExtensionsCheck(IIntegrator integrator)
  {
    this.integrator = integrator != null ? integrator : throw new ArgumentNullException(nameof (integrator));
  }

  protected override string DoPerformCheck(CADSettings settings, SettingsValidatorContext context)
  {
    if (context != SettingsValidatorContext.Generic)
      return (string) null;
    IApplicationFileTypes service = ServiceUtils.GetService<IApplicationFileTypes>((object) this.integrator, false);
    if (service == null)
      return (string) null;
    foreach (DocumentGroup fileDocumentGroup in (Collection<DocumentGroup>) settings.FileDocumentGroups)
    {
      foreach (GlobalId<int> documentType in fileDocumentGroup.DocumentTypes)
      {
        DocumentTypeSettings settings1 = DocumentTypeSettingsCache.GetSettings(documentType.Id);
        if (string.IsNullOrEmpty(settings1.DocumentFileExt))
          return $"Для типа документов '{documentType.Name}' не задано расширение файлов. Для корректной работы интегратора требуется, чтобы расширение файлы было задано.";
        string fileName = Path.ChangeExtension("samplefile", settings1.DocumentFileExt);
        if (!service.IsApplicationFile(fileName))
          return $"Для типа документов '{documentType.Name}' указано неподдерживаемое интегратором расширение файла {settings1.DocumentFileExt}.";
      }
    }
    return (string) null;
  }
}
