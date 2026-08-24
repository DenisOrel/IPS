// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CustomMultipleResolutionsDescriptor`1
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Intermech.Office.Client;

internal class CustomMultipleResolutionsDescriptor<TResolutionDescriptor> : 
  MultipleObjectsDescriptor,
  IDescriptor,
  INodeItems,
  IPersistable
  where TResolutionDescriptor : IDescriptor
{
  [CanBeNull]
  private static Image _imageMultipleResolutionsIcon;

  [NotNull]
  public static Image MultipleResolutionsIcon
  {
    get
    {
      return OfficeImages.GetImage(ref CustomMultipleResolutionsDescriptor<TResolutionDescriptor>._imageMultipleResolutionsIcon, "Resolutions.png");
    }
  }

  public CustomMultipleResolutionsDescriptor([NotNull] string caption, [NotNull] IEnumerable<long> resolutionIDs)
    : base(caption, (IEnumerable<IDescriptor>) null, CustomMultipleResolutionsDescriptor<TResolutionDescriptor>.MultipleResolutionsIcon)
  {
    this._descriptors = new DescriptorCollection(this.CreateChildDescriptors(resolutionIDs));
  }

  public CustomMultipleResolutionsDescriptor([NotNull] IEnumerable<long> resolutionIDs)
    : this(string.Empty, resolutionIDs)
  {
  }

  [NotNull]
  protected virtual IEnumerable<IDescriptor> CreateChildDescriptors([NotNull] IEnumerable<long> resolutionIDs)
  {
    return resolutionIDs.Select<long, IDescriptor>(new Func<long, IDescriptor>(this.CreateChildDescriptor));
  }

  [NotNull]
  protected virtual IDescriptor CreateChildDescriptor(long resolutionID)
  {
    return (IDescriptor) Intermech.Diagnostics.Check.NotNull<ConstructorInfo>(typeof (TResolutionDescriptor).GetConstructor(new Type[1]
    {
      typeof (long)
    }), "ctor").Invoke(new object[1]
    {
      (object) resolutionID
    });
  }
}
