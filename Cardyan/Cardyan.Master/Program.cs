using System;
using System.Linq;
using System.Collections.Generic;

namespace Cardyan
{
    //using Cardyan.Inventory.Models;
    using AgileMapperEntities;

    public class PersonApiModel
    {
        public string Name { get; set; }
    }

    public class Address
    {
        public string Name { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            //var person1 = AgileObjects.AgileMapper.Mapper.Map(new PersonApiModel()).ToANew<Person>();

            //var dictionary = new Dictionary<string, object> { ["Name"] = "Frank" };
            //var person2 = AgileObjects.AgileMapper.Mapper.Map(dictionary).ToANew<Person>();


            //var branch = new Branch { Id = 1, Name = "" };

            //for (var i = 0; i < 1000; i++)
            //    branch.Warehouses.Add(new Warehouse { Id = 1, Name = "" });

            //foreach (var warehouse in branch.Warehouses)
            //{
            //    warehouse.Branch = branch;

            //    for (var i = 0; i < 1000; i++)
            //    {
            //        var product = new Product { Id = 1, Name = "" };
            //        var wProduct = new WarehouseProduct { Product = product, Warehouse = warehouse };
            //        warehouse.Products.Add(wProduct);
            //    }
            //}

Branch Test()
{
    var branch = new Branch();
    var product = new Product();

    for (var i = 0; i < 1000; i++)
        branch.Warehouses.Add(new Warehouse());

    foreach (var warehouse in branch.Warehouses)
    {
        warehouse.Branch = branch;

        for (var i = 0; i < 1000; i++)
        {
            var wProduct = new WarehouseProduct { Product = product, Warehouse = warehouse };
            product.Warehouses.Add(wProduct);
            warehouse.Products.Add(wProduct);
        }
    }

    return branch;
}

            //AgileObjects.AgileMapper.Mapper.Unflatten(queryString).To()

            AutoMapper.Mapper.Initialize(p => { });
            var mc = new AutoMapper.MapperConfiguration(cfg => { });
            AutoMapper.IMapper sh = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg => { }));


            //System.Threading.Tasks.Parallel.For(0, Environment.ProcessorCount, (i) => AutoMapper.Mapper.Map<Branch>(Test()));


            System.Threading.Tasks.Parallel.For(0, Environment.ProcessorCount, (i) => AgileObjects.AgileMapper.Mapper.CreateNew().Map(Test()).ToANew<Branch>());
            System.Threading.Tasks.Parallel.For(0, Environment.ProcessorCount, (i) => (new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg => { })) as AutoMapper.IMapper).Map<Branch>(Test()));


            

            System.Threading.Tasks.Parallel.For(0, Environment.ProcessorCount, (i) => AgileObjects.AgileMapper.Mapper.Map(Test()).ToANew<Branch>());


            //warehouse = new Warehouse { BranchId = 36, Code = "ddd" };
            var newWarehouse = AgileObjects.AgileMapper.Mapper.Map(Test()).ToANew<Branch>();

            var newWarehouse2 = AgileObjects.AgileMapper.Mapper.Map(Test()).ToANew<Branch>();

            var newWarehouse3 = AgileObjects.AgileMapper.Mapper.Map(Test()).ToANew<Branch>();

            var newWarehouse4 = AgileObjects.AgileMapper.Mapper.Map(Test()).ToANew<Branch>();

            Console.WriteLine();

            var newWarehouseA1 = AutoMapper.Mapper.Map<Branch>(Test());

            var newWarehouseA2 = AutoMapper.Mapper.Map<Branch>(Test());

            var newWarehouseA3 = AutoMapper.Mapper.Map<Branch>(Test());

            var newWarehouseA4 = AutoMapper.Mapper.Map<Branch>(Test());

            Console.WriteLine();

            Console.ReadKey(intercept: true);

            //if no "temp.txt" (
            //    ECHO found
            //) ELSE (
            //    ECHO not found
            //)
        }
    }
}
