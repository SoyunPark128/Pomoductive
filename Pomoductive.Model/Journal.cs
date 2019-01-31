using System;
using System.Collections.Generic;
using System.Text;

namespace Pomoductive.Models
{
    public class Journal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime JournalDate { get; } = DateTime.Today;
        public string JournalContents { get; set; } = "";
    }
}
