namespace DDDC.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ships
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ship_id { get; set; }

        public int? owner_id { get; set; }

        [StringLength(50)]
        public string ship_name { get; set; }

        [StringLength(50)]
        public string ship_type { get; set; }

        public int? capacity { get; set; }

        [StringLength(50)]
        public string ship_status { get; set; }

        public DateTime? ship_reg_time { get; set; }

        [StringLength(200)]
        public string Picture { get; set; }

        [StringLength(50)]
        public string province { get; set; }

        [StringLength(50)]
        public string city { get; set; }

        [StringLength(50)]
        public string Position { get; set; }
    }
}
