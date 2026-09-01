// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.ComponentIterator
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using SCH;

#nullable disable
namespace CSharpPlugin;

internal sealed class ComponentIterator : SchIterator<ISchComponent>
{
  private readonly string _schemaFileName;

  public ComponentIterator(ISch_Document document, string schemaFileName)
    : base(TObjectId.eSchComponent, document)
  {
    this._schemaFileName = schemaFileName;
  }

  protected override ISchComponent CreateObject(ISch_BasicContainer component)
  {
    return (ISchComponent) new SchComponent(component as ISch_Component, this._schemaFileName);
  }
}
