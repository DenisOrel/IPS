// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGIntegratorAPI
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Runtime.ComInterop.LocalServer;
using System;
using System.IO;

#nullable disable
namespace Intermech.MG.Integrator;

public abstract class MGIntegratorAPI : SingleThreadedObject
{
  protected void CreateFileDocument(string documentPath)
  {
    if (string.IsNullOrEmpty(documentPath))
      throw new ArgumentException("Не задан путь к регистрируемому документу.", nameof (documentPath));
    if (!File.Exists(documentPath))
      throw new FileNotFoundException($"Файл '{documentPath}' не найден на диске, его регистрация в IPS невозможна.");
    ServiceUtils.GetService<IFileImportService>((object) ServicesManager.ServiceContainer, true).ImportFile(documentPath);
  }

  protected long FindDocumentId(string documentPath, bool throwNotFound)
  {
    if (!string.IsNullOrEmpty(documentPath) && Path.IsPathRooted(documentPath))
    {
      IFileVault service = ServiceUtils.GetService<IFileVault>((object) ServicesManager.ServiceContainer, true);
      if (service.FindArea(documentPath) == service.WorkArea)
      {
        FileOrigin fileOrigin = service.WorkArea.GetFileOrigin(documentPath, false);
        if (fileOrigin.OriginType == FileOriginType.WorkFile)
          return fileOrigin.WorkObject.ObjectId;
      }
    }
    if (throwNotFound)
      throw new DocumentNotRegisteredException(documentPath);
    return 0;
  }

  protected void CreateSpecificationWindow(long assemblyID)
  {
    ServiceUtils.GetService<IECADIntegratorsDocumentService>((object) ServicesManager.ServiceContainer, true).CreateSpecificationWindow(assemblyID);
  }

  protected void SetError(Exception ex)
  {
    this.ErrorCode = 1;
    this.ErrorMessage = ex.Message;
  }

  protected void Prepare()
  {
    this.ErrorCode = 0;
    this.ErrorMessage = string.Empty;
  }

  public int ErrorCode { get; private set; }

  public string ErrorMessage { get; private set; }
}
