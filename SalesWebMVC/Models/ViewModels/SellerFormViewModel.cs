using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Models.ViewModels
{
    public class SellerFormViewModel //Classe que contem os dados para o formulario de cadastro de vendedor
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }

        public SellerFormViewModel()
        {
        }

        public SellerFormViewModel(Seller seller, ICollection<Department> departments)
        {
            Seller = seller;
            Departments = departments;
        }
    }
}
