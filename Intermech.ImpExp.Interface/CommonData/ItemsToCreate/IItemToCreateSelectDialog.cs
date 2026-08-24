// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.IItemToCreateSelectDialog
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

public interface IItemToCreateSelectDialog
{
  DialogResult ShowDialog();

  Guid SelectedItemGUID { get; set; }

  string SelectedItemName { get; }
}
