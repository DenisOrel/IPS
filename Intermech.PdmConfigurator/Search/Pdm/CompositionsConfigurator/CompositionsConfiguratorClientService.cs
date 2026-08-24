// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.CompositionsConfiguratorClientService
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public sealed class CompositionsConfiguratorClientService : ICompositionsConfiguratorClientService
{
  private LazyService<IClipboard> _clipboard = new LazyService<IClipboard>();

  public void CopyApplicationConditionsToClipboard(long relationID)
  {
    if (RelationHelper.IsUnknownRelationID(relationID))
      throw new ArgumentException();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ObjectsApplicabilitiesCriterionsCollection clipboardObject = new ObjectsApplicabilitiesCriterionsCollection((object) sessionKeeper.Session.GetRelation(relationID));
      if (clipboardObject.Count > 0)
      {
        this._clipboard.Value.SetDataObject((object) clipboardObject);
      }
      else
      {
        int num = (int) MessageBox.Show("Невозможно выполнить команду. Не найдено условий применения", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }
  }

  public void PasteApplicationConditionsFromClipboard(IEnumerable<long> relationIds)
  {
    if (relationIds == null)
      throw new ArgumentNullException(nameof (relationIds));
    if (RelationHelper.IsAnyUnknownRelationID(relationIds))
      throw new ArgumentException();
    if (!(this._clipboard.Value.GetDataObject() is ObjectsApplicabilitiesCriterionsCollection dataObject))
    {
      int num = (int) MessageBox.Show("Ошибка. Буфер обмена пуст или содержит данные неподходящего формата", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
    {
      long[] optionVersionIds = dataObject.GetOptionVersionIds();
      foreach (long aRelationID in relationIds.Distinct<long>())
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          using (NotificationContext.Create(sessionKeeper.Session))
          {
            IDBRelation relation = sessionKeeper.Session.GetRelation(aRelationID);
            try
            {
              IDBObject source = sessionKeeper.Session.GetObject(relation.ProjID);
              ObjectOptionsHolder objectOptionsHolder = new ObjectOptionsHolder((object) source);
              long[] notPresentOnObject = this.GetNewOptionVersinIdsNotPresentOnObject((IEnumerable<long>) objectOptionsHolder.Options, (IEnumerable<long>) optionVersionIds);
              if (notPresentOnObject.Length != 0)
              {
                objectOptionsHolder.AddOptions((IList<long>) ((IEnumerable<long>) notPresentOnObject).ToList<long>());
                objectOptionsHolder.SaveToObject((IDBAttributable) source);
              }
              ObjectsApplicabilitiesCriterionsCollection criterionsCollection = new ObjectsApplicabilitiesCriterionsCollection((object) relation);
              if (criterionsCollection.Count > 0)
              {
                if (MessageBox.Show($"На связи #{relation.RelationID} уже имеются условия применения. Добавить к существующим?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                  criterionsCollection.AddRange((IList<IPdmCriterion>) dataObject);
                  criterionsCollection.SaveToObject((IDBAttributable) relation);
                }
                else
                  dataObject.SaveToObject((IDBAttributable) relation);
              }
              else
                dataObject.SaveToObject((IDBAttributable) relation);
            }
            catch (Exception ex)
            {
              if (MessageBox.Show($"Во время добавления условий применения для связи #{relation.RelationID} произошла ошибка: {ex.Message}{Environment.NewLine}Продолжить?", "Intermech Professional Solution", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.No)
                break;
            }
          }
        }
      }
    }
  }

  private long[] GetNewOptionVersinIdsNotPresentOnObject(
    IEnumerable<long> objectOptionVersionIds,
    IEnumerable<long> newOptionVersionIds)
  {
    return newOptionVersionIds.Where<long>((Func<long, bool>) (o => !objectOptionVersionIds.Contains<long>(o))).ToArray<long>();
  }
}
