using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
