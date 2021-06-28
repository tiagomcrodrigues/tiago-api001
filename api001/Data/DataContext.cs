using api001.Models;
using Microsoft.EntityFrameworkCore;


namespace api001.Data
{
    /// <summary>
    /// Classe para mapear a base de dados
    /// </summary>
    public class DataContext : DbContext
    {

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        /// <summary>
        /// Propriedade para mapear a tabela Product
        /// </summary>
        public DbSet<Product> Products { get; set; }
        
        /// <summary>
        /// Propriedade para mapear a tabela Category
        /// </summary>
        public DbSet<Category> Categories  { get; set; }
        
        /// <summary>
        /// Properidade para mapear a tabela User
        /// </summary>
        public DbSet<User> Users { get; set; }


    }
}
