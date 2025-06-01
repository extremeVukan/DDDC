namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AfterSales
    {
        [Key]
        public int ServicesID { get; set; }

        [StringLength(50)]
        public string ordernumber { get; set; }

        public int? UserID { get; set; }

        public int? Shipid { get; set; }

        public DateTime? ApplicationDate { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string AdminComment { get; set; }
    }
}
