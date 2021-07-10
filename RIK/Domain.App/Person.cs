using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Person: DomainEntityId
    {


        [MaxLength(256)]
        public string FirstName { get; set; } = default!;
        [MaxLength(256)]
        public string LastName { get; set; } = default!;
        [MaxLength(20)]
        public string IdentificationCode { get; set; } = default!;

        public PaymentType PaymentType { get; set; }
        [MaxLength(1500)]

        public string? Comment { get; set; }

        public Guid EventId { get; set;}
        public Event? Event { get; set; }
    }
}