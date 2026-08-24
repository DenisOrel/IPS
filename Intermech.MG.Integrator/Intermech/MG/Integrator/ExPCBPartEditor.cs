// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBPartEditor
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using MGCPCB;
using MGCPCBPartsEditor;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBPartEditor : IDisposable
{
  private PartsDB _dbParts;
  private List<PCBPart> _parts;
  private IMGCPDBPartsEditorDlg _parteditor;
  private bool _changed;

  public void OpenDB(Document pcbDoc)
  {
    // ISSUE: reference to a compiler-generated field
    if (ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, IMGCPDBPartsEditorDlg>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IMGCPDBPartsEditorDlg), typeof (ExPCBPartEditor)));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this._parteditor = ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__0, pcbDoc.PartEditor);
    this._dbParts = this._parteditor != null ? this._parteditor.ActiveDatabaseEx : throw new Exception($"Для документа {pcbDoc.Path} не удалось получить PartEditor");
    this._parts = new List<PCBPart>();
    if (this._dbParts == null)
      return;
    // ISSUE: reference to a compiler-generated method
    foreach (object obj1 in (IMGCPDBPartitions) this._dbParts.get_Partitions())
    {
      // ISSUE: reference to a compiler-generated field
      if (ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (ExPCBPartEditor)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, IEnumerable> target = ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, IEnumerable>> p3 = ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parts", (IEnumerable<Type>) null, typeof (ExPCBPartEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__1.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__1, obj1);
      foreach (object obj3 in target((CallSite) p3, obj2))
      {
        List<PCBPart> parts = this._parts;
        // ISSUE: reference to a compiler-generated field
        if (ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, Type, object, PCBPart>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (ExPCBPartEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PCBPart pcbPart = ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__2.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__4.\u003C\u003Ep__2, typeof (PCBPart), obj3);
        parts.Add(pcbPart);
      }
    }
  }

  public string GetPropertyValue(string partNumber, string propertyName)
  {
    PCBPart pcbPart = this._parts.Find((Predicate<PCBPart>) (x => x.Number.Equals(partNumber)));
    if (pcbPart == null)
      return (string) null;
    // ISSUE: reference to a compiler-generated field
    if (ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, IMGCPDBPart>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (IMGCPDBPart), typeof (ExPCBPartEditor)));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    foreach (IMGCPDBProperty property in (IMGCPDBProperties) ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__0, pcbPart.Instance).Properties)
    {
      try
      {
        if (property.Name.Equals(propertyName))
        {
          // ISSUE: reference to a compiler-generated field
          if (ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ExPCBPartEditor)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string> target = ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string>> p2 = ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (ExPCBPartEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          object obj = ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__1.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__5.\u003C\u003Ep__1, typeof (Convert), property.get_Value());
          return target((CallSite) p2, obj);
        }
      }
      finally
      {
        Marshal.FinalReleaseComObject((object) property);
      }
    }
    return (string) null;
  }

  public string GetPartDescription(string partNumber)
  {
    return this._parts.Find((Predicate<PCBPart>) (x => x.Number.Equals(partNumber)))?.Description;
  }

  public bool SetPartDescription(string partNumber, string partDescription)
  {
    PCBPart pcbPart = this._parts.Find((Predicate<PCBPart>) (x => x.Number.Equals(partNumber)));
    if (pcbPart == null)
      return false;
    if (pcbPart.Description != partDescription)
    {
      pcbPart.Description = partDescription;
      // ISSUE: reference to a compiler-generated field
      if (ExPCBPartEditor.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExPCBPartEditor.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Description", typeof (ExPCBPartEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ExPCBPartEditor.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__7.\u003C\u003Ep__0, pcbPart.Instance, partDescription);
      this._changed = true;
    }
    return true;
  }

  public void Dispose()
  {
    if (this._parts != null)
    {
      foreach (PCBPart part in this._parts)
      {
        // ISSUE: reference to a compiler-generated field
        if (ExPCBPartEditor.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExPCBPartEditor.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Action<CallSite, Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", (IEnumerable<Type>) null, typeof (ExPCBPartEditor), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ExPCBPartEditor.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) ExPCBPartEditor.\u003C\u003Eo__8.\u003C\u003Ep__0, typeof (Marshal), part.Instance);
      }
      this._parts = (List<PCBPart>) null;
    }
    if (this._dbParts != null)
      Marshal.FinalReleaseComObject((object) this._dbParts);
    if (this._parteditor == null)
      return;
    if (this._changed)
    {
      // ISSUE: reference to a compiler-generated method
      this._parteditor.SaveActiveDatabase();
    }
    // ISSUE: reference to a compiler-generated method
    this._parteditor.Quit();
    Marshal.FinalReleaseComObject((object) this._parteditor);
  }
}
