// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Documents.ReportDocumentSyncStrategy
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Diagnostics;
using Intermech.Interfaces.Expert;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Reports.Documents;

/// <summary>Стратегия синхронизации документов</summary>
internal class ReportDocumentSyncStrategy
{
  /// <summary>
  /// 
  /// </summary>
  private readonly IDictionary<IComparable, object> _syncObjects = (IDictionary<IComparable, object>) new Dictionary<IComparable, object>();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="docRecord"></param>
  /// <returns></returns>
  private IComparable CreateSyncKey(DocRecord docRecord)
  {
    return (IComparable) new Tuple<long, long>(docRecord.objID, docRecord.scriptID);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="docRecord"></param>
  /// <returns></returns>
  public object CreateSyncObject([NotNull] DocRecord docRecord)
  {
    IComparable syncKey = this.CreateSyncKey(docRecord);
    lock (this._syncObjects)
    {
      object syncObject;
      if (this._syncObjects.TryGetValue(syncKey, out syncObject))
        return syncObject;
      syncObject = new object();
      this._syncObjects[syncKey] = syncObject;
      return syncObject;
    }
  }
}
