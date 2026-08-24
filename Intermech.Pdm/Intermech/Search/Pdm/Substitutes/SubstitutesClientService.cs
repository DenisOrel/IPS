// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Substitutes.SubstitutesClientService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Substitutes;

public sealed class SubstitutesClientService : ISubstitutesClientService
{
  public void ActualizeSubstitute(long relationID)
  {
    if (RelationHelper.IsUnknownRelationID(relationID))
      throw new ArgumentException();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session, (object) this))
        (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesServerService)) as ISubstitutesServerService).ActualizeSubstitute(sessionKeeper.Session.SessionGUID, relationID);
    }
  }

  public void SaveSubstitutes(SaveSubstitutesParams @params)
  {
    if (@params == null)
      throw new ArgumentNullException("@params");
    if (!SaveSubstitutesParams.Check(@params))
      throw new ArgumentException();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session, (object) this))
        (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesServerService)) as ISubstitutesServerService).SaveSubstitutes(sessionKeeper.Session.SessionGUID, @params);
    }
  }

  public void RemoveSubstitutes(long projectVersionID, int relationTypeID)
  {
    if (ObjectHelper.IsUnknownObjectVersionID(projectVersionID))
      throw new ArgumentException();
    RemoveSubstitutesParams @params = relationTypeID != -1 && SubstitutesHelper.IsSuitableForSubstitutesRelationType(relationTypeID) ? new RemoveSubstitutesParams()
    {
      ProjectVersionID = projectVersionID,
      RelationTypeID = relationTypeID
    } : throw new ArgumentException();
    @params.DeleteAuxiliaryPositionRelations = MessageBox.Show("Удалять связи при удалении вспомогательных позиций?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      using (NotificationContext.Create(sessionKeeper.Session, (object) this))
        (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesServerService)) as ISubstitutesServerService).RemoveSubstitutes(sessionKeeper.Session.SessionGUID, @params);
    }
  }
}
