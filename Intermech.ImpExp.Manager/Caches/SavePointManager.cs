// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.Caches.SavePointManager
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Manager.Caches;

internal class SavePointManager : ISavePoint
{
  private string _fileFullName = string.Empty;

  public bool IsResumeMode(SavePoint point)
  {
    if (point == null)
      return false;
    if (point.OperationTerminateType == TerminateType.Complete)
      return true;
    return point.OperationTerminateType == TerminateType.Pump && point.RePumpMode;
  }

  public SavePointManager()
  {
    this._fileFullName = Path.Combine(CacheHelper.CacheFolder, "spimpexp.dat");
  }

  public SavePoint GetSavePoint()
  {
    SavePoint savePoint = new SavePoint();
    if (!File.Exists(this._fileFullName))
      return (SavePoint) null;
    XmlDocument saved = new XmlDocument();
    saved.Load(this._fileFullName);
    savePoint.Load(saved);
    return savePoint;
  }

  public void SetSavePoint(SavePoint point) => point.Save().Save(this._fileFullName);

  public void RemoveSavePoint()
  {
    if (!File.Exists(this._fileFullName))
      return;
    File.Delete(this._fileFullName);
  }
}
