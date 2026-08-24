// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.VarnishHelper
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class VarnishHelper
{
  public Guid ColorGuid { get; private set; } = Guid.Empty;

  public long FolderId { get; private set; }

  public List<Tuple<Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>>> Keys { get; private set; }

  public bool IsDataLoaded { get; private set; }

  public Guid NodeGuid => Consts.IMHVarnishHandbookNodeGuid;

  public VarnishHelper() => this.FolderId = 0L;

  public Dictionary<string, string> GetAttrValues(Guid attrTypeGuid)
  {
    Dictionary<string, string> attrValues = new Dictionary<string, string>();
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeGuid);
    if (attributeType == null)
      return attrValues;
    for (int index = 0; index < attributeType.PossibleValues.Count; ++index)
      attrValues.Add(attributeType.PossibleValues[index].ToString(), attributeType.PossibleValuesDescriptions[index].ToString());
    return attrValues;
  }

  public void LoadData(long folderId)
  {
    this.FolderId = folderId;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.ColorGuid = sessionKeeper.Session.GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService1 ? customService1.GetObjectGuidByName("COLOR_VARNISH_ATTR") : Guid.Empty;
      List<long> linksEntersInFolder = ImbaseHelper.GetLinksEntersInFolder(sessionKeeper.Session, folderId);
      if (linksEntersInFolder == null || !(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService2))
        return;
      List<Tuple<string, string, string, string, string>> source = new List<Tuple<string, string, string, string, string>>();
      foreach (long linkId in linksEntersInFolder)
      {
        List<Tuple<string, string, string, string, string>> elements = this.GetElements(sessionKeeper.Session, customService2, linkId);
        if (elements != null)
          source.AddRange((IEnumerable<Tuple<string, string, string, string, string>>) elements);
      }
      if (source.Count > 0)
      {
        Dictionary<string, string> dictionary1 = customService2.NameRecordReferences(sessionKeeper.Session.SessionGUID, source.Select<Tuple<string, string, string, string, string>, string>((System.Func<Tuple<string, string, string, string, string>, string>) (x => x.Item1)).Distinct<string>().ToList<string>());
        Dictionary<string, string> dictionary2 = customService2.NameRecordReferences(sessionKeeper.Session.SessionGUID, source.Select<Tuple<string, string, string, string, string>, string>((System.Func<Tuple<string, string, string, string, string>, string>) (x => x.Item2)).Distinct<string>().ToList<string>());
        Dictionary<string, string> attrValues = this.GetAttrValues(Consts.CoatingClassAttrTypeGuid);
        Dictionary<string, string> dictionary3 = customService2.NameRecordReferences(sessionKeeper.Session.SessionGUID, source.Select<Tuple<string, string, string, string, string>, string>((System.Func<Tuple<string, string, string, string, string>, string>) (x => x.Item4)).Distinct<string>().ToList<string>());
        Dictionary<string, string> dictionary4 = customService2.NameRecordReferences(sessionKeeper.Session.SessionGUID, source.Select<Tuple<string, string, string, string, string>, string>((System.Func<Tuple<string, string, string, string, string>, string>) (x => x.Item5)).Distinct<string>().ToList<string>());
        this.Keys = new List<Tuple<Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>>>(source.Count);
        foreach (Tuple<string, string, string, string, string> tuple in source)
        {
          string str1 = dictionary1.ContainsKey(tuple.Item1) ? dictionary1[tuple.Item1] : tuple.Item1;
          string str2 = dictionary2.ContainsKey(tuple.Item2) ? dictionary2[tuple.Item2] : tuple.Item2;
          string str3 = attrValues.ContainsKey(tuple.Item3) ? attrValues[tuple.Item3] : tuple.Item3;
          string str4 = dictionary3.ContainsKey(tuple.Item4) ? dictionary3[tuple.Item4] : tuple.Item4;
          string str5 = dictionary4.ContainsKey(tuple.Item5) ? dictionary4[tuple.Item5] : tuple.Item5;
          this.Keys.Add(new Tuple<Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>, Tuple<string, string>>(new Tuple<string, string>(tuple.Item1, str1), new Tuple<string, string>(tuple.Item2, str2), new Tuple<string, string>(tuple.Item3, str3), new Tuple<string, string>(tuple.Item4, str4), new Tuple<string, string>(tuple.Item5, str5)));
        }
      }
      this.IsDataLoaded = this.Keys != null && this.Keys.Count > 0;
    }
  }

  private List<Tuple<string, string, string, string, string>> GetElements(
    IUserSession session,
    IImbaseServer srv,
    long linkId)
  {
    DataTable table;
    srv.LoadRecords(session.SessionGUID, linkId, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out table, out AttributeTypeProperties[] _, out ImbaseKeyInfo _);
    if (table == null)
      return (List<Tuple<string, string, string, string, string>>) null;
    string colColorName = MetaDataHelper.GetAttributeTypeID(this.ColorGuid).ToString();
    return table.AsEnumerable().Select<DataRow, Tuple<string, string, string, string, string>>((System.Func<DataRow, Tuple<string, string, string, string, string>>) (row =>
    {
      string str1 = ImbaseHelper.MakeInternalImbaseKey(linkId, Convert.ToInt64(row["-2"]));
      string str2 = table.Columns.Contains(colColorName) ? row[colColorName].ToString() : string.Empty;
      string str3 = table.Columns.Contains(Consts.CoatingClassAttrTypeId.ToString()) ? row[Consts.CoatingClassAttrTypeId.ToString()].ToString() : string.Empty;
      string str4 = table.Columns.Contains(Consts.CoatingGroupAttrTypeId.ToString()) ? row[Consts.CoatingGroupAttrTypeId.ToString()].ToString() : string.Empty;
      string str5 = table.Columns.Contains(Consts.TermsOfUseAttrTypeId.ToString()) ? row[Consts.TermsOfUseAttrTypeId.ToString()].ToString() : string.Empty;
      string str6 = str2;
      string str7 = str3;
      string str8 = str4;
      string str9 = str5;
      return new Tuple<string, string, string, string, string>(str1, str6, str7, str8, str9);
    })).ToList<Tuple<string, string, string, string, string>>();
  }
}
