using System;

namespace Pozdravlyator.Models
{
    public class Birthday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? PhotoPath { get; set; }
        public string? Comment { get; set; }
    }
} 