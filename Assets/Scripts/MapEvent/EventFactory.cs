using System.Collections.Generic;
using System.Linq;
using MapEvent.Data;
using MapEvent.Effect;

namespace MapEvent
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


        private static IEventEffect GetEffect(EffectData optionData)
        {
            switch (optionData.eventEffectType)
            {
                case EventEffectType.Leave:
                    return new LeaveEventEffect();
                case EventEffectType.Reward:
                    return new GetRewardEventEffect();
                case EventEffectType.MathQuestion:
                    return new QuestionEventEffect();
                case EventEffectType.ChangeHealth:
                    return new ChangeHealthEventEffect();
                case EventEffectType.PayAndGain:
                    return new PayAndGainEventEffect();
                default:
                    return null;
            }
        }
        
        
        
    }
}