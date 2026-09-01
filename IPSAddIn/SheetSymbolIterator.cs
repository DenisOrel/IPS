// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SheetSymbolIterator
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using SCH;

#nullable disable
namespace CSharpPlugin;

internal sealed class SheetSymbolIterator(ISch_Document document) : SchIterator<ISchSheetSymbol>(TObjectId.eSheetSymbol, document)
{
  protected override ISchSheetSymbol CreateObject(ISch_BasicContainer component)
  {
    return (ISchSheetSymbol) new SheetSymbol(component as ISch_SheetSymbol);
  }
}
