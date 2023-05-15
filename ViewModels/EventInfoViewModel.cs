using Food_Scape.Models;
using System.Collections;
using System.Collections.Generic;

namespace Food_Scape.ViewModels
{
    public class EventInfoViewModel : IEnumerable<Event>
    {
        public List<Event> Events { get; set; }
        public List<Review> Reviews { get; set; }

        public IEnumerator<Event> GetEnumerator()
        {
            return Events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
