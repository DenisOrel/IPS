// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControlPropertyClass
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Класс для хранения настроек подписей на этапах жихненного цикла объектов
/// и уровнях продвижения объекта
/// </summary>
[TypeConverter(typeof (SignControlPropertyTypeConverter))]
[Editor(typeof (SignControlPropertyTypeEditor), typeof (UITypeEditor))]
public class SignControlPropertyClass
{
  private bool _isFilledOk;
  private GraphsSet _graphsSet = new GraphsSet();
  private int _id = -1;
  private SignControlPropertyEnum _idType;
  private int _objectType = -1;

  /// <summary>Конструктор</summary>
  public SignControlPropertyClass()
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="id">идентификатор</param>
  /// <param name="idType">тип идентификатора</param>
  /// <param name="isReadOnly">только для чтения</param>
  /// <param name="objectTypeID">идентификатор типа объекта</param>
  public SignControlPropertyClass(
    int id,
    SignControlPropertyEnum idType,
    bool isReadOnly,
    int objectTypeID)
  {
    this._objectType = objectTypeID;
    this.Load(id, idType, isReadOnly);
  }

  /// <summary>Конструктор</summary>
  /// <param name="id">идентификатор</param>
  /// <param name="idType">тип идентификатора</param>
  /// <param name="objectTypeID">идентификатор типа объекта</param>
  public SignControlPropertyClass(int id, SignControlPropertyEnum idType, int objectTypeID)
    : this(id, idType, false, objectTypeID)
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="id">идентификатор</param>
  /// <param name="idType">тип идентификатора</param>
  public SignControlPropertyClass(int id, SignControlPropertyEnum idType)
    : this(id, idType, false, -1)
  {
  }

  /// <summary>Сохранить изменения в контейнер</summary>
  /// <param name="id">идентификатор</param>
  /// <param name="idType">тип идентификатора</param>
  /// <returns>True если все ок</returns>
  public bool Save(int id, SignControlPropertyEnum idType)
  {
    bool flag = false;
    switch (idType)
    {
      case SignControlPropertyEnum.LCStep:
        flag = this.SaveStep(id);
        break;
      case SignControlPropertyEnum.LCLevel:
        flag = this.SaveLevel(id);
        break;
    }
    return flag;
  }

