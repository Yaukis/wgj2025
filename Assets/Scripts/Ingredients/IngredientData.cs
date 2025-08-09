using Ingredients;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "IngredientData", order = 0)]
public class IngredientData : ScriptableObject
{
    public IngredientType id;
    public string name;
    public Sprite icon;
    public GameObject worldPrefab;
    public GameObject grabPrefab;
}