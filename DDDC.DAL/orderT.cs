namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("orderT")]
    public partial class orderT
    {
        [Key]
        public int Listid { get; set; }

        public int order_id1 { get; set; }

        [StringLength(50)]
        public string orderNumber { get; set; }

        public int? passenger_id { get; set; }

        public int? ship_id { get; set; }

        [StringLength(50)]
        public string order_status { get; set; }

        [StringLength(255)]
        public string star_location { get; set; }

        [StringLength(255)]
        public string end_location { get; set; }

        [StringLength(255)]
        public string ship_locetion { get; set; }

        public DateTime? start_time { get; set; }

        public DateTime? end_time { get; set; }

        public decimal? total_price { get; set; }

        [StringLength(50)]
        public string estimate { get; set; }

        [StringLength(50)]
        public string payment_status { get; set; }
    }
}
