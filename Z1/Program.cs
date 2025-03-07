using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CarDataAnalysis
{
    public class Car
    {
        public int id { get; set; }
        public string? marka { get; set; }
        public string? model { get; set; }
        public int rok_modelowy { get; set; }
        public string? vin { get; set; }
        public string? kolor { get; set; }
        public int przebieg { get; set; }
        public string? rodzaj_silnika { get; set; }
        public string? skrzynia_biegow { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            string filePath = "C:/Users/kamil/Downloads/MOCK_DATAA.json";

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                List<Car> cars = JsonConvert.DeserializeObject<List<Car>>(jsonContent);

                if (cars == null)
                {
                    Console.WriteLine("Nie udało się zdeserializować danych z pliku JSON.");
                    return;
                }

                
                var carCounts = cars
                    .GroupBy(car => car.marka)
                    .Select(group => new
                    {
                        Marka = group.Key,
                        Ilosc = group.Count()
                    })
                    .OrderByDescending(x => x.Ilosc);


                
                Console.WriteLine("Ilość pojazdów każdej marki:");
                foreach (var carCount in carCounts)
                {
                    Console.WriteLine($"{carCount.Marka}: {carCount.Ilosc}");
                }
                

                //Zapytanie 1:  Znajdź średni przebieg dla samochodów marki Ford.
                var averageFordMileage = cars.Where(car => car.marka == "Ford").Average(car => car.przebieg);
                Console.WriteLine($"\nŚredni przebieg dla samochodów marki Ford: {averageFordMileage}");

                //Zapytanie 2:  Wyświetl marki samochodów wyprodukowanych po 2010 roku, posortowane alfabetycznie.
                var brandsAfter2010 = cars.Where(car => car.rok_modelowy > 2010).Select(car => car.marka).Distinct().OrderBy(marka => marka);

                Console.WriteLine("\nMarki samochodów wyprodukowanych po 2010 roku (posortowane alfabetycznie):");
                foreach (var marka in brandsAfter2010)
                {
                    Console.WriteLine(marka);
                }

                //Zapytanie 3: Znajdź 5 samochodów z najmniejszym przebiegiem.
                var fiveCarsWithLowestMileage = cars.OrderBy(car => car.przebieg).Take(5);
                Console.WriteLine("\n5 samochodów z najmniejszym przebiegiem:");
                foreach (var car in fiveCarsWithLowestMileage)
                {
                    Console.WriteLine($"{car.marka} {car.model}: {car.przebieg}");
                }

                //Zapytanie 4: Policz, ile jest samochodów z automatyczną skrzynią biegów.
                long automaticCarsCount = cars.Count(car => car.skrzynia_biegow.ToLower() == "automatyczna");

                Console.WriteLine($"\nLiczba samochodów z automatyczną skrzynią biegów: {automaticCarsCount}");


                //Zapytanie 5: Znajdź najstarszy samochód (najmniejszy rok modelowy)
                var oldestCar = cars.OrderBy(car => car.rok_modelowy).FirstOrDefault();

                if (oldestCar != null)
                {
                    Console.WriteLine($"\nNajstarszy samochód: {oldestCar.marka} {oldestCar.model} ({oldestCar.rok_modelowy})");
                }
                else
                {
                    Console.WriteLine($"\nBrak danych o samochodach.");
                }



            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Plik '{filePath}' nie został znaleziony.");
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine($"Błąd podczas deserializacji pliku JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
