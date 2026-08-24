// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.DTSuffixesHelper
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal static class DTSuffixesHelper
{
  private static IDocumentTypeSettingsService _DTSettingsSrv = (IDocumentTypeSettingsService) null;
  public static List<Tuple<string, string, int>> DTSuffixes = new List<Tuple<string, string, int>>();

  public static void FillDTSuffixes(
    IUserSession session,
    IObjectTypeItemList objectTypes,
    IObjectTypeItem type)
  {
    if (DTSuffixesHelper._DTSettingsSrv == null)
      DTSuffixesHelper._DTSettingsSrv = session.GetCustomService(typeof (IDocumentTypeSettingsService)) as IDocumentTypeSettingsService;
    DocumentTypeSettings settings = DTSuffixesHelper._DTSettingsSrv.GetSettings(session.SessionGUID, type.ID);
    string docTypeCode = settings.DocumentTypeCode.Trim();
    string fileExt = settings.DocumentFileExt.ToLower();
    if (!string.IsNullOrEmpty(docTypeCode) && !DTSuffixesHelper.DTSuffixes.Exists((Predicate<Tuple<string, string, int>>) (x => x.Item1.Equals(docTypeCode) && x.Item2.Equals(fileExt))) && type.VersionableMode != ObjectVersionModes.Abstract)
      DTSuffixesHelper.DTSuffixes.Add(new Tuple<string, string, int>(docTypeCode, fileExt, type.ID));
    foreach (int id in new List<int>((IEnumerable<int>) type.ChildIDs))
    {
      IObjectTypeItem byId = objectTypes.GetByID(id);
      if (byId != null)
        DTSuffixesHelper.FillDTSuffixes(session, objectTypes, byId);
    }
  }

  public static int FindDocTypeBySuffix(string designation, string fileExt)
  {
    Tuple<string, string, int> tuple = string.IsNullOrEmpty(fileExt) ? DTSuffixesHelper.DTSuffixes.Find((Predicate<Tuple<string, string, int>>) (x => designation.EndsWith(x.Item1))) : DTSuffixesHelper.DTSuffixes.Find((Predicate<Tuple<string, string, int>>) (x => designation.EndsWith(x.Item1) && x.Item2.Equals(fileExt)));
    return tuple == null ? PumpHelper.ObjTypePaperDocumentID : tuple.Item3;
  }
}
