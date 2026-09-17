using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImageDatabase", menuName = "Scriptable Objects/ImageDatabase")]
public class ImageDatabase : ScriptableObject
{
    public List<BadgeData> badges;
}


[System.Serializable]
public class BadgeData
{
    public string id;
    public string URL;
    public string badgeHeading;

}
