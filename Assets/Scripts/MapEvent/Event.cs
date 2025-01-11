using System;
using System.Collections.Generic;
using MapEvent.Data;

namespace MapEvent
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