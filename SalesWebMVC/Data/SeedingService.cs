using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Data
{
    public class SeedingService //Serviço criado para popular as tabelas do banco de dados.
    {
        private SalesWebMVCContext _context;

        public SeedingService(SalesWebMVCContext context)
        {
            _context = context;
        }
        //Feita a injeção de dependência. Sempre que um SeedingService for criado, irá receber uma instância do context.

        public void Seed() //Método responsável por popular a base de dados.
        {
            //Se já existe algum dado nas entidades abaixo, a operação será interrompida.
            if(_context.Department.Any() || _context.SalesRecord.Any() || _context.Seller.Any())
            {
                return;
            }

            //Como estamos utilizando o WorkFlow CodeFirst (cria objetos pra depois criar base de dados)
            //iremos fazer a instanciação desses objetos e salvar na base de dados.

            Department d1 = new Department(1, "Computers");
            Department d2 = new Department(2, "Electronics");
            Department d3 = new Department(3, "Fashion");
            Department d4 = new Department(4, "Books");

            Seller s1 = new Seller(1, "Bob Brown", "bob.brown@gmail.com", new DateTime(1990, 12, 03), 2500.0, d1);
            Seller s2 = new Seller(2, "Maria Gutz", "maria.gutz@gmail.com", new DateTime(1985, 7, 21), 3500.0, d2);
            Seller s3 = new Seller(3, "Michael Eggers", "michael.eggers@gmail.com", new DateTime(1979, 1, 9), 3500.0, d3);
            Seller s4 = new Seller(4, "Erika Lins", "erika.lins@gmail.com", new DateTime(1993, 10, 13), 1800.0, d4);
            Seller s5 = new Seller(5, "Luke Stanford", "luke.stanford@gmail.com", new DateTime(1984, 6, 20), 2800.0, d2);

            SalesRecord sr1 = new SalesRecord(1, new DateTime(2020, 01, 12, 10, 12, 45), 580.0, Models.Enums.SaleStatus.Billed, s2);
            SalesRecord sr2 = new SalesRecord(2, new DateTime(2020, 02, 3, 13, 32, 11), 1020.0, Models.Enums.SaleStatus.Billed, s5);
            SalesRecord sr3 = new SalesRecord(3, new DateTime(2020, 02, 4, 15, 10, 23), 210.0, Models.Enums.SaleStatus.Billed, s4);

            //Adicionando e salvando todos os objetos acima no banco.
            _context.Department.AddRange(d1, d2, d3, d4); //O Método AddRange permite que adicionemos varios objetos de uma só vez.
            _context.Seller.AddRange(s1, s2, s3, s4, s5);
            _context.SalesRecord.AddRange(sr1, sr2, sr3);

            _context.SaveChanges();
            
        }
        
    }
}
