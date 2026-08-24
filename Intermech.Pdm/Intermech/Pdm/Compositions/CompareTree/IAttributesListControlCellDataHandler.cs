// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.IAttributesListControlCellDataHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.PropertyEditors;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal interface IAttributesListControlCellDataHandler
{
  void SetDataValue(
    GetCellDataEventArgs e,
    IAttributePropertyDescriberService propertyDescriberService,
    IElementInfo currentElementInfo);

  void SetBackColor(GetCellDataEventArgs e);
}
