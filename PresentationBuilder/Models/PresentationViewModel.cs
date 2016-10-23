using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationBuilder.Models
{
    public class PresentationViewModel
    {
        public ApplicationUser UserInfo { get; set; }
        public List<Presentation> Presentations { get; set; }
    }
}