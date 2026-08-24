// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeClientConsts
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeClientConsts
{
  private const string SmdoVer100Str = "SDIP-1.0";
  public static readonly int SmdoVer100Int = OfficeClientConsts.TranslateSmdoVerToInt("SDIP-1.0");
  private const string SmdoVer211Str = "SDIP-2.1.1";
  public static readonly int SmdoVer211Int = OfficeClientConsts.TranslateSmdoVerToInt("SDIP-2.1.1");
  public static int SmdoVerActualInt = OfficeClientConsts.TranslateSmdoVerToInt(OfficeClientConsts.SmdoVerActualStr);
  public static readonly bool isSmdoVer100Actual = OfficeClientConsts.SmdoVerActualInt == OfficeClientConsts.SmdoVer100Int;
  public static readonly bool isSmdoVer211Actual = OfficeClientConsts.SmdoVerActualInt == OfficeClientConsts.SmdoVer211Int;
  public static bool IsPrivateOffice = false;
  public static readonly string SmdoDateFormat = "yyyy-MM-dd";
  public const string PrivateRegistered = "PrivateRegistered";
  [NotNull]
  public static string OfficePropertyPageName = Localization.GetString("Office.Client_3");
  [NotNull]
  public static string GeneralPropertyPageName = Localization.GetString("Office.Client_1");
  [NotNull]
  public static string OfficeSupervisorsPageName = Localization.GetString("Office.Client_83");
  public static int CategorySubordinateRoot = -1;
  public static int CategoryIncomingDocuments = -1;
  public static Guid CategoryIncomingDocumentsGuid = new Guid("084F9508-0599-4225-943C-A58969794C4F");
  public static Guid CategorySubordinateRootGuid = new Guid("1AD8ABB6-F61B-430c-90E9-08FB86010145");
  [NotNull]
  public static string CmdOfficeMainNode = "Office.MainNode";
  [NotNull]
  public static string MenuOfficeMainNode = Localization.GetString("Office.Client_3");
  [NotNull]
  public static string CmdCreateResolution = "Office.CreateResolution";
  [NotNull]
  public static string CmdCreateResolutionByProto = "Office.CreateResolutionByProto";
  [NotNull]
  public static string CmdCreateConfidentialResolution = "Office.CreateConfidentialResolution";
  [NotNull]
  public static string MenuCreateResolution = Localization.GetString("Office.Client_54");
  [NotNull]
  public static string MenuCreateConfidentialResolution = Localization.GetString("Office.Client_82");
  [NotNull]
  public static string CmdRegisterDocument = "Office.RegisterDocument";
  [NotNull]
  public static string MenuRegisterDocument = Localization.GetString("Office.Client_55");
  [NotNull]
  public static string CmdAnswer = "Office.Answer";
  [NotNull]
  public static string MenuAnswer = Localization.GetString("Office.Client_56");
  [NotNull]
  public static string CmdRegisterAttachments = "Office.RegisterAttachments";
  [NotNull]
  public static string MenuRegisterAttachments = Localization.GetString("Office.Client_57");
  [NotNull]
  public static string CmdConvertToInternalMessage = "Office.ConvertToInternalMessage";
  [NotNull]
  public static string MenuConvertToInternalMessage = Localization.GetString("Office.Client_58");
  [NotNull]
  public static string CmdSendEmail = "Office.SendEmail";
  [NotNull]
  public static string MenuSendEmail = Localization.GetString("Office.Client_17");
  [NotNull]
  public static string CmdSendEmailProcess = "Office.SendEmailProcess";
  [NotNull]
  public static string MenuPrivateRegister = "Зарегистрировать во внутренней канцелярии";
  [NotNull]
  public static string CmdPrivateRegister = "Office.PrivateRegister";
  [NotNull]
  public static string MenuSendEmailProcess = Localization.GetString("Office.Client_20");
  [NotNull]
  public static string CmdGenerateRegNumber = "Office.GenerateRegNumber";
  [NotNull]
  public static string MenuGenerateRegNumber = Localization.GetString("Office.Client_80");
  public static int CategoryResolutionsRoot = -1;
  public static Guid CategoryResolutionsRootGuid = new Guid("C8E9C099-3F95-488C-B9F2-2B84C1BCDEA7");

  public static string SmdoVerActualStr => "SDIP-1.0";

  public static int TranslateSmdoVerToInt(string smdoVerStr)
  {
    string str = "SDIP-";
    if (!smdoVerStr.StartsWith(str))
      throw new Exception("Неверная версия СМДО: " + smdoVerStr);
    string[] strArray = smdoVerStr.Substring(str.Length).Split('.');
    int num = Convert.ToInt32(strArray[0]) * 100 * 100;
    if (strArray.Length > 1)
      num += Convert.ToInt32(strArray[1]) * 100;
    if (strArray.Length > 2)
      num += Convert.ToInt32(strArray[2]);
    return num;
  }

  public static void Init([NotNull] IUserSession session)
  {
  }
}
