namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ordernews
    {
        [Key]
        public int news_id { get; set; }

        [StringLength(255)]
        public string headText { get; set; }

        public int? Client_id { get; set; }

        public int? Driver_id { get; set; }

        [StringLength(255)]
        public string message { get; set; }

        [StringLength(50)]
        public string message_type { get; set; }

        public DateTime? send_time { get; set; }

        [StringLength(50)]
        public string read_status { get; set; }
    }
}
