using System;
using System.Collections.Generic;
using System.Text;

namespace BookOrder.Services.SingleSignOn
{
    //TODO potential deprecation of this class
    public static class SSOConfigurationSettings
    {
        public static string CASLoginUrl = "https://login.ewu.edu/cas/";
        //this is the default ending"%2Flogin%2Fcas" consider investigating whether or not these url encodings are necessary server side
        public static string ApplicationUrl = "146.187.134.48:4200/authentication";
        //TODO consider investigating whether we need these encodings here or not "https%3A%2F%2F"
        public static string CASLoginParameters = $"login?service=http://{ApplicationUrl}";
        public static string CASAuthenticationUrl = "https://login.ewu.edu/cas/serviceValidate";
    }
}
