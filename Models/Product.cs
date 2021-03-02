using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMarked.Models
{
    public class Product
    {
        public Product()
        {
            DateCreated = DateTime.Now;
        }

        public int ProductId { get; set; }

        public string Link { get; set; }

        public string Text { get; set; }

        [MinLength(8),MaxLength(8)]
        public String PhoneNr { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public string UserName { get; set; }
        

    }
}
