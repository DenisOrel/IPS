// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBRelationTaskInProject
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Localization;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using Intermech.Metadata;
using System;
using System.Data;

#nullable disable
namespace Intermech.Project.Server;

[DBRelationTypeHandler("cad00e93-306c-11d8-b4e9-00304f19f545")]
public class DBRelationTaskInProject([NotNull] UserSession uSession, [NotNull] DataTable relationsTable) : 
  DBRelation(uSession, relationsTable),
  IDBRelationTaskInProject,
  IDBRelation,
  IDBAttributable,
  IDBSessionable,
  IPluginsData,
  IDeletable,
  IDBGuid,
  IDBLocalizable,
  IDBLastAccessInfo
{
  public static Guid TypeGuid => Intermech.Project.RelationTypes.TaskComposition.Guid;

  [NotEmpty]
  public new static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition;
}
