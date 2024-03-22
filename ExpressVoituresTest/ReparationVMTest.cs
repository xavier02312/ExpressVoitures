using ExpressVoitures.Data;
using ExpressVoitures.Models;
using ExpressVoitures.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVoituresTest
{
    public class ReparationVMTests
    {
        [Fact]
        [Description("si la somme des réparations est de 1000€ il doit retourner 1000€")]
        public void CalculTotalReparationTest()
        {
            //Arrange
            var dbContextOptions2 = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb2")
                .Options;
            var dbContext = new ApplicationDbContext(dbContextOptions2);

            dbContext.Reparations.AddRange(
                new Reparation() { IdVoiture = 1, TypeDeReparation = null, CoutsReparation = 500 },
                new Reparation() { IdVoiture = 1, TypeDeReparation = null, CoutsReparation = 500 }
            );

            dbContext.SaveChanges();
            var reparationVM = new ReparationsVM(1, null, dbContext);

            //Act
            float TotalReparation = reparationVM.CalculTotalReparation();

            //Assert
            Assert.Equal(1000, TotalReparation);

            var reparations = dbContext.Reparations.Where(r => r.IdVoiture == 1).ToList();

            dbContext.Reparations.RemoveRange(reparations);
            dbContext.SaveChanges();
        }
    }
}
