// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ContextDropDownControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ContextDropDownControl : ObjectsDropDownControl
{
  public ContextDropDownControl(
    ButtonItem buttonEditingContextsBrowse,
    DropDownMenuItem menu,
    Image image,
    IList<long> objectIDs,
    long selectedItem)
    : base(menu, ObjectsDropDownOptions.Default, "Текущий контекст редактирования", image, new Intermech.Interfaces.MyObjectElement(0L, "Контекст редактирования не выбран", (object) null, MetaDataHelper.GetObjectTypeID("cad0146b-306c-11d8-b4e9-00304f19f545")), objectIDs, (IList<int>) MetaDataHelper.GetSpecialGroupingIDs(), selectedItem)
  {
    buttonEditingContextsBrowse.ImageIndex = this.namedImageList.ImageIndex("imgEditingContextsBrowse");
    buttonEditingContextsBrowse.Click += new EventHandler(this.DoContextBrowse);
  }

  private void DoContextBrowse(object sender, EventArgs e)
  {
    DescriptorCollection descriptors = new DescriptorCollection();
    List<int> contextTopObjectsIds = MetaDataHelper.GetEditingContextTopObjectsIDs();
    for (int index = 0; index < contextTopObjectsIds.Count; ++index)
      descriptors.Add((IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(contextTopObjectsIds[index]));
    long[] numArray = SelectionWindow.SelectObjects("Выберите контекст редактирования", string.Empty, (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Контексты редактирования ", descriptors), SelectionOptions.Default | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect);
    if (numArray == null || numArray.Length == 0)
      return;
    this.AlterObject(numArray[0], true, true, true);
  }
}
