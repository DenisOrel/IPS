// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDIntegratorAPI
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Commands;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Controls;
using Intermech.Tools.Integrators;
using Interop.Viewdraw;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

[ComVisible(true)]
[Guid("0F2B37A1-49D6-4049-AF76-32AA312742DB")]
[ProgId("IPS.DXDIntegratorAPI")]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof (IDXDIntegratorAPI))]
public class DXDIntegratorAPI : MGIntegratorAPI, IDXDIntegratorAPI
{
  public void CreateSpecification(IVdApp application)
  {
    this.Prepare();
    try
    {
      long documentId = this.FindDocumentId(this.CurrentProjectPath(application), true);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        this.CreateSpecificationWindow((ServiceUtils.GetService<IArticleService>((object) ServicesManager.ServiceContainer, true).FindBaseArticle(documentId, VersionsRuleSources.GetEditorRule().OwnerId, (object) sessionKeeper.Session) ?? throw new Exception($"Для документа не найдено базовое исполнение. Выполните для {sessionKeeper.Session.GetObject(documentId).NameInMessages} расширенное сохранение.")).ObjectID);
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }

  public void CreateElementList(IVdApp application)
  {
    this.Prepare();
    try
    {
      long documentId = this.FindDocumentId(this.CurrentProjectPath(application), true);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        new DXDElementList().Create(sessionKeeper.Session, documentId);
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }

  public void ImbaseBinding(IVdApp application)
  {
    this.Prepare();
    try
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      IVdObjs vdObjs = (application.ActiveView ?? throw new DocumentNotOpenedException()).Query(VdObjectTypeMask.VDM_COMP, VdAllOrSelected.VD_SELECTED);
      if (vdObjs == null || vdObjs.Count <= 0)
        throw new SelectedComponentsNotFoundException();
      MGIntegratorSettings settings = ServiceUtils.GetService<MGSettingsService>((object) ClientContext.Integrators.GetIntegrator(new IntegratorObject(MGConsts.DXDIntegratorId, MGConsts.DXDIntegratorName), true), true).GetSettings();
      for (int Index = 1; Index <= vdObjs.Count; ++Index)
      {
        // ISSUE: reference to a compiler-generated method
        object obj = vdObjs.Item(Index);
        if (!(obj is IVdComp))
        {
          Marshal.ReleaseComObject(obj);
        }
        else
        {
          DXDComponent dxdComponent = new DXDComponent((IVdComp) obj, settings);
          try
          {
            dxdComponent.ImbaseBinding();
          }
          finally
          {
            dxdComponent.Dispose();
          }
        }
      }
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }

  public void ImportProject(IVdApp application)
  {
    this.Prepare();
    try
    {
      this.CreateFileDocument(this.CurrentProjectPath(application));
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }

  public void ViewDocumentProperties(IVdApp application)
  {
    this.Prepare();
    try
    {
      long documentId = this.FindDocumentId(this.CurrentProjectPath(application), true);
      int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, documentId);
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }

  public void SaveChanges(IVdApp application)
  {
    this.Prepare();
    try
    {
      long documentId = this.FindDocumentId(this.CurrentProjectPath(application), true);
      ObjectCommand saveChangesCommand = ObjectCommandFactory.CreateSaveChangesCommand(true);
      saveChangesCommand.ObjectId = documentId;
      saveChangesCommand.UpdateUI = false;
      saveChangesCommand.Execute();
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
  }

  private string CurrentProjectPath(IVdApp application)
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IProjectData projectData = application.GetProjectData();
    try
    {
      // ISSUE: reference to a compiler-generated method
      return projectData.GetProjectFilePath();
    }
    finally
    {
      Marshal.FinalReleaseComObject((object) projectData);
    }
  }
}
