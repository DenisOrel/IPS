// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXDocument
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.CADInterface.Proxies;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXDocument(ICADDocumentProvider docProvider, CADSystemProxy appProxy) : 
  CADDocumentProxy(docProvider, appProxy)
{
  private static readonly string impossibleValue = new string(char.MinValue, 1);

  protected override string DetectMasterFile()
  {
    string path = base.DetectMasterFile();
    if (string.IsNullOrEmpty(path) || !File.Exists(path))
      path = this.FullName;
    return path;
  }

  protected override bool DetectHasConfigurations()
  {
    try
    {
      return this.GetDefaultConfiguration() != null;
    }
    catch (NotSupportedException ex)
    {
      return false;
    }
    catch (NotImplementedException ex)
    {
      return false;
    }
    catch (COMException ex)
    {
      if (ex.ErrorCode == -2147467259 /*0x80004005*/)
        return false;
      throw;
    }
  }

  protected override void DoForceLoad()
  {
    if (this.CADSystem.GetDocumentOpenStatus(this.FullName) == CADDocumentOpenStatus.NotOpen && (object.Equals((object) this.MasterFile, (object) NXDocument.impossibleValue) || object.Equals((object) this.GetDefaultConfiguration(), (object) NXDocument.impossibleValue)))
      throw new InvalidOperationException();
  }
}
