using System;
using UnityEngine;

// Create a ScriptableObject to store data
[CreateAssetMenu(fileName = "NewTreeData", menuName = "CustomData/TreeInformation")]
public class TreeInformation : ScriptableObject
{
    public string treeName;
    public int Amount;

    public int Price;
}
