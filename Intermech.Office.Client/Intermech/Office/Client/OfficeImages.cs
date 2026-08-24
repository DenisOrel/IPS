// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeImages
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Office.Client;

public static class OfficeImages
{
  private const string ResourcesNamespace = "Intermech.Office.Client.Resources.";

  [NotNull]
  public static Image GetImage([CanBeNull] ref Image image, [NotNull] string iconName)
  {
    LazyInitializer.EnsureInitialized<Image>(ref image, (Func<Image>) (() =>
    {
      Bitmap image1 = new Bitmap(OfficeImages.GetResourceStream(iconName) ?? throw new Exception("Can`t load office resource stream: " + iconName));
      image1.MakeTransparent();
      return (Image) image1;
    }));
    return image;
  }

  [CanBeNull]
  public static Stream GetResourceStream([NotNull] string resourceName)
  {
    return OfficeImages.GetResourceStream("Intermech.Office.Client.Resources.", resourceName);
  }

  [CanBeNull]
  public static Stream GetResourceStream([NotNull] string resourcesNamespace, [NotNull] string resourceName)
  {
    return typeof (OfficeImages).Assembly.GetManifestResourceStream(resourcesNamespace + resourceName);
  }
}
