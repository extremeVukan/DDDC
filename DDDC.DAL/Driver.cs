namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Driver")]
    public partial class Driver
    {
        [Key]
        public int Driver_id { get; set; }

        [StringLength(50)]
        public string Driver_name { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        public int? ship_id { get; set; }

        [StringLength(50)]
        public string ship_name { get; set; }

        [StringLength(50)]
        public string position { get; set; }
    }
}
