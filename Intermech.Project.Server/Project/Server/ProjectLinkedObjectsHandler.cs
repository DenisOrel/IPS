// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectLinkedObjectsHandler
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel.Search;
using Intermech.Metadata;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Project.Server;

internal sealed class ProjectLinkedObjectsHandler : LinkedObjectsHandler, ILinkedObjectsHandler
{
  [NotNull]
  public List<int> HandleTypes { get; } = new List<int>()
  {
    (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task
  };

  [NotNull]
  public List<int> OutputTypes { get; } = new List<int>()
  {
    (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency
  };

  [NotNull]
  public List<LinkedObject> Handle(
    [NotNull] IUserSession session,
    [NotEmpty] long objectID,
    int objectType,
    [CanBeNull] string filtrationOwnerID)
  {
    List<LinkedObject> linkedObjectList = new List<LinkedObject>();
    DataTable dataTable = session.GetObjectCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency).Select(new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FromTask, RelationalOperators.Equal, (object) objectID, LogicalOperators.OR, 1, false),
      new ConditionStructure((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ToTask, RelationalOperators.Equal, (object) objectID, LogicalOperators.AND, -1, false)
    }, new object[1]{ (object) -2 }));
    if (dataTable != null && dataTable.Rows.Count > 0)
      linkedObjectList.AddRange((IEnumerable<LinkedObject>) dataTable.Select<LinkedObject>((System.Func<DataRow, LinkedObject>) (row => new LinkedObject(Convert.ToInt64(row[0])))));
    return linkedObjectList;
  }

  [NotNull]
  [NotWhitespace]
  public string Name => "Модуль ImProject";

  protected override void OnReloadTypes()
  {
  }

  bool ILinkedObjectsHandler.IsTypesChanged(IUserSession session) => this.IsTypesChanged(session);

  void ILinkedObjectsHandler.UpdateHandleAndOutputTypes(IUserSession session, bool force)
  {
    this.UpdateHandleAndOutputTypes(session, force);
  }
}