  /// <summary>Сохранить изменения в атрибут объекта - контейнера</summary>
  /// <param name="stepID">Шаг жихненного цикла</param>
  /// <returns>True если все ок</returns>
  private bool SaveStep(int stepID)
  {
    this._id = stepID;
    this._idType = SignControlPropertyEnum.LCStep;
    bool flag1 = false;
    if (stepID >= 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IUserSession session = sessionKeeper.Session;
        IContainerService customService = session.GetCustomService(typeof (IContainerService)) as IContainerService;
        IDBObject dbObject = (IDBObject) null;
        for (int index = 0; dbObject == null && index < 50; ++index)
        {
          Thread.Sleep(200);
          if (this._objectType != 0)
          {
            dbObject = customService.GetContainerForLCStepObjectType((object) session.SessionGUID, stepID, this._objectType, true);
          }
          else
          {
            dbObject = customService.GetContainerForLCStep((object) session.SessionGUID, stepID, true);
            this._idType = SignControlPropertyEnum.LCStep;
          }
        }
        bool flag2 = dbObject.ObjectModifyMode == ObjectModifyModes.Checkout;
        if (flag2)
          dbObject = dbObject.CheckOut();
        try
        {
          IDBAttribute aIDBAttribute = dbObject.Attributes.AddAttribute(SignsHolder.SignsSetupAttrTypeID, false);
          using (MemoryStream memoryStream = new MemoryStream())
          {
            Guid guid = (sessionKeeper.Session.GetLifecycleStep(stepID) as IDBGuid).GUID;
            this._graphsSet.Save((Stream) memoryStream);
            BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "sings.xml", ArcMethods.ZLibPacked, string.Empty);
            new BlobProcWriter(aIDBAttribute, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
          }
        }
        finally
        {
          if (flag2)
            dbObject.CheckIn();
        }
        SignsCache.ClearCache(session);
      }
      flag1 = true;
    }
    return flag1;
  }

  /// <summary>Сохранить изменения в атрибут объекта - контейнера</summary>
  /// <param name="levelID">Уровень продвижения</param>
  /// <returns>True если все ок</returns>
  private bool SaveLevel(int levelID)
  {
    this._id = levelID;
    this._idType = SignControlPropertyEnum.LCLevel;
    bool flag = false;
    if (levelID >= 0)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IUserSession session = sessionKeeper.Session;
        IDBAttribute aIDBAttribute = (session.GetCustomService(typeof (IContainerService)) as IContainerService).GetContainerForLCLevel((object) session.SessionGUID, levelID, true).Attributes.AddAttribute(SignsHolder.SignsSetupAttrTypeID, false);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          this._graphsSet.Save((Stream) memoryStream);
          BlobInformation aBlobInformation = new BlobInformation(memoryStream.Length, 0L, DateTime.Now, "sings.xml", ArcMethods.ZLibPacked, string.Empty);
          new BlobProcWriter(aIDBAttribute, 0, aBlobInformation, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
        }
        SignsCache.ClearCache(session);
      }
      flag = true;
    }
    return flag;
  }

  /// <summary>Загрузить данные из контейнера</summary>
  /// <param name="id"></param>
  /// <param name="idType"></param>
  /// <param name="isReadOnly"></param>
  public void Load(int id, SignControlPropertyEnum idType, bool isReadOnly)
  {
    switch (idType)
    {
      case SignControlPropertyEnum.LCStep:
        this.LoadStep(id);
        break;
      case SignControlPropertyEnum.LCLevel:
        this.LoadLevel(id);
        break;
    }
    this._isFilledOk = this._graphsSet.Count > 0;
  }

  /// <summary>Загрузить данные из контейнера</summary>
  /// <param name="step">Шаг жизненного цикла</param>
  private void LoadStep(int step)
  {
    this._isFilledOk = false;
    this._graphsSet.Clear();
    this._id = step;
    this._idType = SignControlPropertyEnum.LCStep;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      IContainerService customService = session.GetCustomService(typeof (IContainerService)) as IContainerService;
      IDBObject dbObject;
      if (this._objectType != 0)
      {
        dbObject = customService.GetContainerForLCStepObjectType((object) session.SessionGUID, step, this._objectType);
      }
      else
      {
        dbObject = customService.GetContainerForLCStep((object) session.SessionGUID, step);
        this._idType = SignControlPropertyEnum.LCStep;
      }
      if (dbObject == null)
        return;
      IDBAttribute attributeById = dbObject.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
        if (memoryStream.Length <= 0L)
          return;
        this._graphsSet = GraphsSet.Load((Stream) memoryStream);
      }
    }
  }

  /// <summary>Загрузка параметров для уровня продвижения</summary>
  /// <param name="level">Идентификатор уровня продвижения</param>
  private void LoadLevel(int level)
  {
    this._isFilledOk = false;
    this._graphsSet.Clear();
    this._id = level;
    this._idType = SignControlPropertyEnum.LCLevel;
    if (level <= 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      IDBObject containerForLcLevel = (sessionKeeper.Session.GetCustomService(typeof (IContainerService)) as IContainerService).GetContainerForLCLevel((object) session.SessionGUID, level);
      if (containerForLcLevel == null)
        return;
      IDBAttribute attributeById = containerForLcLevel.GetAttributeByID(SignsHolder.SignsSetupAttrTypeID);
      if (attributeById == null)
        return;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BlobProcReader(attributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
        if (memoryStream.Length <= 0L)
          return;
        this._graphsSet = GraphsSet.Load((Stream) memoryStream);
      }
    }
  }

  /// <summary>Загружено и заполненно</summary>
  public bool isFilledOk => this._isFilledOk;

  /// <summary>Набор подписей</summary>
  public GraphsSet GraphsSet
  {
    get => this._graphsSet;
    set
    {
      this._graphsSet = GraphsSet.Clone(value);
      this._isFilledOk = this._graphsSet.Count > 0;
    }
  }

  /// <summary>идентификатор типа объекта</summary>
  public int ObjectTypeID => this._objectType;

  /// <summary>Шаг жизненного цикла</summary>
  public int LCStep => this._idType.Equals((object) SignControlPropertyEnum.LCStep) ? this._id : -1;

  /// <summary>Уровень продвижения объекта</summary>
  public int LCLevel
  {
    get => this._idType.Equals((object) SignControlPropertyEnum.LCLevel) ? this._id : -1;
  }

  /// <summary>Дублирование объекта</summary>
  /// <param name="value">Исходный объекта</param>
  /// <returns>Конечный объект</returns>
  public static SignControlPropertyClass Clone(SignControlPropertyClass value)
  {
    return new SignControlPropertyClass()
    {
      _objectType = value._objectType,
      _id = value._id,
      _idType = value._idType,
      _graphsSet = GraphsSet.Clone(value._graphsSet),
      _isFilledOk = value._isFilledOk
    };
  }
}
