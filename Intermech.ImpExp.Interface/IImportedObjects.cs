// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IImportedObjects
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс на импротированные объекты</summary>
public interface IImportedObjects
{
  long GetID(long objectID);

  Guid GetObjectGUID(long objectID);

  Guid GetGUID(long objectID);

  int GetObjectTypeID(long objectID);

  int GetObjectTypeIDForID(long id);

  DictionaryValue GetInfo(long objectID);

  void AddValue(long objectID, long id, int objectTypeID, Guid objectGuid, Guid guid);

  System.Collections.Generic.Dictionary<object, DictionaryValue> Dictionary { get; }
}
