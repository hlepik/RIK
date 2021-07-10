using System.Collections;
using System.Collections.Generic;
using DAL.App.DTO;

namespace WebApp.Models
{
    public class ParticipantsViewModel
    {
        public Event? Event { get; set; }
        public List<Participant>? Participants { get; set; }

        public Person? Person { get; set; }
        public Company? Company { get; set; }
        public bool IsPerson { get; set; }
        public bool IsCompany { get; set; } = true;

    }
}