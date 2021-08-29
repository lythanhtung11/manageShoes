using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectWeb.Models
{
    public partial class User
    {
        public User()
        {
            Profiles = new HashSet<Profile>();
            Receipts = new HashSet<Receipt>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string Datejoin { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
