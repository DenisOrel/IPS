// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.IMGProject
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Data.SectionEntities;
using Intermech.Tools.Integrators.Mechanical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal interface IMGProject : IDisposable
{
  Dictionary<string, IMGProjectItem> GetProjectItems();

  IValueBagContainer Properties { get; }

  ICollection<InitialArticleData> GetArticles(SectionEntity documentItem);

  bool IsValid();
}
