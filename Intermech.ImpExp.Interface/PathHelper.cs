// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PathHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

public static class PathHelper
{
  /// <summary>Преобразование относительного пути в абсолютный</summary>
  /// <param name="path"></param>
  /// <returns></returns>
  public static string Normalize(string path)
  {
    return path == null || path.Contains(":") ? path : Path.Combine(Application.StartupPath, path);
  }
}
