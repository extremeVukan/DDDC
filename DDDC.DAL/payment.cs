namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("payment")]
    public partial class payment
    {
        [Key]
        public int payment_id { get; set; }

        public int? order_id { get; set; }

        [StringLength(50)]
        public string payment_way { get; set; }

        [StringLength(50)]
        public string payment_price { get; set; }

        [StringLength(50)]
        public string payment_status { get; set; }

        public DateTime? payment_day { get; set; }

        [StringLength(100)]
        public string transaction_id { get; set; }
    }
}
