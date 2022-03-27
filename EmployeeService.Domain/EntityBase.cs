using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeService.Domain
{
    public class EntityBase
    {
        [Column("LAST_UPDT_DATE")]
        public DateTime LastUpdtDate { get; set; }

        [Column("LAST_UPDT_USER_ID", TypeName = "VARCHAR2")]
        public string LastUpdtUserId { get; set; }
    }
}
