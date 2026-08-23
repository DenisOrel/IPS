// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControlPropertyClass
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Signs.Client;

[TypeConverter(typeof (SignControlPropertyTypeConverter))]
[Editor(typeof (SignControlPropertyTypeEditor), typeof (UITypeEditor))]
public class SignControlPropertyClass
{
  private bool _isFilledOk;
  private GraphsSet _graphsSet = new GraphsSet();
  private int _id = -1;
  private SignControlPropertyEnum _idType;
  private int _objectType = -1;

  public SignControlPropertyClass()
  {
  }

  public SignControlPropertyClass(
    int id,
    SignControlPropertyEnum idType,
    bool isReadOnly,
    int objectTypeID)
  {
    this._objectType = objectTypeID;
    this.Load(id, idType, isReadOnly);
  }

  public SignControlPropertyClass(int id, SignControlPropertyEnum idType, int objectTypeID)
    : this(id, idType, false, objectTypeID)
  {
  }

  public SignControlPropertyClass(int id, SignControlPropertyEnum idType)
    : this(id, idType, false, -1)
  {
  }

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

  public bool isFilledOk => this._isFilledOk;

  public GraphsSet GraphsSet
  {
    get => this._graphsSet;
    set
    {
      this._graphsSet = GraphsSet.Clone(value);
      this._isFilledOk = this._graphsSet.Count > 0;
    }
  }

  public int ObjectTypeID => this._objectType;

  public int LCStep => this._idType.Equals((object) SignControlPropertyEnum.LCStep) ? this._id : -1;

  public int LCLevel
  {
    get => this._idType.Equals((object) SignControlPropertyEnum.LCLevel) ? this._id : -1;
  }

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
