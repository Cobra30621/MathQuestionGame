using NueGames.Data.Containers;

namespace NueGames.Relic
{
    /// <summary>
    /// 遺物
    /// </summary>
    public class RelicClip
    {
        /// <summary>
        /// 執行遺物的邏輯
        /// </summary>
        public RelicBase Relic;
        /// <summary>
        /// 遺物的資料(用來顯示 UI、商品價格等等)
        /// </summary>
        public RelicData Data;

        public RelicClip(RelicBase relic, RelicData data)
        {
            Relic = relic;
            Data = data;
        }

        public override string ToString()
        {
            return $"{nameof(Relic)}: {Relic}, {nameof(Data)}: {Data}";
        }
    }
}