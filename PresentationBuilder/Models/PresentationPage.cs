//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PresentationBuilder.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PresentationPage
    {
        public int PresentationPageId { get; set; }
        public int PresentationId { get; set; }
        public string ImagePath { get; set; }
        public string SoundPath { get; set; }
        public Nullable<int> SoundLength { get; set; }
        public byte Order { get; set; }
        public bool Hidden { get; set; }
    
        public virtual Presentation Presentation { get; set; }
    }
}