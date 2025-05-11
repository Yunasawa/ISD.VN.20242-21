using System;

namespace MediaStore.Models
{
    public class MediaProperty
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public virtual Media Media { get; set; }
    }
} 