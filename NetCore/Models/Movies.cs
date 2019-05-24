using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Models
{
    public class Movies
    {
        public int Id { get; set; }

        public string name { get; set; }

        public DateTime SynTime { get; set; }
        /// <summary>
        /// ef core 并发字段
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
