// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.SpecialDocumentTypes
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Search;

internal class SpecialDocumentTypes
{
  private List<SpecialDocumentTypes.SpecialDocumentType> _docTypes;

  public SpecialDocumentTypes()
  {
    this._docTypes = new List<SpecialDocumentTypes.SpecialDocumentType>();
    this.AddNewSpecialType("техпроцесс", new Guid("cad00187-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Габаритный чертеж (AutoCAD)", new Guid("cad00746-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Габаритный чертеж (ProE)", new Guid("cad00748-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Габаритный чертеж (SolidWorks)", new Guid("cad0074a-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Монтажный чертеж (SolidWorks)", new Guid("cad00752-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Чертеж детали (AutoCAD)", new Guid("cad00900-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Чертеж детали (PDF)", new Guid("cadd96c6-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Чертеж общего вида (SolidWorks)", new Guid("cad00765-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Электромонтажный чертеж (AutoCAD)", new Guid("cad00789-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Электромонтажный чертеж (SolidWorks)", new Guid("cad0078d-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Комплект извещений", new Guid("cadd9522-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Модель детали Unigraphics", new Guid("cad00794-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Модель детали Компас", new Guid("cadd93e2-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Сборочный чертеж ProE", new Guid("cad00903-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Сборочный чертеж Unigraphics", new Guid("cad00906-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Чертеж Unigraphics", new Guid("cad0090d-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("Документ Winword", new Guid("cad008f3-306c-11d8-b4e9-00304f19f545"));
    this.AddNewSpecialType("XLS", new Guid("cad008f2-306c-11d8-b4e9-00304f19f545"));
  }

  private void AddNewSpecialType(string compareString, Guid objectTypeGuid)
  {
    this._docTypes.Add(new SpecialDocumentTypes.SpecialDocumentType(compareString.ToLower(), objectTypeGuid));
  }

  public Guid Find(string compareString)
  {
    if (string.IsNullOrEmpty(compareString))
      return Guid.Empty;
    SpecialDocumentTypes.SpecialDocumentType specialDocumentType = this._docTypes.Find((Predicate<SpecialDocumentTypes.SpecialDocumentType>) (x => x.CompareString.Equals(compareString.ToLower())));
    return specialDocumentType == null ? Guid.Empty : specialDocumentType.ObjectTypeGuid;
  }

  private class SpecialDocumentType
  {
    public string CompareString { get; private set; }

    public Guid ObjectTypeGuid { get; private set; }

    public SpecialDocumentType(string compareString, Guid objectTypeGuid)
    {
      this.CompareString = compareString;
      this.ObjectTypeGuid = objectTypeGuid;
    }
  }
}
