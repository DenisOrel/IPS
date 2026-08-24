// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IImportedObjectListItems
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

public interface IImportedObjectListItems
{
  ImportingObject this[int index] { get; set; }

  void Clear();

  int Count { get; }

  void Add(ImportingObject io);

  bool UseObject(long objectID);

  bool UseObject(Guid objectGuid);

  ImportingObject[] ToArray();

  int CurrentIndex { get; }
}
