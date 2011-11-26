using System;

namespace Market.Cqrsnes.WebUi.Models
{
    public class ChangeCountViewModel
    {
        public Guid Id { get; set; }

        public int Count { get; set; }

        public string Deliver { get; set; }

        public bool IsDelivery
        {
            get
            {
                return !string.IsNullOrEmpty(Deliver);
            }
        }
    }
}