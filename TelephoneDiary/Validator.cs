using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace TelephoneDiary
{
    static class Validator
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                return address.Address == email;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsValidString(string s)
        {
            return s.Length != 0;
        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            try
            {
                if (!IsValidString(phoneNumber)) return false;
                Regex rgx = new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
                return rgx.IsMatch(phoneNumber);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
