using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMVCContext _context;

        public DepartmentService(SalesWebMVCContext context)
        {
            _context = context;
        }

        //Tasks (async, await): Objeto que encapsula o processamento assincrono.
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync(); //Essa execucao nao irá bloquear a aplicacao, pois é uma chamada assincrona.
            //Expressao Link não executa no banco, só prepara a chamada.
            //Neste caso, o que provoca sua execucao é o .ToList(), que por sua vez é uma operacao sincrona (App fica bloqueada executando o ToList()).
            //.ToList() -> LINQ | .ToListAsync() -> EF
            //O await avisa ao compilador que se trata de uma chamada assincrona.
        }
    }
}
