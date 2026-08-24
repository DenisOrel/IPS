// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.ExistsExemplar
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class ExistsExemplar
{
  public long InstanceID;
  public int InstanceTypeID = -1;
  public string SerialNo = string.Empty;
  public string Name = string.Empty;
  public string Designation = string.Empty;

  public ExistsExemplar(
    long instanceID,
    int instanceTypeID,
    string serialNo,
    string name,
    string designation)
  {
    this.InstanceID = instanceID;
    this.InstanceTypeID = instanceTypeID;
    this.SerialNo = serialNo;
    this.Name = name;
    this.Designation = designation;
  }
}
