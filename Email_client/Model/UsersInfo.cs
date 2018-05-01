using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_client.Model
{
   public class UsersInfo
    {
        [Key]
        public int Id{ get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(450)]
        public string Login { get; set; }
        [MinLength(3)]
        public string Password { get; set; }
    }
}
