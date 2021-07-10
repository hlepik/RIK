using System.Collections.Generic;
using DAL.App.DTO;

namespace WebApp.Models
{
    public class AllEventsViewModel
    {
        public IEnumerable<Event?>? ComingEvents { get; set; }
        public IEnumerable<Event?>? PreviousEvents { get; set; }
    }
}