using System;

namespace DAL.App.DTO
{
    public class Participant
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}