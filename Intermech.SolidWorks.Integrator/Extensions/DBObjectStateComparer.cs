// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Extensions.DBObjectStateComparer
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.Files;
using System.Collections.Generic;

#nullable disable
namespace Intermech.SolidWorks.Integrator.Extensions;

internal sealed class DBObjectStateComparer : IEqualityComparer<DBObjectState>
{
  public bool Equals(DBObjectState x, DBObjectState y) => x.ObjectId == y.ObjectId;

  public int GetHashCode(DBObjectState obj) => obj.ObjectId.GetHashCode();
}
