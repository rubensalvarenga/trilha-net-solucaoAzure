using AppAzure.Models;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


namespace AppAzure.Controllers
{
    public class FuncionarioController : Controller
    {

        private readonly string _connectionString;
        private readonly string _tableName;


        public FuncionarioController(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("SAConnectionString");
            _tableName = configuration.GetValue<string>("AzureTableName");

        }

        private TableClient GetTableClient()
        {
            var serviceClient = new TableServiceClient(_connectionString);
            var tableClient = serviceClient.GetTableClient(_tableName);

            tableClient.CreateIfNotExists();

            return tableClient;
        }



        public IActionResult Index()
        {
            var tableClient = GetTableClient();
            var funcionarios = tableClient.Query<Funcionario>().ToList();


            return View(funcionarios);
        }

        public IActionResult Log()
        {
            var tableClient = GetTableClient();
            var funcionarios = tableClient.Query<Funcionario>().ToList();


            return View(funcionarios);
        }

        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(Funcionario funcionario)
        {
            var tableClient = GetTableClient();
            funcionario.RowKey = Guid.NewGuid().ToString();
            funcionario.PartitionKey = funcionario.RowKey;
            funcionario.TipoAcao = TipoAcao.Inclusao;
            var transforma = funcionario.RowKey.Substring(funcionario.RowKey.Length - 8);
            string numeros = Regex.Replace(transforma, "[^0-9]", "");
            funcionario.Id = int.Parse(numeros);

            tableClient.UpsertEntity(funcionario);

            return View(funcionario);
        }


        public IActionResult Editar(string id)
        {
            var tableClient = GetTableClient();
            var funcionario = tableClient.GetEntity<Funcionario>(id, id).Value;

            if (funcionario == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(funcionario);
        }

        [HttpPost]
        public IActionResult Editar(Funcionario funcionario)
        {
            var tableClient = GetTableClient();
            var funcionarioBanco = tableClient.GetEntity<Funcionario>(funcionario.RowKey,funcionario.RowKey).Value;


            funcionarioBanco.Nome = funcionario.Nome;
            funcionarioBanco.Endereco = funcionario.Endereco;
            funcionarioBanco.Ramal = funcionario.Ramal;
            funcionarioBanco.EmailProfissional = funcionario.EmailProfissional;
            funcionarioBanco.Departamento = funcionario.Departamento;
            funcionarioBanco.Salario = funcionario.Salario;
            funcionarioBanco.DataAdmissao = funcionario.DataAdmissao;
            funcionarioBanco.TipoAcao = TipoAcao.Atualizacao;

            tableClient.UpsertEntity(funcionarioBanco);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Detalhes(string id, Funcionario funcionario)
        {
            var tableClient = GetTableClient();
            var funcionarioTable = tableClient.GetEntity<Funcionario>(id, id).Value;

            if (funcionarioTable == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(funcionarioTable);
        }


        public IActionResult Excluir(string id)
        {
            var tableClient = GetTableClient();
            var funcionario = tableClient.GetEntity<Funcionario>(id, id).Value;

            if (funcionario == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(funcionario);

        }

        [HttpPost]
        public IActionResult Excluir(Funcionario funcionario)
        {
            var tableClient = GetTableClient();
            tableClient.DeleteEntity(funcionario.RowKey, funcionario.RowKey);

            return RedirectToAction(nameof(Index));

        }

    }
}
