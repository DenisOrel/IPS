// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsQuery
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Queries;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ContainsQuery : DelayedQuery
{
  protected BackgroundReader reader;
  protected SearchSchemeID scheme;
  protected bool inProducts;
  protected long objectID;

  public ContainsQuery(
    INodeQuerySupport support,
    long objectID,
    SearchSchemeID scheme,
    BackgroundReader reader,
    bool inProducts,
    bool realQuery)
    : base(support, realQuery)
  {
    this.reader = reader;
    this.scheme = scheme;
    this.inProducts = inProducts;
    this.objectID = objectID;
  }

  protected override DataTable GetDataTable(RecordMapping mapping)
  {
    if (this.reader == null || !this.realQuery)
      return (DataTable) null;
    switch (this.reader.State)
    {
      case BackgroundState.Empty:
        if (this.scheme is VirtualSearchSchemeID)
        {
          if (this.scheme is VirtualSearchSchemeID scheme)
            this.reader.Execute(mapping, this.objectID, new RuntimeSearchScheme(scheme.ContainsMode == ContainsMode.Applicability ? SearchDirection.EntersTo : SearchDirection.Contains, 0L, scheme.Types.ToArray(), scheme.RelTypes.ToArray(), (AttributeSource[]) null), VersionsRuleSources.GetEditorRule().OwnerId);
        }
        else
          this.reader.Execute(mapping, this.objectID, this.scheme.SchemeID, this.inProducts, VersionsRuleSources.GetEditorRule().OwnerId);
        return (DataTable) null;
      case BackgroundState.Fill:
        return this.FilterTable(mapping, this.reader.CorrectDataTableFromMapping(mapping));
      default:
        return (DataTable) null;
    }
  }
}
