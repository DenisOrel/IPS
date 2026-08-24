// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.AttrsRelationshipPropertyClass
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class AttrsRelationshipPropertyClass
{
  private int _relatingAttrId;
  private int _relatedAttrId;
  private long _objectId;

  public AttrsRelationshipPropertyClass()
    : this(0, 0, 0L)
  {
  }

  public AttrsRelationshipPropertyClass(int relatingAttrId, int relatedAttrId, long objectId)
  {
    this._relatingAttrId = relatingAttrId;
    this._relatedAttrId = relatedAttrId;
    this._objectId = objectId;
  }

  public AttrsRelationshipPropertyClass(long objectId)
    : this(0, 0, objectId)
  {
  }

  public AttrsRelationshipPropertyClass(string stringValue)
  {
    string[] strArray = stringValue.Split('=');
    int result1;
    int result2;
    long result3;
    if (!strArray.Length.Equals(3) || !int.TryParse(strArray[0], out result1) || !int.TryParse(strArray[1], out result2) || !long.TryParse(strArray[2], out result3))
      return;
    this._relatingAttrId = result1;
    this._relatedAttrId = result2;
    this._objectId = result3;
  }

  public int RelatingAttrId => this._relatingAttrId;

  public int RelatedAttrId => this._relatedAttrId;

  public long ObjectId
  {
    get => this._objectId;
    set => this._objectId = value;
  }

  public override string ToString()
  {
    return $"{this.RelatingAttrId}={this.RelatedAttrId}={this.ObjectId}";
  }
}
