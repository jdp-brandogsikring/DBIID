﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Domain.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string PushUrl { get; set; } = string.Empty;
        public bool EnablePush { get; set; } = false;


        public List<LinkApplicationCompany> Links { get; set; } = new();
    }
}
