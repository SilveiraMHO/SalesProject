using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return _context.Seller.ToList();
        }

        public void CreateSeller(Seller l_seller)
        {
            _context.Add(l_seller);
            _context.SaveChanges();
        }
    }
}
