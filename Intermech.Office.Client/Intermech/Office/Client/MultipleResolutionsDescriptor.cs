// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.MultipleResolutionsDescriptor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Office.Client;

internal class MultipleResolutionsDescriptor([NotNull] string caption, [NotNull] IEnumerable<long> resolutionIDs) : 
  CustomMultipleResolutionsDescriptor<Descriptor>(caption, resolutionIDs),
  IDescriptor,
  INodeItems,
  IPersistable
{
  public MultipleResolutionsDescriptor([NotNull] IEnumerable<long> resolutionIDs)
    : this(string.Empty, resolutionIDs)
  {
  }
}
