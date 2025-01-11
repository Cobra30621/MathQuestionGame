using System;
using rStarTools.Scripts.StringList;

namespace Encounter.Data
{
    [Serializable]
    public class EncounterName : NameBase<EnemyEncounterOverview>
    {
        protected override string LabelText => "遭遇";
    }
}