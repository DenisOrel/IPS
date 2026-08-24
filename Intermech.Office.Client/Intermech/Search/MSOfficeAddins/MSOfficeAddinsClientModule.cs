// Decompiled with JetBrains decompiler
// Type: Intermech.Search.MSOfficeAddins.MSOfficeAddinsClientModule
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Search.MSOfficeAddins;

public sealed class MSOfficeAddinsClientModule
{
  private MSOfficeAddinsComService _msOfficeAddinsComService = new MSOfficeAddinsComService();
  private RegistrationServices _registrationServices = new RegistrationServices();
  private int _msOfficeAddinsComServiceCookie;

  public void Load()
  {
    try
    {
      this._msOfficeAddinsComServiceCookie = this._registrationServices.RegisterTypeForComClients(typeof (MSOfficeAddinsComService), RegistrationClassContext.LocalServer, RegistrationConnectionType.MultipleUse);
    }
    catch (Exception ex)
    {
      this.WriteException(ex);
    }
  }

  public void Unload()
  {
    try
    {
      this._registrationServices.UnregisterTypeForComClients(this._msOfficeAddinsComServiceCookie);
    }
    catch (Exception ex)
    {
      this.WriteException(ex);
    }
  }

  private void WriteException(Exception exception)
  {
    if (!(ServicesManager.GetService(typeof (IOutputView)) is IOutputView service))
      return;
    service.WriteString("Ошибки", exception.Message);
  }
}
