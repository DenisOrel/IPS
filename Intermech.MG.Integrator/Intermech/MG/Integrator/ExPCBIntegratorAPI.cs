// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBIntegratorAPI
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Tools.Integrators;
using MGCPCB;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

[ComVisible(true)]
[Guid("1C9D4522-5FED-48F2-9106-32B34D7498B9")]
[ProgId("IPS.ExPCBIntegratorAPI")]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof (IExPCBIntegratorAPI))]
public class ExPCBIntegratorAPI : MGIntegratorAPI, IExPCBIntegratorAPI
{
  public void CreateElementList(Application application) => throw new NotImplementedException();

  public void CreateSpecification(Application application) => throw new NotImplementedException();

  public void ImportProject(Application application) => throw new NotImplementedException();

  public void SaveChanges(Application application) => throw new NotImplementedException();

  public void ViewDocumentProperties(Application application)
  {
    throw new NotImplementedException();
  }

  public int ImbaseBinding(Application application)
  {
    this.Prepare();
    int num = 0;
    try
    {
      // ISSUE: variable of a compiler-generated type
      MGCPCB.Document activeDocument = application.ActiveDocument;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Components components = activeDocument != null ? activeDocument.get_Components(EPcbSelectionType.epcbSelectSelected) : throw new DocumentNotOpenedException();
      if (components == null || components.Count <= 0)
        throw new SelectedComponentsNotFoundException();
      MGIntegratorSettings settings = ServiceUtils.GetService<MGSettingsService>((object) ClientContext.Integrators.GetIntegrator(new IntegratorObject(MGConsts.DXDIntegratorId, MGConsts.DXDIntegratorName), true), true).GetSettings();
      ExPCBPartEditor partEditor = new ExPCBPartEditor();
      partEditor.OpenDB(activeDocument);
      SpecialAttributesService specService = new SpecialAttributesService(partEditor);
      for (int vIndex = 1; vIndex <= components.Count; ++vIndex)
      {
        object o = (object) components[(object) vIndex];
        if (!(o is Component))
        {
          Marshal.ReleaseComObject(o);
        }
        else
        {
          ExPCBComponent exPcbComponent = new ExPCBComponent((Component) o, settings, specService);
          try
          {
            if (exPcbComponent.ImbaseBinding())
              ++num;
          }
          finally
          {
            exPcbComponent.Dispose();
          }
        }
      }
    }
    catch (Exception ex)
    {
      this.SetError(ex);
    }
    return num;
  }
}
