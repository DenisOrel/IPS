// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.LicenseServer
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using MGCPCBAutomationLicensing;

#nullable disable
namespace Intermech.MG.Integrator;

internal static class LicenseServer
{
  private static IApplication _instance;

  public static IApplication Instance
  {
    get
    {
      if (LicenseServer._instance != null)
      {
        try
        {
          LicenseServer._instance.GetToken(0);
        }
        catch
        {
          LicenseServer._instance = (IApplication) null;
        }
      }
      if (LicenseServer._instance == null)
        LicenseServer._instance = ComInstancesCreator<IApplication>.GetInstance(MGConsts.ExPCBLicenseProgID, MGConsts.ExPCBLicensex64ProgID);
      return LicenseServer._instance;
    }
  }

  public static int GetToken(int key) => LicenseServer.Instance.GetToken(key);
}
