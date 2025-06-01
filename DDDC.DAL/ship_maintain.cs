namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ship_maintain
    {
        [Key]
        public int maintain_id { get; set; }

        public int? ship_id { get; set; }

        [StringLength(50)]
        public string maintain_type { get; set; }

        [Column(TypeName = "date")]
        public DateTime? maintain_date { get; set; }

        [Column(TypeName = "text")]
        public string description { get; set; }
    }
}
