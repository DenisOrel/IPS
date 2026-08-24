// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsObjectsCreatorService
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Statistics.Configurations;
using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class StatisticsObjectsCreatorService : 
  IObjectCreatorRiderCustomService,
  IObjectCreatorCustomService
{
  public long CreateObjectDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    if (MetaDataHelper.GetObjectTypeID(StatisticsConst.StatisticsCommandTypeGuid) == ObjectTypeID)
    {
      CommandObjectCreator commandObjectCreator = new CommandObjectCreator(ObjectTypeID);
      if (commandObjectCreator.ShowDialog() != DialogResult.OK)
        return -1;
      bool flag = true;
      if (commandObjectCreator.OpenConfig)
      {
        CommandSettings commandSettings = new CommandSettings();
        switch (commandObjectCreator.CommandType)
        {
          case CommandStatisticsTypesEnum.CreatedDate:
            CreatedDateConfigsForm createdDateConfigsForm = new CreatedDateConfigsForm();
            createdDateConfigsForm.UserType = commandObjectCreator.UsersType;
            if (createdDateConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = createdDateConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.SignDate:
            SignDateConfigsForm signDateConfigsForm = new SignDateConfigsForm();
            signDateConfigsForm.UserType = commandObjectCreator.UsersType;
            if (signDateConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = signDateConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.LCStepDate:
            LCStepDateConfigsForm stepDateConfigsForm = new LCStepDateConfigsForm();
            stepDateConfigsForm.UserType = commandObjectCreator.UsersType;
            if (stepDateConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = stepDateConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.LCLevelDate:
            LCLevelDateConfigsForm levelDateConfigsForm = new LCLevelDateConfigsForm();
            levelDateConfigsForm.UserType = commandObjectCreator.UsersType;
            if (levelDateConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = levelDateConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.DateAttrValue:
            DateAttrValueConfigsForm valueConfigsForm = new DateAttrValueConfigsForm();
            valueConfigsForm.UserType = commandObjectCreator.UsersType;
            if (valueConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = valueConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.ProcessTemplate:
            ProcessTemplateConfigsForm templateConfigsForm = new ProcessTemplateConfigsForm();
            if (templateConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = templateConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.TimeInTask:
            TimeInTaskConfigsForm inTaskConfigsForm = new TimeInTaskConfigsForm();
            if (inTaskConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = inTaskConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.TimeOneTaskFormUsers:
            TimeOneTaskFormUsersConfigsForm usersConfigsForm = new TimeOneTaskFormUsersConfigsForm();
            if (usersConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = usersConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
          case CommandStatisticsTypesEnum.RevertCountTask:
            RevertCountTaskConfigsForm countTaskConfigsForm = new RevertCountTaskConfigsForm();
            if (countTaskConfigsForm.ShowDialog() == DialogResult.OK)
            {
              commandSettings = countTaskConfigsForm.Settings;
              flag = false;
              break;
            }
            break;
        }
        if (!flag)
        {
          if (!(ApplicationServices.Container.GetService(typeof (IStatisticsClientService)) is IStatisticsClientService service))
            throw new KernelException("Не найден IStatisticsClientService");
          using (SessionKeeper sessionKeeper = new SessionKeeper())
            service.WriteStatisticObjectsCommandSettings(sessionKeeper.Session, commandObjectCreator.ObjectID, commandSettings);
        }
        if (flag)
          this.SetSettingWithoutConfigForm(commandObjectCreator.ObjectID, commandObjectCreator.UsersType, StatisticsObjectsTypeEnum.CommandStatisticsObject, commandObjectCreator.CommandType);
      }
      else
        this.SetSettingWithoutConfigForm(commandObjectCreator.ObjectID, commandObjectCreator.UsersType, StatisticsObjectsTypeEnum.CommandStatisticsObject, commandObjectCreator.CommandType);
      return commandObjectCreator.ObjectID;
    }
    MetaDataHelper.GetObjectTypeID(StatisticsConst.StatisticsTasksObjectsTypeGuid);
    return -1;
  }

  private void SetSettingWithoutConfigForm(
    long objectID,
    UsersEnum userType,
    StatisticsObjectsTypeEnum objTypeEnum,
    CommandStatisticsTypesEnum commandTypesEnum = CommandStatisticsTypesEnum.None)
  {
    if (!(ApplicationServices.Container.GetService(typeof (IStatisticsClientService)) is IStatisticsClientService service))
      throw new KernelException("Не найден IStatisticsClientService");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      CommandSettings commandSettings = new CommandSettings()
      {
        ObjectID = objectID,
        StatisticsUsersType = userType,
        StatisticsObjectType = objTypeEnum,
        CommandType = commandTypesEnum,
        StartDateTime = DateTime.Now,
        EndDateTime = DateTime.Now,
        Filters = new Filters(),
        AnalizedObjectsTypes = new List<ObjectTypesListItem>(),
        CollectPeriodIndex = 1,
        ListUsers = new List<StatisticsUsers>(),
        CollectPeriod = CollectPeriodsEnum.Day,
        Activities = new List<ActivityItem>(),
        Templates = new List<ListItem>()
      };
      service.WriteStatisticObjectsCommandSettings(sessionKeeper.Session, objectID, commandSettings);
    }
  }

  public bool AcceptDialog(
    int ObjectTypeID,
    long TemplateObjectID,
    int[] RelationTypeIDs,
    long[] RelatedObjectIDs,
    DateTime StartDate,
    bool isVersion)
  {
    return true;
  }

  public bool AfterCreate(long newObjectID) => true;

  public IDictionary<ObjectCreatePages, bool> VisiblePages { get; private set; }

  public bool OnCommitAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  public bool OnBeforeCommitAction(IUserSession session, IDBObject newObject) => true;

  public bool OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  public Dictionary<UserControl, int> AddPages(object CreatedObject, int propPageIndex)
  {
    return (Dictionary<UserControl, int>) null;
  }
}
