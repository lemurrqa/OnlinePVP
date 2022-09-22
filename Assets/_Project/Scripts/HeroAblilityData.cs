using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroAblilityData", menuName = "ScriptableObjects/HeroAblilityData", order = 1)]
public class HeroAblilityData : ScriptableObject
{
    public List<Data> data;

    [System.Serializable]
    public class Data
    {
        public AbilityType type;
        public float distance;
    }
}
