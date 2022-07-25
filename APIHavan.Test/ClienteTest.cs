using APIHavan.Controllers;
using APIHavan.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIHavan.Test
{
    public class ClienteTest
    {
        private DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseMySql("Server=localhost;User Id=root;Password=123456;Database=havan", ServerVersion.AutoDetect("Server=localhost;User Id=root;Password=123456;Database=havan"))
                    .Options;

        private Cliente[] dadosCliente()
        {

            var cliente = new[] { 
                new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email="email1@email.com" }, 
                new Cliente { clienteId = 2, cnpj = "cnpjtest2", razaoSocial = "razaosocialtest2", email="email2@email.com" },
            };

            return cliente;
        }

        private void PopularClientes(AppDbContext context)
        {
            context.Clientes.AddRange(dadosCliente());
            context.SaveChanges();
        }

        private void RemoverListaClientes(AppDbContext context)
        {
            using (context = new AppDbContext(options))
            {
                context.Clientes.RemoveRange(dadosCliente());
                context.SaveChanges();
            }
        }

        private void RemoverCliente(AppDbContext context, Cliente cliente)
        {
            using (context = new AppDbContext(options))
            {
                context.Clientes.Remove(cliente);
                context.SaveChanges();
            }
        }

     

        [Test]
        public async Task testePegaTodosClientes()
        {

            var context = new AppDbContext(options);

            PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetClientes();

            Assert.AreEqual(2, result.Value.Count());

            RemoverListaClientes(context);

        }


        [Test]
        public async Task testePegaClientePorId()
        {
       
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };
            
            var context = new AppDbContext(options);

            PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetCliente(1);

            Assert.AreEqual(cliente.clienteId, result.Value.clienteId);

            RemoverListaClientes(context);
        }

        [Test]
        public async Task testeInsereClientes()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);

            var query = new ClientesController(context);

            var result = await query.PostCliente(cliente);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Cliente;

            Assert.AreEqual(1, item.clienteId);

            RemoverCliente(context, item);
        }

        [Test]
        public async Task testeEditaClientes()
        {
    
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);

            var query = new ClientesController(context);

            await query.PostCliente(cliente);

            cliente.cnpj = "novoCnpj";
            cliente.razaoSocial = "novaRazaoSocial";


            var resultNovo = await query.PutCliente(cliente.clienteId, cliente) as StatusCodeResult;

            Assert.AreEqual(204, resultNovo.StatusCode);

            RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeRemoveClientes()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(options);

            var query = new ClientesController(context);

            var result = await query.PostCliente(cliente);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Cliente;

            Assert.AreEqual(1, item.clienteId);

            var resultDelete = await query.DeleteCliente(cliente.clienteId) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }

        //
    }
}