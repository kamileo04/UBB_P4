using System.Data.SqlClient;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace DapperContribExample
{
    [Table("Produkty")]
    public class Product
    {
        [ExplicitKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }



        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Price: {Price}, Stock: {Stock}";
        }
    }

    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return (int)connection.Insert(product);
            }
        }

        public Product GetProduct(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<Product>(id);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Product>();
            }
        }

        public bool UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Update(product);
            }
        }

        public bool DeleteProduct(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var product = new Product { Id = id };
                return connection.Delete(product);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=Kamil_Redmibook;Initial Catalog=BiblioP4;Integrated Security=True";
            var productRepository = new ProductRepository(connectionString);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Wybierz akcję:");
                Console.WriteLine("1. Dodaj produkt");
                Console.WriteLine("2. Wyświetl wszystkie produkty");
                Console.WriteLine("3. Edytuj produkt");
                Console.WriteLine("4. Usuń produkt");
                Console.WriteLine("5. Wyjdź");
                Console.Write("Wybór: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct(productRepository);
                        break;
                    case "2":
                        ShowAllProducts(productRepository);
                        break;
                    case "3":
                        EditProduct(productRepository);
                        break;
                    case "4":
                        DeleteProduct(productRepository);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Naciśnij dowolny klawisz, aby kontynuować.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddProduct(ProductRepository repo)
        {
            Console.Write("Podaj ID produktu: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Podaj nazwę produktu: ");
            string name = Console.ReadLine();
            Console.Write("Podaj cenę produktu: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Podaj stan magazynowy: ");
            int stock = int.Parse(Console.ReadLine());

            var product = new Product { Id = id, Name = name, Price = price, Stock = stock };
            try
            {
                int newId = repo.AddProduct(product);
                Console.WriteLine($"Dodano produkt z ID: {id}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić.");
            Console.ReadKey();
        }

        static void ShowAllProducts(ProductRepository repo)
        {
            var products = repo.GetAllProducts();
            bool hasProducts = false;
            Console.WriteLine("\nWszystkie produkty:");
            foreach (var p in products)
            {
                Console.WriteLine(p);
                hasProducts = true;
            }
            if (!hasProducts)
            {
                Console.WriteLine("Brak produktów w bazie.");
            }
            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić.");
            Console.ReadKey();
        }

        static void EditProduct(ProductRepository repo)
        {
            Console.Write("Podaj ID produktu do edycji: ");
            int id = int.Parse(Console.ReadLine());
            var product = repo.GetProduct(id);

            if (product != null)
            {
                Console.WriteLine($"Aktualny produkt: {product}");
                Console.Write("Nowa nazwa (Enter, aby pominąć): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name)) product.Name = name;

                Console.Write("Nowa cena (Enter, aby pominąć): ");
                string priceInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(priceInput)) product.Price = decimal.Parse(priceInput);

                Console.Write("Nowy stan magazynowy (Enter, aby pominąć): ");
                string stockInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(stockInput)) product.Stock = int.Parse(stockInput);

                if (repo.UpdateProduct(product))
                {
                    Console.WriteLine("Produkt zaktualizowany.");
                }
                else
                {
                    Console.WriteLine("Aktualizacja nie powiodła się.");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono produktu o podanym ID.");
            }
            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić.");
            Console.ReadKey();
        }

        static void DeleteProduct(ProductRepository repo)
        {
            Console.Write("Podaj ID produktu do usunięcia: ");
            int id = int.Parse(Console.ReadLine());
            if (repo.DeleteProduct(id))
            {
                Console.WriteLine("Produkt usunięty.");
            }
            else
            {
                Console.WriteLine("Usunięcie nie powiodło się lub produkt nie istnieje.");
            }
            Console.WriteLine("Naciśnij dowolny klawisz, aby wrócić.");
            Console.ReadKey();
        }
    }
}
