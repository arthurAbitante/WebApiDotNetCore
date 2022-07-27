using APIHavan.Controllers;
using APIHavan.Data;
using APIHavan.Test.Constants;
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
        [Test]
        public async Task testePegaTodosClientes()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetClientes();

            Assert.AreEqual(2, result.Value.Count());

            Funcoes.RemoverListaClientes(context);
        }


        [Test]
        public async Task testePegaClientePorId()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };
            
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetCliente(1);

            Assert.AreEqual(cliente.clienteId, result.Value.clienteId);

            Funcoes.RemoverListaClientes(context);
        }

        [Test]
        public async Task testeInsereClientes()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ClientesController(context);

            var result = await query.PostCliente(cliente);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Cliente;

            Assert.AreEqual(1, item.clienteId);

            Funcoes.RemoverCliente(context, item);
        }

        [Test]
        public async Task testeEditaClientes()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ClientesController(context);

            await query.PostCliente(cliente);

            cliente.cnpj = "novoCnpj";
            cliente.razaoSocial = "novaRazaoSocial";

            var resultNovo = await query.PutCliente(cliente.clienteId, cliente) as StatusCodeResult;

            Assert.AreEqual(204, resultNovo.StatusCode);

            Funcoes.RemoverCliente(context, cliente);
        }

        [Test]
        public async Task testeRemoveClientes()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ClientesController(context);

            var result = await query.PostCliente(cliente);

            var response = result.Result as CreatedAtActionResult;
            var item = response.Value as Cliente;

            Assert.AreEqual(1, item.clienteId);

            var resultDelete = await query.DeleteCliente(cliente.clienteId) as StatusCodeResult;

            Assert.AreEqual(204, resultDelete.StatusCode);
        }

        [Test]
        public async Task RetornaTipoCorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetClientes();

            Assert.IsInstanceOf<ActionResult<IEnumerable<Cliente>>>(result);
            Funcoes.RemoverListaClientes(context);
        }

        [Test]
        public async Task RetornaIdIncorreto()
        {
            var context = new AppDbContext(Funcoes.options);

            Funcoes.PopularClientes(context);

            var query = new ClientesController(context);

            var result = await query.GetCliente(99);
            var statusCode = result.Result as StatusCodeResult;
            Assert.IsInstanceOf<NotFoundResult>(statusCode);

            Funcoes.RemoverListaClientes(context);
        }

        [Test]
        public async Task RetornaNaoEncontradoQuandoIdEInvalido()
        {
            var cliente = new Cliente { clienteId = 1, cnpj = "cnpjteste", razaoSocial = "razaosocialteste", email = "email1@email.com" };

            var context = new AppDbContext(Funcoes.options);

            var query = new ClientesController(context);

           var result = await query.PutCliente(99, cliente);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task testaDeleteQuandoIdInvalido()
        {
            var context = new AppDbContext(Funcoes.options);

            var query = new ClientesController(context);

            var resultDelete = await query.DeleteCliente(99) as StatusCodeResult;

            Assert.AreEqual(404, resultDelete.StatusCode);
        }
    }
}