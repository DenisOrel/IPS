// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ItemFactories.IClassificatorItem
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData.ItemFactories;

internal interface IClassificatorItem
{
  string FolderKey { get; set; }

  string FolderName { get; }

  int Owner { get; set; }

  long ImageObjectID { get; set; }

  string Note { get; }

  byte[] FileBody { get; }

  string Formula { get; }

  int FolderLev { get; }

  char Notalpha { get; }

  int OrderId { get; }

  string BitmapType { get; }

  long ParentID { get; set; }

  int ObjTypeID { get; set; }

  long ObjectID { get; set; }
}
