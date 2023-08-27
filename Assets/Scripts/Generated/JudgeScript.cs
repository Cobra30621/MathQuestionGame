using UnityEngine;

public class JudgeScript : MonoBehaviour
{
    [Tooltip("Minimum HP value for the judge condition")]
    public int minHP = 40;

    [Tooltip("Maximum HP percentage for the judge condition")]
    [Range(0, 100)]
    public float maxHPPercentage = 60;

    public bool Judge(int enemyHP)
    {
        if (enemyHP > minHP && enemyHP < (maxHPPercentage / 100) * 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}