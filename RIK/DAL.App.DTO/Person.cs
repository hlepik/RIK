using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Person: DomainEntityId
    {

        [MaxLength(256, ErrorMessage = "Eesnimi on liiga pikk!"), MinLength(2, ErrorMessage = "Eesnimi peab sisaldama vähemalt 2 märki!")]
        [Required(ErrorMessage = "Eesnimi väli on kohustuslik")]

        [DisplayName("Eesnimi")]
        public string FirstName { get; set; } = default!;
        [MaxLength(256, ErrorMessage = "Perekonnanimi on liiga pikk!"), MinLength(2, ErrorMessage = "Perekonnanimi peab sisaldama vähemalt 2 märki!")]
        [Required(ErrorMessage = "Perekonnanimi väli on kohustuslik")]

        [DisplayName("Perekonnanimi")]
        public string LastName { get; set; } = default!;
        [MaxLength(20, ErrorMessage = "Isikukood on liiga pikk!"), MinLength(5, ErrorMessage = "Isikukood peab sisaldama vähemalt 5 märki!")]
        [DisplayName("Isikukood")]
        [Required(ErrorMessage = "Isikukood väli on kohustuslik")]

        public string IdentificationCode { get; set; } = default!;
        [DisplayName("Makseviis")]
        [Required(ErrorMessage = "Palun valige makseviis!")]
        public PaymentType PaymentType { get; set; }
        [MaxLength(1500)]
        [DisplayName("Lisainfo")]
        public string? Comment { get; set; }

        public Guid EventId { get; set;}
        public Event? Event { get; set; }
    }
}