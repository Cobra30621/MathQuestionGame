
using System;
using System.Collections.Generic;
using NueGames.Data.Collection;

[Serializable]
public class RandomActionData
{
    public List<RandomAction> randomActionList;
}

[Serializable]
public class RandomAction
{
    public float Probability;
    public ActionData ActionData;
}