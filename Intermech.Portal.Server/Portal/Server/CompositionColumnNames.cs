// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.CompositionColumnNames
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Kernel.Search;

#nullable disable
namespace Intermech.Portal.Server;

internal enum CompositionColumnNames
{
  [AttributeID(ObligatoryObjectAttributes.F_PRJLINK_ID), SourceType(AttributeSourceTypes.Relation)] PrjLinkID,
  [AttributeID(ObligatoryObjectAttributes.F_PRJ_GUID), SourceType(AttributeSourceTypes.Relation)] PrjGuid,
  [AttributeID("cad014d0-306c-11d8-b4e9-00304f19f545"), SourceType(AttributeSourceTypes.Relation)] RelTypeName,
  [AttributeID("cad001a9-306c-11d8-b4e9-00304f19f545"), SourceType(AttributeSourceTypes.Relation)] RelationTypeGuid,
  [AttributeID(ObligatoryObjectAttributes.F_PROJ_ID), SourceType(AttributeSourceTypes.Relation)] ProjID,
  [AttributeID("cad001c2-306c-11d8-b4e9-00304f19f545"), SourceType(AttributeSourceTypes.Relation)] VersionInRelation,
  [AttributeID(ObligatoryObjectAttributes.F_OBJECT_ID), SourceType(AttributeSourceTypes.Object)] ObjectID,
  [AttributeID(ObligatoryObjectAttributes.F_PART_ID), SourceType(AttributeSourceTypes.Relation)] PartID,
  [AttributeID("cad0156a-306c-11d8-b4e9-00304f19f545"), SourceType(AttributeSourceTypes.Object)] LinkedGuid,
  [AttributeID("cad001a0-306c-11d8-b4e9-00304f19f545"), SourceType(AttributeSourceTypes.Object)] ObjectTypeGuid,
  [AttributeID("cad014cf-306c-11d8-b4e9-00304f19f545"), SourceType(AttributeSourceTypes.Object)] ObjectTypeName,
}
