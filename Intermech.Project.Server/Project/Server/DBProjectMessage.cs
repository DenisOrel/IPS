// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBProjectMessage
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Metadata;
using Intermech.Workflow.Server;
using System;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cadd91f6-306c-11d8-b4e9-00304f19f545", true)]
public class DBProjectMessage([NotNull] UserSession uSession, [NotNull] DataTable objectsTable) : 
  DBMessage(uSession, objectsTable),
  IDBProjectMessage,
  IDBObject,
  IDBAttributable,
  IDBSessionable,
  IPluginsData
{
  public static int AttrTaskID = Intermech.Workflow.Attributes.Activity.ID;

  public static Guid TypeGuid => Intermech.Project.ObjectTypes.ProjectMessage.Guid;

  [NotEmpty]
  public new static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.ProjectMessage;

  [NotEmpty]
  public long TaskID
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.GetAttrSureObjLinkValue(DBProjectMessage.AttrTaskID);
    }
  }

  [NotNull]
  internal DBProjectTask Task
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.Session.GetServerTask(this.TaskID);
    }
  }

  [NotNull]
  IDBProjectTask IDBProjectMessage.Task => this.Session.GetTask(this.TaskID);
}
