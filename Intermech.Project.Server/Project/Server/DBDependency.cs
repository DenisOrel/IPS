// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBDependency
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Metadata;
using System;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cad00e9a-306c-11d8-b4e9-00304f19f545", true)]
public class DBDependency([NotNull] UserSession uSession, [NotNull] DataTable objectsTable) : 
  DBObject(uSession, objectsTable),
  IDBDependency,
  IDBObject,
  IDBAttributable,
  IDBSessionable,
  IPluginsData
{
  public static Guid TypeGuid => Intermech.Project.ObjectTypes.Dependency.Guid;

  [NotEmpty]
  public new static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency;

  public long ToTaskID
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.GetAttrSureObjLinkValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ToTask);
    }
  }

  public long FromTaskID
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.GetAttrSureObjLinkValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FromTask);
    }
  }
}
