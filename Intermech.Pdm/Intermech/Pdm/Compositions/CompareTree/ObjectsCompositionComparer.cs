// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ObjectsCompositionComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ObjectsCompositionComparer
{
  private readonly Guid _ruleID;
  private Thread _thread1;
  private Thread _thread2;

  public ObjectsCompositionComparer(Guid ruleID) => this._ruleID = ruleID;

  private Thread StartReadCompositionThread(
    CompositionItem item,
    CompositionFiltrationSettings filtration,
    bool recursive)
  {
    Thread thread = new Thread(new ParameterizedThreadStart(this.ReadCompositionThread));
    thread.IsBackground = true;
    thread.Name = $"PDM.TwoObjectsComparer_ReadComposition_{Guid.NewGuid()}";
    thread.Start((object) new LevelCompositionReaderArgs(item, filtration, recursive));
    return thread;
  }

  public void Abort()
  {
    if (this._thread1 != null && this._thread1.IsAlive)
      this._thread1.Abort();
    if (this._thread2 == null || !this._thread2.IsAlive)
      return;
    this._thread2.Abort();
  }

  public void Compare(
    CompositionItem item1,
    CompositionFiltrationSettings filtration1,
    CompositionItem item2,
    CompositionFiltrationSettings filtration2,
    bool recursive)
  {
    this._thread1 = this.StartReadCompositionThread(item1, filtration1, recursive);
    this._thread2 = this.StartReadCompositionThread(item2, filtration2, recursive);
    if (this._thread1 != null && this._thread1.IsAlive)
      this._thread1.Join();
    if (this._thread2 != null && this._thread2.IsAlive)
      this._thread2.Join();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.CompareItems(sessionKeeper.Session, item1, item2);
  }

  private void ReadCompositionThread(object obj)
  {
    try
    {
      LevelCompositionReaderArgs compositionReaderArgs = (LevelCompositionReaderArgs) obj;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string sessionName = $"PDM.TwoObjectsComparer_{Guid.NewGuid()}";
        IUserSession userSession = sessionKeeper.Session.Clone(sessionName);
        try
        {
          ICompareTreeSettingsService service = (ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService));
          LevelCompositionReader compositionReader = new LevelCompositionReader(compositionReaderArgs.Filtration, this._ruleID, service);
          userSession.EditingContextID = compositionReaderArgs.Filtration.EditingContextID;
          IUserSession session = sessionKeeper.Session;
          CompositionItem parent = compositionReaderArgs.Item;
          int num = compositionReaderArgs.Recursive ? 1 : 0;
          compositionReader.Read(session, parent, num != 0);
        }
        finally
        {
          userSession.Logout(sessionName);
        }
      }
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "Ошибка при получении состава", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private void CompareItems(IUserSession session, CompositionItem item1, CompositionItem item2)
  {
    ICompareTreeSettingsService service = (ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService));
    bool flag1 = false;
    if (item2.Count == 0)
    {
      for (int index = 0; index < item1.Count; ++index)
      {
        CompositionItem levelItem = item1[index];
        CompositionItem compositionItem = item2.Find((Predicate<CompositionItem>) (x => !x.Empty && x.PrjLinkGUID.Equals(levelItem.PrjLinkGUID)));
        if (compositionItem != null)
        {
          levelItem.LevelIndex = index;
          compositionItem.LevelIndex = index;
          if (levelItem.ID == compositionItem.ID && levelItem.ObjectID != compositionItem.ObjectID)
            compositionItem.CompositionItemFlag |= CompositionItemFlags.AnotherVersion;
        }
      }
    }
    for (int index = 0; index < item1.Count; ++index)
    {
      CompositionItem levelItem = item1[index];
      if (levelItem.LevelIndex < 0)
      {
        levelItem.LevelIndex = index;
        if (item2.Count == 0)
        {
          CompareFlagHelper.SetStateFlagRecursiveDown(levelItem, CompositionItemFlags.Removed);
          item2.Add(this.CreateEmptyFromProtorype(item2, levelItem, index));
          flag1 = true;
        }
        else
        {
          CompositionItem compositionItem = (CompositionItem) null;
          List<CompositionItem> all = item2.FindAll((Predicate<CompositionItem>) (x => !x.Empty && x.LevelIndex < 0 && x.ObjectID.Equals(levelItem.ObjectID)));
          bool flag2 = false;
          if (all.Count == 0)
          {
            all = item2.FindAll((Predicate<CompositionItem>) (x => !x.Empty && x.LevelIndex < 0 && x.ID.Equals(levelItem.ID)));
            flag2 = all.Count > 0;
          }
          if (all.Count == 1)
          {
            all[0].LevelIndex = index;
            compositionItem = all[0];
            if (flag2)
              compositionItem.CompositionItemFlag |= CompositionItemFlags.AnotherVersion;
          }
          else
          {
            IDAttributesSearcher attributesSearcher = new IDAttributesSearcher(service.GetIDRelationAttributes(this._ruleID, item1.ObjectTypeID, levelItem.RelationTypeID), AttributeSourceTypes.Relation);
            if (all.Count == 0)
            {
              compositionItem = attributesSearcher.Find(levelItem, (List<CompositionItem>) item2);
              if (compositionItem != null)
              {
                compositionItem.LevelIndex = index;
              }
              else
              {
                compositionItem = new IDAttributesSearcher(service.GetIDObjectAttributes(this._ruleID, levelItem.ObjectTypeID), AttributeSourceTypes.Object).Find(levelItem, (List<CompositionItem>) item2);
                if (compositionItem != null)
                  compositionItem.LevelIndex = index;
              }
            }
            else if (all.Count > 1)
            {
              compositionItem = attributesSearcher.Find(levelItem, all) ?? all.Find((Predicate<CompositionItem>) (x => x.LevelIndex < 0));
              if (compositionItem != null)
              {
                compositionItem.LevelIndex = index;
                if (flag2)
                  compositionItem.CompositionItemFlag |= CompositionItemFlags.AnotherVersion;
              }
            }
          }
          if (compositionItem == null)
          {
            CompareFlagHelper.SetStateFlagRecursiveDown(levelItem, CompositionItemFlags.Removed);
            item2.Add(this.CreateEmptyFromProtorype(item2, levelItem, index));
            flag1 = true;
          }
          else
            this.CompareItems(session, levelItem, compositionItem);
        }
      }
    }
    for (int index = 0; index < item2.Count; ++index)
    {
      CompositionItem compositionItem = item2[index];
      if (compositionItem.LevelIndex < 0)
      {
        this.InsertEmpryItemToCollection(item1, item2, compositionItem, index);
        CompareFlagHelper.SetStateFlagRecursiveDown(compositionItem, CompositionItemFlags.Added);
        flag1 = true;
      }
    }
    if (flag1)
      CompareFlagHelper.SetStateFlagRecursiveUp(item2, CompositionItemFlags.ChangedInComposition);
    item1.Sort((Comparison<CompositionItem>) ((x, y) => x.LevelIndex.CompareTo(y.LevelIndex)));
    item2.Sort((Comparison<CompositionItem>) ((x, y) => x.LevelIndex.CompareTo(y.LevelIndex)));
  }

  private void InsertEmpryItemToCollection(
    CompositionItem item1,
    CompositionItem item2,
    CompositionItem item,
    int index)
  {
    this.IncrementLevelIndex((List<CompositionItem>) item1, index);
    this.IncrementLevelIndex((List<CompositionItem>) item2, index);
    item1.Add(this.CreateEmptyFromProtorype(item1, item, index));
    item.LevelIndex = index;
  }

  private void IncrementLevelIndex(List<CompositionItem> items, int minIndex)
  {
    foreach (CompositionItem compositionItem in items)
    {
      if (compositionItem.LevelIndex >= minIndex)
        ++compositionItem.LevelIndex;
    }
  }

  private CompositionItem CreateEmptyFromProtorype(
    CompositionItem parent,
    CompositionItem prototype,
    int levelIndex)
  {
    CompositionItem empty = CompositionItem.CreateEmpty(parent, levelIndex);
    if (prototype.Count > 0)
    {
      foreach (CompositionItem prototype1 in (List<CompositionItem>) prototype)
        empty.Add(this.CreateEmptyFromProtorype(empty, prototype1, prototype1.LevelIndex));
    }
    return empty;
  }
}
