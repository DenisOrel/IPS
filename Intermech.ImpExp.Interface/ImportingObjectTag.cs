// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImportingObjectTag
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.Interface;

public sealed class ImportingObjectTag : ImportingAttributableTag<ImportingObject>
{
  public override short ClassID => 28;

  public ImportingObjectTag()
  {
  }

  public ImportingObjectTag(ImportingObject importingObject)
    : base(importingObject)
  {
  }

  public override ImportingObject Clone()
  {
    ObjectRecord objectRecord = this.Attributable.Object;
    ImportingObject importingObject = new ImportingObject(new ObjectRecord(objectRecord.Object_id, objectRecord.ObjectGuid, objectRecord.Id, objectRecord.IdGuid, objectRecord.Lc_step, objectRecord.VersionId, objectRecord.ChkoutBy, objectRecord.ChkoutGuid, objectRecord.ObjectVerType, objectRecord.ObjectType, objectRecord.OwnerId, objectRecord.OwnerGuid, objectRecord.ModifyDate, objectRecord.LevelId, objectRecord.ObjCreate, objectRecord.Caption, objectRecord.ProjectId, objectRecord.ProjectGuid, objectRecord.AccessLevel));
    foreach (AttributeRecord attribute in this.Attributable.Attributes)
      importingObject.Attributes.Add((AttributeRecord) attribute.Clone());
    return importingObject;
  }
}
