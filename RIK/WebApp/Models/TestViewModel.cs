using System.Collections;
using System.Collections.Generic;
using Domain.App;


namespace WebApp.ViewModels.Test
{
    public class TestViewModel
    {
        public ICollection<Event> Events { get; set; } = default!;
    }
}