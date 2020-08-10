using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
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

        public async Task<List<Seller>> FindAllSellersAsync()
        {
            return await _context.Seller.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task CreateSellerAsync(Seller l_seller)
        {
            _context.Add(l_seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var l_sellerRemove = await _context.Seller.FindAsync(id);

            _context.Seller.Remove(l_sellerRemove);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool l_hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
            
            if (!l_hasAny) //Testando se já existe um obj Seller no banco com o mesmo id do obj Seller que chegou no metodo.
            {
                throw new NotFoundException("Id Not Found!");
            }

            //Quando chamamos a operação de atualizar no banco de dados [.Update()] o banco pode retornar uma excessao de conflito de concorrencia.
            //Se este erro ocorrer no banco de dados, o EF vai produzir uma excessao chamada "DbUpdateConcurrencyException".
            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message); //Será passada a mensagem que veio do Banco de dados.
            }
            //Se a Exception do EF ocorrer, vai ser relançado uma outra excessao em nivel de serviço.
        }
    }
}
