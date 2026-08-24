// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.OilHelper
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class OilHelper
{
  public long FolderID { get; private set; }

  public Dictionary<string, string> Keys { get; private set; }

  public bool IsDataLoaded { get; private set; }

  public Guid NodeGuid => Consts.IMHOilHandbookNodeGuid;

  public OilHelper() => this.FolderID = 0L;

  public void LoadData(long folderID)
  {
    this.FolderID = folderID;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<long> linksEntersInFolder = ImbaseHelper.GetLinksEntersInFolder(sessionKeeper.Session, folderID);
      if (linksEntersInFolder == null || !(sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService))
        return;
      List<string> keyValues = new List<string>();
      foreach (long linkID in linksEntersInFolder)
      {
        IEnumerable<string> elements = this.GetElements(sessionKeeper.Session, customService, linkID);
        if (elements != null)
          keyValues.AddRange(elements);
      }
      this.Keys = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, keyValues);
      this.IsDataLoaded = this.Keys != null && this.Keys.Count > 0;
    }
  }

  private IEnumerable<string> GetElements(IUserSession session, IImbaseServer srv, long linkID)
  {
    IEnumerable<string> elements = (IEnumerable<string>) null;
    DataTable recordsTable;
    srv.LoadRecords(session.SessionGUID, linkID, string.Empty, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, out recordsTable, out AttributeTypeProperties[] _, out ImbaseKeyInfo _);
    if (recordsTable != null && recordsTable.Rows.Count > 0)
      elements = (IEnumerable<string>) recordsTable.AsEnumerable().Select<DataRow, string>((System.Func<DataRow, string>) (x => ImbaseHelper.MakeInternalImbaseKey(linkID, Convert.ToInt64(x["-2"]))));
    return elements;
  }
}
