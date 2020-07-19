using SalesWebMVC.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace SalesWebMVC.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amout { get; set; }
        public SaleStatus Status { get; set; }
        public Seller Sellers { get; set; }

        public SalesRecord()
        {
        }

        public SalesRecord(int id, DateTime date, double amout, SaleStatus status, Seller sellers)
        {
            Id = id;
            Date = date;
            Amout = amout;
            Status = status;
            Sellers = sellers;
        }
    }
}
