﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WhatsAppApi.Parser
{
    public class PhoneNumber
    {
        public string Country;
        public string CC;
        public string Number;
        public string FullNumber
        {
            get
            {
                return this.CC + this.Number;
            }
        }
        public string ISO3166;
        public string ISO639;
        public string MCC;

        public PhoneNumber(string number)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WhatsAppApi.Parser.countries.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string csv = reader.ReadToEnd();
                    string[] lines = csv.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] values = line.Split(new char[] { ',' });
                        //try to match
                        if (number.StartsWith(values[1]))
                        {
                            //matched
                            this.Country = values[0].Trim(new char[] { '"' });
                            this.CC = values[1];
                            this.Number = number.Substring(this.CC.Length);
                            this.ISO3166 = values[3].Trim(new char[] { '"' });
                            this.ISO639 = values[4].Trim(new char[] { '"' });
                            this.MCC = values[2];
                            return;
                        }
                    }
                    //could not match!
                    throw new Exception(String.Format("Could not dissect phone number {0}", number));
                }
            }
        }
    }
}
