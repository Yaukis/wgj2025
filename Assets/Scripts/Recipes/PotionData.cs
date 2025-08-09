using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionData", menuName = "PotionData", order = 0)]
public class PotionData : ScriptableObject
{
    public string name;
    public Sprite icon;
    public List<IngredientData> ingredients;
}