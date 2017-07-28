using Kvite.Web.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kvite.Web.Models.Requests
{
    public class AddContactsRequest
    {

        [StringLength(128), Required]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [Range(0, 99999999999)]
        public long? MobilePhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Url, StringLength(1000)]
        public string PhotoUrl { get; set; }

        public string SourceId { get; set; }

        public ContactSourceKinds? SourceType { get; set; }

        public string ContactUserId { get; set; }

        public string OwnerUserId { get; set; }
    }
}