// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.SpecialAttributesService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using MGCPCB;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class SpecialAttributesService : IDisposable
{
  private ExPCBPartEditor _partEditor;
  private List<ISpecialAttributeHandler> _handlers;

  public SpecialAttributesService(ExPCBPartEditor partEditor)
  {
    this._partEditor = partEditor;
    this.DoInitialize();
  }

  private void DoInitialize()
  {
    this._handlers = new List<ISpecialAttributeHandler>();
    this._handlers.Add((ISpecialAttributeHandler) new PartNumberHandler());
    this._handlers.Add((ISpecialAttributeHandler) new RefDesHandler());
    this._handlers.Add((ISpecialAttributeHandler) new DescriptionHandler(this._partEditor));
  }

  public string ReadCustomValue(Component component, string attributeName)
  {
    return this._partEditor.GetPropertyValue(component.PartNumber, attributeName);
  }

  public bool ReadValue(Component component, string attributeName, out string value)
  {
    ISpecialAttributeHandler attributeHandler = this._handlers.Find((Predicate<ISpecialAttributeHandler>) (x => x.AttributeName.Equals(attributeName.ToUpper())));
    if (attributeHandler != null)
    {
      value = attributeHandler.ReadValue(component);
      return true;
    }
    value = (string) null;
    return false;
  }

  public bool WriteValue(Component component, string attributeName, string value)
  {
    ISpecialAttributeHandler attributeHandler = this._handlers.Find((Predicate<ISpecialAttributeHandler>) (x => x.AttributeName.Equals(attributeName.ToUpper())));
    if (attributeHandler == null)
      return false;
    attributeHandler.WriteValue(component, value);
    return true;
  }

  public void Dispose()
  {
    this._handlers.Clear();
    if (this._partEditor == null)
      return;
    this._partEditor.Dispose();
  }
}
