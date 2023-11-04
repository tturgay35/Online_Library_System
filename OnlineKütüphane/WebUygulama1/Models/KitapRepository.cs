﻿using WebUygulama1.Models.Utility;

namespace WebUygulama1.Models
{
    public class KitapRepository : Repository<Kitap>, IKitapRepository
    {
        private  UygulamaDbContext _uygulamaDbContext;

        public KitapRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext= uygulamaDbContext;
        }

        public void Guncelle(Kitap kitap)
        {
            _uygulamaDbContext.Update(kitap);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
