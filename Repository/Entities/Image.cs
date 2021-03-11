using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Repository.Entities
{
    public partial class Image
    {
        public int Idimage { get; set; }
        public string Image1 { get; set; }
        public int? ParentKey { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]   
        public virtual Product Product { get; set; }
    }
}
