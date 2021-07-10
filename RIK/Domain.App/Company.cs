using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Company: DomainEntityId
    {

        [MaxLength(500)] public string CompanyName { get; set; } = default!;

        [MaxLength(20)]
        public string RegisterCode { get; set; } = default!;

        public int PeopleCount { get; set; }
        public PaymentType PaymentType { get; set; }
        [MaxLength(5000)]
        public string? Comment { get; set; }
        public Guid EventId { get; set;}
        public Event? Event { get; set; }

    }
}