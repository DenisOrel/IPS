// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.IImBlobsItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal interface IImBlobsItem
{
  int Key { get; }

  BlobType BlobType { get; set; }

  int Used { get; }

  string Source { get; }

  int Hash { get; }

  string TmpFileName { get; set; }

  bool IsZipped { get; }

  long ObjectID { get; set; }

  long FileSize { get; set; }

  void UnpackTempFile(Encoding encoding);
}
