using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Event: DomainEntityId
    {


        [MaxLength(500, ErrorMessage = "Ürituse nimi peab jääma alla 500 märgi!"), MinLength(2, ErrorMessage = "Ürituse nimi peab olema vähemalt 2 märki pikk!")]
        [DisplayName("Ürituse nimi")]
        [Required(ErrorMessage = "Ürituse nimi väli on kohustuslik!")]
        public string EventName { get; set; } = default!;


        [DisplayName("Toimumisaeg")]
        [Required(ErrorMessage = "Kuupäev ei saa olla tühi!")]
        public DateTime EventDate { get; set; }
        [MaxLength(1000, ErrorMessage = "Asukoha nimetus liiga pikk!"), MinLength(2, ErrorMessage = "Asukoht peab sisaldama vähemalt 2 märki!")]
        [DisplayName("Koht")]
        [Required(ErrorMessage = "Väli koht on kohustuslik!")]

        public string Location { get; set; }= default!;
        [DisplayName("Lisainfo")]
        [MaxLength(1000)]
        public string? Comment { get; set; }

        public IEnumerable<Participant>? Participants { get; set; }

        public ICollection<Person>? Persons { get; set; }
        public ICollection<Company>? Companies { get; set; }
    }
}