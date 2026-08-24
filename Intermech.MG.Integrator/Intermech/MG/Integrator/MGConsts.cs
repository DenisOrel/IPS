// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGConsts
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal static class MGConsts
{
  public static readonly string PluginName = "Интегратор с приложениями Mentor Graphics";
  public static readonly string DXDIntegratorName = "Интегратор с DxDesigner";
  public static readonly string ExPCBIntegratorName = "Интегратор с Expedition PCB";
  public static readonly string DXDApplicationName = "Mentor Graphics DxDesigner";
  public static readonly string ExPCBApplicationName = "Mentor Graphics Expedition PCB";
  public static readonly Guid DXDIntegratorId = new Guid("F4D4FE2D-C526-44E5-B26E-9DFF5C17FE7C");
  public static readonly Guid ExPCBIntegratorId = new Guid("{2B33FA1C-BE81-4E41-90C5-BBB23E34463A}");
  public static readonly Guid ObjTypeDXDProject = new Guid("cadd93d6-306c-11d8-b4e9-00304f19f545");
  public static readonly Guid ObjTypeExPCBProject = new Guid("cadd96fe-306c-11d8-b4e9-00304f19f545");
  public static readonly Guid ObjTypeDocumentMG = new Guid("cadd93d6-306c-11d8-b4e9-00304f19f545");
  public static readonly string DXDProgID = "Viewdraw.Application";
  public static readonly string DXDClassID = "{EA4ABD70-84B0-11CE-8237-00001B4D36B5}";
  public static readonly string DXDClassID21 = "{77BAC7E1-6BF6-1014-BC64-956CFB04B493}";
  public static readonly string DXDClassID22 = "{B2896BF0-7157-1014-8FDA-BD37BBB75522}";
  public static readonly string PartsEditorProgID = "MGCPCBLibraries.PartsEditorDlg";
  public static readonly string PartsEditorx64ProgID = "Intermech.MG.PartsDatabaseService";
  public static readonly string ExPCBProgID = "MGCPCB.ExpeditionPCBApplication";
  public static readonly string ExPCBLicenseProgID = "MGCPCBAutomationLicensing.Application";
  public static readonly string ExPCBLicensex64ProgID = "Intermech.MG.LicenseService";
  public static readonly string ExPCBClassID = "{EDEDED02-D5F6-4B04-8FE7-EDEDEDEDED01}";
  public static readonly string ExPCBClassID21 = "{77BF19BC-6BF6-1014-BA4D-D7EA92EBF620}";
  public static readonly string ExPCBClassID22 = "{B28C4761-7157-1014-8CA3-C44A7CF836E2}";
  public static readonly string ExPCBLicenseClassID = "{87F05DE4-AABE-4C41-9B45-4BF08E47DEB4}";
  public static readonly string WDirVariable = "WDIR";
  public static readonly string LicenseVariable = "MGLS_LICENSE_FILE";
  public static readonly string ProjectFileExtension = ".prj";
  public static readonly string PosGuidAttribute = "PosGuid";
  public static readonly Guid SchemaId = new Guid("C12A1B7C-A12A-459D-851A-879BD2BF13EE");
  public static readonly Guid MGDocuments = new Guid("52B1353F-4311-4E70-91ED-BA3BD27CEB45");
}
