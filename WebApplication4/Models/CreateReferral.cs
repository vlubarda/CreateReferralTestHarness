using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreateReferralTestHarness.Models
{
    public class CreateReferral
    {
        [Required]
        public string AccountId { get; set; }

        [Required]
        public Referral[] Referrals { get; set; }
    }

    public class Referral
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}