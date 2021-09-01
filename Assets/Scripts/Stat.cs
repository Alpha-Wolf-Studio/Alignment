using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public string name;
    [SerializeField] private float initial;             // Se modifica por crafteo
    [SerializeField] private int points;                // Puntos que el jugador pone
    [SerializeField] private float amountPlus;          // Puntos que te da la mejora
    [SerializeField] private float multiplySubtract;    // Multiplicador que disminuye el amount
    [SerializeField] private float current;             // La cantidad actual que tiene
    [SerializeField] private float max;                 // maximo que se calcula con: Inicial + points * amountOfPoint * (multiplySubtract * amountOfPoint)

    public void AddPoint()
    {
        points++;
        CalculateMax();
    }
    public void Init()
    {
        current = initial;
        CalculateMax();
    }
    void CalculateMax()
    {
        max = initial;
        if (points > 0)
        {
            max += amountPlus;
            for (int i = 1; i < points; i++)
            {
                max += amountPlus * (multiplySubtract / (i - 1));
            }
        }
    }
    public void SetInitial(float value)
    {
        initial = value;
    }
    public void SetIncrementForPoints(float value)
    {
        amountPlus = 0;
    }
    public void SetMultiplySubtract(float value)
    {
        multiplySubtract = 0;
    }
    public void SetCurrent(float value)
    {
        current = value;
    }
    public void SetMax(float value)
    {
        max = value;
    }
    // ----------------------------------------------------------------
    public void AddInitial(float increaseIn)
    {
        initial += increaseIn;
        CalculateMax();
    }
    public void AddCurrent(float increaseIn)
    {
        current += increaseIn;
        //if (current > max) current = max;
    }
    // -----------------------------------------------------------------------
    public float GetInitial()
    {
        return initial;
    }
    public float GetPoints()
    {
        return points;
    }
    public float GetIncrementForPoints()
    {
        return amountPlus;
    }
    public float GetMultiplySubtract()
    {
        return multiplySubtract;
    }
    public float GetCurrent()
    {
        return current;
    }
    public float GetMax()
    {
        return max;
    }




}