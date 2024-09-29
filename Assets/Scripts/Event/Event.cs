using System;
using System.Collections.Generic;

namespace NueGames.Event
{
    [Serializable]
    public class Event
    {
        public EventData data;

        public List<Option> Options;


        public Event(EventData data, List<Option> options)
        {
            this.data = data;
            Options = options;
        }
    }
}