// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIStandardPartLibraryService
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIStandardPartLibraryService(
  IIntegrator owner,
  StandardLibraryMode mode,
  string folderName) : CADStandardPartLibraryService(owner, mode, folderName)
{
  protected override bool OnCanImportCustomParts() => true;

  protected override bool OnIsCustomPartArticle(ValueBag articleAttributes)
  {
    string strA = articleAttributes.Read<string>((StringKey) CADVirtualAttributes.ArticleSection, (string) null);
    string str = articleAttributes.Read<string>((StringKey) IDCache.Default.Name.Text, (string) null);
    return !string.IsNullOrEmpty(strA) && string.Compare(strA, IDCache.Default.StandardArticles.Text, StringComparison.CurrentCultureIgnoreCase) == 0 && !string.IsNullOrEmpty(str) || base.OnIsCustomPartArticle(articleAttributes);
  }

  protected override void DoPrepareCustomPartArticleToImport(ValueBag articleAttributes)
  {
    base.DoPrepareCustomPartArticleToImport(articleAttributes);
    string newValue1 = articleAttributes.Read<string>((StringKey) IDCache.Default.Designation.Text, (string) null);
    if (string.IsNullOrEmpty(newValue1))
      return;
    string newValue2 = articleAttributes.Read<string>((StringKey) IDCache.Default.Name.Text, (string) null);
    articleAttributes.Update((StringKey) IDCache.Default.Designation.Text, (object) string.Empty);
    articleAttributes.Update((StringKey) IDCache.Default.Name.Text, (object) newValue1);
    articleAttributes.Update((StringKey) "Наименование_0", (object) newValue2);
  }
}
