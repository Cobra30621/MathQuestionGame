using System;
using rStarTools.Scripts.StringList;

namespace Data.Encounter
{
    [Serializable]
    public class EncounterName : NameBase<EnemyEncounterOverview>
    {
        protected override string LabelText => "遭遇";
    }
}