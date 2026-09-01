// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.PhysicalDocument
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using System;

#nullable disable
namespace CSharpPlugin;

internal class PhysicalDocument : LongLifeObject, IPhysicalDocument, IDisposable
{
  private readonly IDocument _document;
  private readonly SchemaDocumentInfo _schDoc;
  private readonly IPSAddInProxy _proxy;
  private bool needOpen = true;

  public PhysicalDocument(IDocument document, IPSAddInProxy proxy, SchemaDocumentInfo schDoc)
  {
    this._document = document;
    this._proxy = proxy;
    this._schDoc = schDoc;
  }

  public string RoomName => this._document.DM_PhysicalRoomName();

  public int ComponentsCount => this._document.DM_ComponentCount();

  public ISchComponent ReadComponent(int index)
  {
    return (ISchComponent) new PhysicalComponent(this._document.DM_Components(index), this);
  }

  internal void InvokeSchDocument(Action<SchDocument> action)
  {
    if (this._schDoc == null)
      return;
    using (ISchDocument schDocument = this._proxy.GetSchDocument(this._schDoc.FullPath, this.needOpen))
      action(schDocument as SchDocument);
    if (!this.needOpen)
      return;
    this.needOpen = false;
  }

  public void Dispose()
  {
  }
}
