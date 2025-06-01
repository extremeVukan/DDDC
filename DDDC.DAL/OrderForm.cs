namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderForm")]
    public partial class OrderForm
    {
        [Key]
        public int OrderID { get; set; }

        [StringLength(50)]
        public string OrderNumber { get; set; }

        public int? ClientID { get; set; }

        [StringLength(50)]
        public string ShipName { get; set; }

        public int? ShipID { get; set; }

        public int? OwnerID { get; set; }

        [StringLength(50)]
        public string OwnerName { get; set; }

        [StringLength(200)]
        public string PrePosition { get; set; }

        [StringLength(200)]
        public string Destination { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        public DateTime? Start_Time { get; set; }

        public DateTime? End_Time { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string img { get; set; }

        [StringLength(50)]
        public string Distance { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
