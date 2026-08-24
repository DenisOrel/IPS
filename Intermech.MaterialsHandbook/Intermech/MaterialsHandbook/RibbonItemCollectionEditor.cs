// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.RibbonItemCollectionEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class RibbonItemCollectionEditor : CollectionEditor
{
  public RibbonItemCollectionEditor()
    : base(typeof (RibbonItemCollection))
  {
  }

  protected override Type CreateCollectionItemType() => typeof (RibbonButton);

  protected override Type[] CreateNewItemTypes()
  {
    return new Type[1]{ typeof (RibbonButton) };
  }
}
