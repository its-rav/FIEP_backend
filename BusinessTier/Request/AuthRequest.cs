using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Request
{
    public class AuthRequest
    {
        public string idToken { get; set; }
        public string fcmToken { get; set; } = null;
    }
}
