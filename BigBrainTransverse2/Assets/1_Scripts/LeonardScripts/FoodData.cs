using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType { greasy, sweet, energy };
public enum FoodName { fries, tacos, sunday, coke, redbull };

[CreateAssetMenu(fileName = "New Food Type", menuName = "Food Spells")]
public class FoodData : ScriptableObject
{
    [SerializeField] private FoodType foodType;
    [SerializeField] private FoodName foodName;
    /*[SerializeField] private Mesh mesh;
    [SerializeField] private List<Mesh> foodMeshList = new List<Mesh>();
    [SerializeField] private Mesh foodMesh;
    [SerializeField] private Material foodMate;*/
    [SerializeField] private int calorieGainOnHit;
    [SerializeField] private int calorieToleranceDecrease;
    [SerializeField] private int calorieGainOverTime;

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

    /*public List<Mesh> FoodMeshList
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
    }*/

    public int CalorieGainOnHit
    {
        get
        {
            return calorieGainOnHit;
        }
    }

    public int CalorieToleranceDecrease
    {
        get
        {
            return calorieToleranceDecrease;
        }
    }

    public int CalorieGainOverTime
    {
        get
        {
            return calorieGainOverTime;
        }
    }
}
