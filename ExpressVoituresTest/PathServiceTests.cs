using ExpressVoitures.Models.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ExpressVoituresTest
{
    public class PathServiceTests
    {
        private readonly IPathService _pathService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environement;

        public PathServiceTests()
        {
            _configuration = new ConfigurationBuilder().Build();
            _environement = Mock.Of<IWebHostEnvironment>(x =>
                 x.WebRootPath == "/path/to/webroot" &&
                 x.ContentRootPath == "/path/to/contentroot");
            _pathService = new PathService(_configuration, _environement);
        }

        [Fact]
        [Description("Avec le nom de l'image, il construit le lien photo")]
        public void GetUploadsPath()
        {
            var filename = "test.jpg";

            var cheminAttendu = Path.Combine(_environement.WebRootPath, filename);

            //Act
            var lienPhoto = _pathService.GetUploadsPath(filename);

            //Assert
            Assert.Equal(cheminAttendu, lienPhoto);
        }
    }
}
