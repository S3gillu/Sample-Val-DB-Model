using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SampleValDBModel.Models;
using System.Data.Entity;

namespace SampleValDBModel.Models
{
    public class LoginDBContext:DbContext
    {

        public LoginDBContext()
           : base("AccountDataBase")
        { }
        public DbSet<UserAccount> UserAccounts  { get; set; }

    }
}