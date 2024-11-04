using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockID { get; set; } // used to form the relation 1-to-many. List<Comment> Comments in Stock.cs
        public Stock? Stock { get; set; } // Navigation property. What allows me to navigate in my Model. (I can do Stock.Company etc...)
        public string AppUserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; }

    }
}