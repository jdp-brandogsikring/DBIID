using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Shared.Features.Applications
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string PushUrl { get; set; } = string.Empty;
        public bool EnablePush { get; set; } = false;
    }
}
