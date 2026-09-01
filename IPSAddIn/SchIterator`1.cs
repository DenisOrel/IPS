// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SchIterator`1
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using SCH;
using System;

#nullable disable
namespace CSharpPlugin;

internal abstract class SchIterator<TProxy> : IDisposable
{
  protected TObjectId itemType;
  protected ISch_Document document;
  protected ISch_Iterator iterator;

  public SchIterator(TObjectId itemType, ISch_Document document)
  {
    this.itemType = itemType;
    this.document = document;
  }

  public TProxy GetNextComponent()
  {
    bool first = false;
    if (this.iterator == null)
    {
      TObjectSet tobjectSet = new TObjectSet();
      tobjectSet.Add(this.itemType);
      TObjectSet argObjectSet = tobjectSet;
      this.iterator = this.document.SchIterator_Create();
      this.iterator.AddFilter_ObjectSet(argObjectSet);
      first = true;
    }
    return this.FetchComponent(first);
  }

  private TProxy FetchComponent(bool first)
  {
    ISch_BasicContainer component = first ? this.iterator.FirstSchObject() : this.iterator.NextSchObject();
    if (component != null)
      return this.CreateObject(component);
    this.document.SchIterator_Destroy(ref this.iterator);
    this.iterator = (ISch_Iterator) null;
    return default (TProxy);
  }

  public void Dispose()
  {
    if (this.iterator == null)
      return;
    this.document.SchIterator_Destroy(ref this.iterator);
  }

  protected abstract TProxy CreateObject(ISch_BasicContainer component);
}
