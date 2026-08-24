// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImportingRelationTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ImportingRelationTag : ImportingAttributableTag<ImportingRelation>
{
  public override short ClassID => 29;

  public override ImportingRelation Clone()
  {
    RelationRecord relation = this.Attributable.Relation;
    return new ImportingRelation(new RelationRecord(relation.PrjLinkId, relation.PrjLinkGuid, relation.ProjId, relation.PartId, relation.RelationType, relation.CreateDate, relation.CreatorID));
  }
}
