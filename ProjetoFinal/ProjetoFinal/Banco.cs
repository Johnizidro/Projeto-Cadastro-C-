using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace ProjetoFinal
{
    public static class Database
    {
        private static readonly string dbPath = @"C:\Users\joão\ProjetoFinal.db"; 
        private static readonly string connectionString = $"Data Source={dbPath};Version=3;";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // ✅ Método para testar a conexão
        public static bool TestarConexao()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Você pode logar isso ou apenas retornar false
                Console.WriteLine("Erro ao testar conexão: " + ex.Message);
                return false;
            }
        }
    }
}
