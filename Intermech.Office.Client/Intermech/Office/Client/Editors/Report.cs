// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.Editors.Report
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client.Editors;

internal class Report
{
  protected long _UnitID;

  public Report(long unitID) => this._UnitID = unitID;

  [NotNull]
  public virtual string Load([NotNull] IUserSession session, int index)
  {
    return this.GetValue(session, index);
  }

  public virtual void Save([NotNull] IDBObject dbResolution, int index, [NotNull] string text)
  {
    IDBAttribute attributeById = dbResolution.GetAttributeByID(OfficeConsts.AttrReportsID);
    if (attributeById != null)
    {
      if (index >= 0 && index < attributeById.ValuesCount)
      {
        attributeById.Index = index;
        attributeById.AsString = text;
      }
      else
        attributeById.AddValue((object) text);
    }
    else
      dbResolution.Attributes.AddAttribute(OfficeConsts.AttrReportsID, false).Value = (object) text;
  }

  [NotNull]
  protected string GetValue([NotNull] IUserSession session, int index)
  {
    IDBAttribute attributeById = session.GetObject(this._UnitID).GetAttributeByID(OfficeConsts.AttrReportsID);
    if (attributeById == null || attributeById.ValuesCount < index)
      return string.Empty;
    attributeById.Index = index;
    return (string) attributeById.Value;
  }
}
