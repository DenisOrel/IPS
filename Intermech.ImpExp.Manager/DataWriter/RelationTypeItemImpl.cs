// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.RelationTypeItemImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class RelationTypeItemImpl(
  IDataWriterProxy dataWriter,
  int id,
  Guid guid,
  string name) : AttributableTypeItem(dataWriter, id, guid, name), IRelationTypeItem, ITypeItem, IAttributableTypeItem
{
}
