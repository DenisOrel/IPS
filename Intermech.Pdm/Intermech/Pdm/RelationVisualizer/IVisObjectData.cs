// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.IVisObjectData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public interface IVisObjectData
{
  long ObjVerId { get; set; }

  int ObjTypeId { get; set; }

  int LCLevelId { get; set; }

  string Caption { get; set; }

  List<VisStatus> StatusList { get; set; }

  void Init(IUserSession ius);
}
