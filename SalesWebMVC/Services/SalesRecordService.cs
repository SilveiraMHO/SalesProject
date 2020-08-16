using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var l_salesList = from obj in _context.SalesRecord select obj;
            
            if (minDate.HasValue)
            {
                l_salesList = _context.SalesRecord.Where(x => x.Date >= minDate);
            }
            if (maxDate.HasValue)
            {
                l_salesList = _context.SalesRecord.Where(x => x.Date <= maxDate);
            }

            return await l_salesList
                .Include(x => x.Sellers)
                .Include(x => x.Sellers.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var l_salesList = from obj in _context.SalesRecord select obj;

            if (minDate.HasValue)
            {
                l_salesList = _context.SalesRecord.Where(x => x.Date >= minDate);
            }
            if (maxDate.HasValue)
            {
                l_salesList = _context.SalesRecord.Where(x => x.Date <= maxDate);
            }

            return await l_salesList
                .Include(x => x.Sellers)
                .Include(x => x.Sellers.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Sellers.Department) //Tipo de retorno agora passa a ser uma lista de IGrouping<Dep,SR>.
                .ToListAsync();
        }
    }
}
