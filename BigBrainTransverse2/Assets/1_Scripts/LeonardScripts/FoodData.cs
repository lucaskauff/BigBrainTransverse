using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType { greasy, sweet, energy };
public enum FoodName { burger, fries, kebab, tacos, drink };

[CreateAssetMenu(fileName = "New Food Type", menuName = "Food Spells")]
public class FoodData : ScriptableObject
{
    [SerializeField]
    private FoodType foodType;
    [SerializeField]
    private FoodName foodName;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private List<Mesh> foodMeshList = new List<Mesh>();
    [SerializeField]
    private Mesh foodMesh;
    [SerializeField]
    private int goldCost;
    [SerializeField]
    private int attackDamage;

    public FoodType FoodType
    {
        get
        {
            return foodType;
        }
        set
        {
            this.foodType = value;
        }
    }

    public FoodName FoodName
    {
        get
        {
            return foodName;
        }
    }

    public List<Mesh> FoodMeshList
    {
        get
        {
            return foodMeshList;
        }
    }

    public Mesh FoodMesh
    {
        get
        {
            return foodMesh;
        }

        set
        {
           this.foodMesh = value;
        }
    }

    public int GoldCost
    {
        get
        {
            return goldCost;
        }
    }

    public int AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }
}
