// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionsView
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;

#nullable disable
namespace Intermech.Office.Client;

internal class ResolutionsView : ChildrenView
{
  public ResolutionsView() => this.ImageIndex = Holder.NamedList.ImageIndex("imgDocDetails");

  public override ContentType ViewContentType => ContentType.NonFolders;

  [NotNull]
  public override string Caption => Localization.GetString("Office.Client_16");

  public override int ImageIndex { get; }

  public override int OrderID => 19;

  public override void Activate(IView previousView)
  {
    if (this.Node is OfficeDocNode node)
      node.Resolutions = true;
    base.Activate(previousView);
  }

  public override void Deactivate(IView nextView)
  {
    if (this.Node is OfficeDocNode node)
      node.Resolutions = false;
    base.Deactivate(nextView);
  }
}
