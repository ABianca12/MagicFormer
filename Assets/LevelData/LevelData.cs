using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public bool HasCollectedRedOrb;
    public bool HasCollectedYellowOrb;
    public bool HasCollectedBlueOrb;

    public bool PlayerStartsWithFireball;
    public bool PlayerStartsWithEarthCrate;
    public bool PlayerStartsWithForcePush;
    public bool PlayerStartsWithVine;
    public bool PlayerStartsWithTimestop;
}
