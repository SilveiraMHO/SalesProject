using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context; //declarando uma dependência para o DBContext.

        public SellerService(SalesWebMVCContext context) //Contrutor para que a injeção de dependencia possa ocorrer.
        {
            _context = context;
        }

        public List<Seller> FindAllSellers()
        {
            return _context.Seller
                .OrderBy(x => x.Name)
                .ToList();
        }

        public void CreateSeller(Seller l_seller)
        {
            _context.Add(l_seller);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller
                .Include(x => x.Department)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Remove(int id)
        {
            var l_sellerRemove = _context.Seller
                .Find(id);

            _context.Seller.Remove(l_sellerRemove);
            _context.SaveChanges();
        }
    }
}
