// Decompiled with JetBrains decompiler
// Type: SolidWorksTools.File.BitmapHandler
// Assembly: SolidWorksTools, Version=2.0.0.0, Culture=neutral, PublicKeyToken=bd18593873b4686d
// MVID: 863FC724-66C1-47FF-B7E4-FE091B230BC6
// Assembly location: D:\Projects\IPS Code\IPS\CADSystem\CAD\SolidWorks\Bin\solidworkstools.dll

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;

#nullable disable
namespace SolidWorksTools.File;

public class BitmapHandler : IDisposable
{
  private ArrayList files;

  public BitmapHandler() => this.files = new ArrayList();

  public void Dispose() => this.CleanFiles();

  public string CreateFileFromResourceBitmap(string bitmapName, Assembly callingAssy)
  {
    string tempFileName = Path.GetTempFileName();
    Stream manifestResourceStream;
    Bitmap bitmap;
    try
    {
      manifestResourceStream = callingAssy.GetManifestResourceStream(bitmapName);
      bitmap = new Bitmap(manifestResourceStream);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return "";
    }
    try
    {
      bitmap.Save(tempFileName);
      this.files.Add((object) tempFileName);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return "";
    }
    finally
    {
      bitmap.Dispose();
      manifestResourceStream.Close();
    }
    return tempFileName;
  }

  public bool CleanFiles()
  {
    foreach (string file in this.files)
    {
      try
      {
        System.IO.File.Delete(file);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
    this.files.Clear();
    this.files = (ArrayList) null;
    return true;
  }
}
