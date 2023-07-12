using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;


namespace AppAzure.Models
{
    public class Funcionario : ITableEntity
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Ramal { get; set; }
        public string EmailProfissional { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Salario { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTimeOffset? DataAdmissao { get; set; }

        public string Departamento { get; set; }

        public TipoAcao TipoAcao { get; set; }
        public string PartitionKey { get ; set ; }
        public string RowKey { get ; set ; }
        public DateTimeOffset? Timestamp { get ; set ; }
        public ETag ETag { get ; set ; }
    }
}
