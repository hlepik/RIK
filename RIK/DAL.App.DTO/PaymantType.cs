using System.ComponentModel.DataAnnotations;

namespace DAL.App.DTO
{
    public enum PaymentType
    {
        [Display(Name = "Sularaha")]
        Cash,
        [Display(Name = "Ülekanne")]
        Transfer
    }
}