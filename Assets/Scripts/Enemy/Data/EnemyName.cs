﻿using System;
using rStarTools.Scripts.StringList;

namespace Enemy
{
    [Serializable]
    public class EnemyName : NameBase<EnemyDataOverview>
    {
        #region Protected Variables

        protected override string LabelText => "敵人ID:";
        
        #endregion
    }
}