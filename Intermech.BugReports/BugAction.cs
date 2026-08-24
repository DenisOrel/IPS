// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.BugAction
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using Intermech.PropertyEditors.AttrProcessor;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.BugReports;

public abstract class BugAction : IFormDesignerActionHandler
{
  protected int _userAttrID;
  protected int _dataAttrID;
  protected int _resultAttrID;
  protected string _status = string.Empty;

  public virtual bool ButtonEnabled(object button, object form)
  {
    bool flag = false;
    if (form is DesForm desForm)
    {
      IElementInfo info = desForm.Info;
      if (info != null && info.ElementKind == AttributableElements.Object)
      {
        AttributeProcessor processor = desForm.Processor;
        AttributeValues attributeValues1 = processor.FindAttributeValues(this._userAttrID);
        AttributeValues attributeValues2 = processor.FindAttributeValues(this._dataAttrID);
        AttributeValues attributeValues3 = processor.FindAttributeValues(this._resultAttrID);
        flag = attributeValues1 != null && !attributeValues1.ReadOnly && attributeValues2 != null && !attributeValues2.ReadOnly && attributeValues3 != null && !attributeValues3.ReadOnly;
      }
    }
    return flag;
  }

  public virtual void ButtonPressed(object button, object form)
  {
    DesForm desForm = form as DesForm;
    List<AttributeValues> newObjectValues = new List<AttributeValues>(3);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long userId = sessionKeeper.Session.UserID;
      newObjectValues.Add(new AttributeValues(this._userAttrID, (object) userId));
    }
    newObjectValues.Add(new AttributeValues(this._dataAttrID, (object) DateTime.Now));
    newObjectValues.Add(new AttributeValues(this._resultAttrID, (object) this._status));
    desForm.AttributeChanging((IEnumerable<AttributeValues>) newObjectValues);
  }
}
