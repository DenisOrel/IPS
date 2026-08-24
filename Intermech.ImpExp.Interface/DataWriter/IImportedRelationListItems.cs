// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IImportedRelationListItems
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

public interface IImportedRelationListItems
{
  ImportingRelation this[int index] { get; set; }

  void Clear();

  int Count { get; }

  void Add(ImportingRelation ir);

  bool UseRelation(long prjLinkID);

  ImportingRelation[] ToArray();

  int CurrentIndex { get; }
}
