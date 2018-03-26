using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterBot
{
    public class Agendamento
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Cidade { get; set; }
        public string Acomodação { get; set; }
        public string Escola { get; set; }
        public string Tempo { get; set; }
        public string Adicionais { get; set; }
        public string VisitaNaAgencia { get; set; }
    }
}
