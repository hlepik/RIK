using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Company : DomainEntityId
    {
        [DisplayName("Ettevõte juriidiline nimi")]
        [Required(ErrorMessage = "Ettevõte juriidiline nimi väli on kohustuslik")]

        [MaxLength(500, ErrorMessage = "Ettevõtte nimetus on liiga pikk!"), MinLength(2, ErrorMessage = "Ettevõtte nimetus peab olema vähemalt 2 märki pikk!")] public string CompanyName { get; set; } = default!;

        [MaxLength(20, ErrorMessage = "Registrikood on liiga pikk!"), MinLength(5, ErrorMessage = "Registrikood peab olema vähemalt 5 märki pikk!")]
        [Required(ErrorMessage = "Registrikood väli on kohustuslik")]

        [DisplayName("Registrikood")]
        public string RegisterCode { get; set; } = default!;
        [Required(ErrorMessage = "Palun lisage inimeste arv!")]

        [DisplayName("Inimeste arv")]
        public int PeopleCount { get; set; }
        [DisplayName("Makseviis")]
        public PaymentType PaymentType { get; set; }
        [MaxLength(5000)]
        [DisplayName("Lisainfo")]
        public string? Comment { get; set; }
        public Guid EventId { get; set;}
        public Event? Event { get; set; }

    }
}