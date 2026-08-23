// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetOptions
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Checksums;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

public class CertSheetOptions
{
  public bool ProcessCertSheets = true;
  public List<long> ObjectIDList = new List<long>();
  public ChecksumAlgorithm ChecksumAlgorithm;
  public bool NormalFilesMode;
  public bool AuthFilesMode;
  public bool ExpandECO;
  public bool ExpandComposition;
  public bool SaveToStandaloneFolder;
  public List<object[]> Graphs = new List<object[]>();
  public List<object[]> EmptyGraphs = new List<object[]>();
  public List<string> Extensions = new List<string>();

  public CertSheetOptions() => this.Clear();

  public void Clear()
  {
    this.ProcessCertSheets = true;
    this.ObjectIDList.Clear();
    this.ChecksumAlgorithm = ChecksumAlgorithm.Crc32;
    this.NormalFilesMode = false;
    this.AuthFilesMode = false;
    this.ExpandECO = false;
    this.ExpandComposition = false;
    this.SaveToStandaloneFolder = false;
    this.Graphs.Clear();
    this.EmptyGraphs.Clear();
    this.Extensions.Clear();
  }

  public bool GraphEnabled(string graphID) => this.CustomGraphEnabled(graphID, this.Graphs);

  public bool EmptyGraphEnabled(string graphID)
  {
    return this.CustomGraphEnabled(graphID, this.EmptyGraphs);
  }

  private bool CustomGraphEnabled(string graphID, List<object[]> lGraphs)
  {
    bool flag = false;
    for (int index = 0; index < lGraphs.Count; ++index)
    {
      if (lGraphs[index][0].Equals((object) graphID))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
