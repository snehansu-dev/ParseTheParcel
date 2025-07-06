using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParseTheParcel.Application.DTOs;
using ParseTheParcel.Application.Models;
using ParseTheParcel.Application.Services;

namespace ParseTheParcel.Tests
{
    public class ParcelServiceTests
    {
        private ParcelService CreateServiceWithRules(List<ParcelRule> rules)
        {
            var options = Options.Create(rules);
            var evaluatorLoggerMock = new Mock<ILogger<ParcelRuleEvaluator>>();
            var serviceLoggerMock = new Mock<ILogger<ParcelService>>();

            var evaluator = new ParcelRuleEvaluator(options, evaluatorLoggerMock.Object);
            return new ParcelService(evaluator, serviceLoggerMock.Object);
        }

        [Fact]
        public void GetParcelTypeAndCost_ReturnsExpectedResult_ForSmallParcel()
        {
            // Arrange
            var rules = new List<ParcelRule>
            {
                new ParcelRule
                {
                    Type = "Small",
                    MaxLength = 200,
                    MaxBreadth = 300,
                    MaxHeight = 150,
                    MaxWeight = 25,
                    Cost = 5.0
                }
            };

            var service = CreateServiceWithRules(rules);
            var request = new ParcelRequest
            {
                Length = 150,
                Breadth = 250,
                Height = 100,
                Weight = 10
            };

            // Act
            var (type, cost, message) = service.GetParcelTypeAndCost(request);

            // Assert
            Assert.Equal("Small", type);
            Assert.Equal(5.0, cost);
            Assert.StartsWith("Requested parcel accepted", message);
        }

        [Fact]
        public void GetParcelTypeAndCost_ReturnsError_WhenParcelWeightExceeds()
        {
            // Arrange
            var rules = new List<ParcelRule>
            {
                new ParcelRule
                {
                    Type = "Small",
                    MaxLength = 200,
                    MaxBreadth = 300,
                    MaxHeight = 150,
                    MaxWeight = 25,
                    Cost = 5.0
                }
            };

            var service = CreateServiceWithRules(rules);
            var request = new ParcelRequest
            {
                Length = 150,
                Breadth = 250,
                Height = 100,
                Weight = 30
            };

            // Act
            var (type, cost, message) = service.GetParcelTypeAndCost(request);

            // Assert
            Assert.Null(type);
            Assert.Null(cost);
            Assert.StartsWith("Requested parcel weight 30kg exceeds", message);
        }
    }
}
