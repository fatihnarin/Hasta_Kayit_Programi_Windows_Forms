﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HastaKayıtProgramı
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HastaKayitProgramiEntities1 : DbContext
    {
        public HastaKayitProgramiEntities1()
            : base("name=HastaKayitProgramiEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Danisanlar> Danisanlar { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilar { get; set; }
        public virtual DbSet<AktifRandevular> AktifRandevular { get; set; }
        public virtual DbSet<DanisanMuayeneTablosu> DanisanMuayeneTablosu { get; set; }
    }
}