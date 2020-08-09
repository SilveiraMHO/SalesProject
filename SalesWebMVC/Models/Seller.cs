using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} required.")] //Notation para campo obrigatório na View.
        [StringLength(60, MinimumLength = 3, ErrorMessage = "The {0} size should be between {2} and {1}.")] //Nessa Notation o framework tem uma mensagem padão, mas é possível personalizar, conforme exemplificado.
        //Na Notation 'StringLength', ainda é possível parametrizar a mensagem de erro. {0} = Nome do atributo. {1} = Primeiro parametro, {2} = Segundo parametro.
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "{0} required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")] //Validacao para email.
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Nascimento")]
        [Required(ErrorMessage = "{0} required.")]
        [DataType(DataType.Date)] //Para tirar as horas na View.
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] //Para formatar data do Brasil.
        public DateTime BirthDate { get; set; }

        [Display(Name = "Salário Base")]
        [Required(ErrorMessage = "{0} required.")]
        [Range(100.0, 50000.0, ErrorMessage = "The {0} must be from {1} to {2}")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }
        
        [Display(Name = "Departamento")]
        public Department Department { get; set; }
        
        public int DepartmentId { get; set; }
        
        public ICollection<SalesRecord> SalesRecords { get; set; } = new List<SalesRecord>();



        public Seller()
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            SalesRecords.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            SalesRecords.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return SalesRecords.Where(x => x.Date >= initial && x.Date <= final)
                .Sum(x => x.Amout);
        }
    }
}
