// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IncomingDocumentsNode
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Selections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class IncomingDocumentsNode(long unitID, [NotNull] List<long> objectsIDs) : 
  ObjectsListNode((IList) objectsIDs)
{
  [NotNull]
  protected override List<PartSlot> CreateFolderSlots()
  {
    return new List<PartSlot>()
    {
      new PartSlot(Intermech.Navigator.Selections.Consts.SelectionsPartGuid, (INodePart) new DescriptorsPart(new DescriptorCollection()
      {
        {
          Intermech.Navigator.Selections.Consts.SelectionsDescriptorGuid,
          (IDescriptor) new HiveDescriptor(Intermech.Navigator.Selections.Consts.SelectionTypeID, (ITopBinding) new IncomingDocumentsBinding(this.Services, this.objectIDs.Cast<long>().ToArray<long>()))
        }
      }, false))
    };
  }

  public override NodeColumnCollection GetDefaultColumns(ContentType content)
  {
    if ((content & ContentType.NonFolders) <= ContentType.None)
      return base.GetDefaultColumns(content);
    NodeColumnCollection columns = new NodeColumnCollection();
    Helper.AddObligatoryColumns(columns, true, false);
    return columns;
  }

  public override INodeQuery GetQuery(ContentType content)
  {
    return (content & ContentType.Folders) > ContentType.None && (content & ContentType.NonFolders) > ContentType.None ? base.GetQuery(ContentType.NonFolders) : base.GetQuery(content);
  }
}
