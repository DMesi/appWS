//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace apiWS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Proizvod
    {
        public int Id { get; set; }
        public Nullable<int> Id_kat { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public Nullable<double> Cena { get; set; }
        public Nullable<int> Lager { get; set; }
        public string Slika { get; set; }
    
        public virtual Kategorija Kategorija { get; set; }
    }
}
