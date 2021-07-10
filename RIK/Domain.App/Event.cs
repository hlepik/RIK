using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Event: DomainEntityId
    {


        [MaxLength(500)]
        public string EventName { get; set; } = default!;

        public DateTime EventDate { get; set; }
        [MaxLength(1000)]
        public string Location { get; set; }= default!;
        [MaxLength(1000)]
        public string? Comment { get; set; }

        public ICollection<Person>? Persons { get; set; }
        public ICollection<Company>? Companies { get; set; }
    }
}