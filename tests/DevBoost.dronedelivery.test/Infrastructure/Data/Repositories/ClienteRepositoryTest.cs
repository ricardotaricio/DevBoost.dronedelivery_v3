﻿using AutoBogus;
using DevBoost.DroneDelivery.Domain.Entities;
using DevBoost.DroneDelivery.Domain.Interfaces.Repositories;
using DevBoost.DroneDelivery.Infrastructure.Data.Contexts;
using DevBoost.DroneDelivery.Infrastructure.Data.Repositories;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevBoost.DroneDelivery.Test.Infrastructure.Data.Repositories
{
    public class ClienteRepositoryTest
    {
        [Fact(DisplayName = "GetById")]
        [Trait("ClienteRepositoryTest", "Repository Tests")]
        public async void GetById_test()
        {

            // Given
            var faker = AutoFaker.Create();

            var options = new DbContextOptionsBuilder<DCDroneDelivery>()
           .UseInMemoryDatabase(databaseName: "DroneDelivery")
           .Options;

            var clientes = faker.Generate<Cliente>(3);

            using (var contexto = new DCDroneDelivery(options))
            {
                contexto.Cliente.AddRange(clientes);
                contexto.SaveChanges();
            }

            var expectResponse = clientes.FirstOrDefault();

            using (var contexto = new DCDroneDelivery(options))
            {
                ClienteRepository clienteRepository = new ClienteRepository(contexto);
                //When
                var cliente = await clienteRepository.ObterPorId(expectResponse.Id);

                //Then
                CompareLogic comparer = new CompareLogic();
                Assert.True(comparer.Compare(expectResponse, cliente).AreEqual);

            }

        }



        [Fact(DisplayName = "GetAll")]
        [Trait("ClienteRepositoryTest", "Repository Tests")]
        public async void GetAll_test()
        {

            // Given
            var faker = AutoFaker.Create();

            var options = new DbContextOptionsBuilder<DCDroneDelivery>()
           .UseInMemoryDatabase(databaseName: "DroneDelivery")
           .Options;

            var clientes = faker.Generate<Cliente>(10);

            using (var contexto = new DCDroneDelivery(options))
            {
                contexto.Cliente.AddRange(clientes);
                contexto.SaveChanges();
            }

            var expectResponse = clientes;

            using (var contexto = new DCDroneDelivery(options))
            {
                ClienteRepository clienteRepository = new ClienteRepository(contexto);

                //When
                var allClientes = await clienteRepository.ObterTodos();

                //Then
                CompareLogic comparer = new CompareLogic();
                Assert.True(comparer.Compare(expectResponse, allClientes.ToList()).AreEqual);
            }
        }


        [Fact(DisplayName = "Update")]
        [Trait("ClienteRepositoryTest", "Repository Tests")]
        public async void Update_test()
        {
            // Given
            var faker = AutoFaker.Create();

            var options = new DbContextOptionsBuilder<DCDroneDelivery>().UseInMemoryDatabase(databaseName: "DroneDelivery").Options;
            var cliente = faker.Generate<Cliente>();
            //Seed
            using (var contexto = new DCDroneDelivery(options))
            {
                contexto.Cliente.AddRange(cliente);
                contexto.SaveChanges();
            }

            bool expectResponse;
            bool result;

            using (var contexto = new DCDroneDelivery(options))
            {
                contexto.Cliente.Update(cliente);
                expectResponse = contexto.SaveChanges() > 0;
            }

            using (var contexto = new DCDroneDelivery(options))
            {
                var clienteRepository = new ClienteRepository(contexto);
                await clienteRepository.Atualizar(cliente);
                result = contexto.SaveChanges() > 0;
            }

            var comparer = new CompareLogic();
            Assert.True(comparer.Compare(expectResponse, result).AreEqual);

        }



        [Fact(DisplayName = "Insert")]
        [Trait("ClienteRepositoryTest", "Repository Tests")]
        public async void Insert_test()
        {
            // Given
            var faker = AutoFaker.Create();

            var options = new DbContextOptionsBuilder<DCDroneDelivery>()
           .UseInMemoryDatabase(databaseName: "DroneDelivery")
           .Options;

            var clienteNovo = faker.Generate<Cliente>();

            var expectResponse = clienteNovo; //verificar!!

            using (var contexto = new DCDroneDelivery(options))
            {
                //when
                var clienteRepository = new ClienteRepository(contexto);
                await clienteRepository.Adicionar(clienteNovo);
                var insertCliente = await clienteRepository.UnitOfWork.Commit();

                //then
                Assert.True(insertCliente);

            }
        }


    }
}
