using System.Collections.Generic;
using System.Linq;
using NueGames.Event.Effect;

namespace NueGames.Event
{
    public static class EventFactory
    {
        public static Event GetEvent(EventData eventData)
        {
            var options = GetOptions(eventData.OptionData);

            return new Event(eventData, options);
        } 
        
        
        /// <summary>
        /// 取得選項
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static List<Option> GetOptions(List<OptionData> options)
        {
            return options.ConvertAll(GetOption).ToList();
        }

        public static Option GetOption(OptionData optionData)
        {
            
            var effect = GetEffect(optionData.EffectData);
            effect.Init(optionData.EffectData);
            
            var option = new Option(effect, optionData);
            
            return option;
        }


        private static IEffect GetEffect(EffectData optionData)
        {
            switch (optionData.EffectType)
            {
                case EffectType.Leave:
                    return new LeaveEffect();
                case EffectType.Reward:
                    return new GetRewardEffect();
                case EffectType.MathQuestion:
                    return new QuestionEffect();
                default:
                    return null;
            }
        }
        
        
        
    }
}