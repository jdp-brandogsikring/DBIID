using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Login
{
    public class ContactMethodDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // fx Email / Phone
        public string Value { get; set; } = string.Empty; // This is the email or phone number
        public string Description { get; set; } = string.Empty; // fx Privat / Mobil

    }
}
