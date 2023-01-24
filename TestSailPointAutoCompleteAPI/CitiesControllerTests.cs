using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SailpointBackend.Controllers;
using SailpointBackend.Models;
using System.Collections.Generic;

namespace TestSailPointBackend
{
    public class CitiesControllerTests
    {
        private CitiesController GetController()
        {
            var contextOptions = new DbContextOptionsBuilder<CityContext>()
                     .UseSqlServer("Data Source=gshalev6-MOBL1\\SQLEXPRESS;Initial Catalog=Cities;Integrated Security=True;Encrypt=False")
                     .Options;
            var context = new CityContext(contextOptions);
            var controller = new CitiesController(context);
            return controller;
        }

        [Fact]
        public async void FilterBySpecificCity_ExpectSingleResult()
        {
            //Arrange
            var controller = GetController();
            //Act
            var result = await controller.GetCities("eilat",0,10);
            var okResult = result.Result as OkObjectResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(okResult is OkObjectResult);
            Assert.IsAssignableFrom<IList<City>>(okResult.Value);
            
            var cities = okResult.Value as IList<City>;
            Assert.True(condition: cities?[0].Name.ToLower() == "eilat");
        }

        [Fact]
        public async void FilterByEmptySearchTerm_ExpectNotFound()
        {
            //Arrange
            var controller = GetController();
            //Act
            var result = await controller.GetCities("",0,10);
            var notFoundResult = result.Result as NotFoundResult;
            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.True(notFoundResult is NotFoundResult);
        }

        [Fact]
        public async void FilterByWideSearchTerm_ExpectFullPageResults()
        {
            //Arrange
            var controller = GetController();
            int pageSize = 10;
            //Act
            var result = await controller.GetCities("e",0,10);
            var okResult = result.Result as OkObjectResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(okResult is OkObjectResult);
            Assert.IsAssignableFrom<IList<City>>(okResult.Value);
            var cities = okResult.Value as IList<City>;
            Assert.True(condition: cities?.Count == pageSize);
        }

        [Fact]
        public async void FilterByWideSearchTerm_ExpectToGetPage2FullResults()
        {
            //Arrange
            var controller = GetController();
            int pageSize = 10;
            //Act
            var result = await controller.GetCities("e", 2, 10); //page 3 with index=2
            //we exepct to get page 3 full since the search is of wide range: the letter e which exist in many cities
            var okResult = result.Result as OkObjectResult;
            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True(okResult is OkObjectResult);
            Assert.IsAssignableFrom<IList<City>>(okResult.Value);
            var cities = okResult.Value as IList<City>;
            Assert.True(condition: cities?.Count == pageSize);
        }
    }
}