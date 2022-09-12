namespace PruebaTecnicaPichincha.Models
{
    public class ReportesModel
    {
        public DateTime Fecha { get; set; }
        public string? Cliente { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public decimal Movimiento { get; set; }
        public decimal SaldoDisponible { get; set; }

    }
}
