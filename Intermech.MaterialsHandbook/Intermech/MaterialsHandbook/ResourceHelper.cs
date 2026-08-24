// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ResourceHelper
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class ResourceHelper
{
  internal static T GetResourceData<T>(Assembly assembly, string resStr) where T : IDisposable
  {
    T resourceData = default (T);
    Stream manifestResourceStream = assembly.GetManifestResourceStream(resStr);
    try
    {
      object instance = Activator.CreateInstance(typeof (T), (object) manifestResourceStream);
      if (instance != null)
        resourceData = (T) instance;
    }
    finally
    {
      if (typeof (T) == typeof (Icon))
        manifestResourceStream.Close();
    }
    return resourceData;
  }
}
