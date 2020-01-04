using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}
