// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.IncomingDocumentsNodeID
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Navigator.Interfaces;
using System.Diagnostics;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class IncomingDocumentsNodeID : INodeID
{
  private readonly long _unitID;

  public IncomingDocumentsNodeID([NotEmpty] long unitID) => this._unitID = unitID;

  public int CategoryID
  {
    [DebuggerStepThrough] get => OfficeClientConsts.CategoryIncomingDocuments;
  }

  public int TypeID => 0;

  [CanBeNull]
  public object Cookie { get; set; }

  public override bool Equals(object obj)
  {
    return obj == null || obj.GetType() != typeof (IncomingDocumentsNodeID) ? base.Equals(obj) : this._unitID == ((IncomingDocumentsNodeID) obj)._unitID;
  }

  public override int GetHashCode() => this._unitID.GetHashCode();
}
