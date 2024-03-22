using ExpressVoitures.Data;
using ExpressVoitures.Models.Service;
using ExpressVoitures.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVoituresTest
{
    public class ReparationServiceTests
    {
        private readonly IReparationService _reparationService;

        public ReparationServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions);

            this._reparationService = new ReparationService(dbContext);
        }

        [Fact]
        [Description("on créé deux réparations de 500€ et on vérifie qu'il calcule 1000€ de réparations sur la voiture ayant l'ID 1")]
        public void SommeReparationTest()
        {
            //Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions);

            var reparations = dbContext.Reparations.Where(r => r.IdVoiture == 1).ToList();
            dbContext.Reparations.RemoveRange(reparations);
            dbContext.SaveChanges();

            var reparation1 = new Reparation();
            reparation1.IdVoiture = 1;
            reparation1.CoutsReparation = 500;

            var reparation2 = new Reparation();
            reparation2.IdVoiture = 1;
            reparation2.CoutsReparation = 500;

            dbContext.Reparations.AddRange(reparation1, reparation2);
            dbContext.SaveChanges();


            //Act
            float Somme = _reparationService.SommeReparations(1);

            //Assert
            Assert.Equal(1000, Somme);

            reparations = dbContext.Reparations.Where(r => r.IdVoiture == 1).ToList();

            dbContext.Reparations.RemoveRange(reparations);
            dbContext.SaveChanges();
        }
    }
}
