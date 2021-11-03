using System;
using Xunit;
using Moq;

namespace CreditCardApplications.tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            // Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

            // Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            // Assert
            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            // Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            mockValidator
                .Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 19 };

            //Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            // ANY STRING VALUE WILL RETURN TRUE
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // SPECIFY WHICH STRING WILL MAKE THE RETURN TRUE
            //mockValidator
            //    .Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith('x'))))
            //    .Returns(true);

            // SPECIFY A LIST OF VALUES THAT WILL RETURN TRUE
            //mockValidator
            //    .Setup(x => x.IsValid(It.IsIn("x", "y", "z")))
            //    .Returns(true);

            // SPECIFY A LIST OF VALUES IN A GIVEN RANGE THAT WILL RETURN TRUE
            //mockValidator
            //    .Setup(x => x.IsValid(It.IsInRange("b", "z", Moq.Range.Inclusive)))
            //    .Returns(true);

            // SPECIFY A REGEX THAT WILL RETURN TRUE
            mockValidator
                .Setup(x => x.IsValid(It.IsRegex("[a-z]", System.Text.RegularExpressions.RegexOptions.None)))
                .Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };

            //Act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }


        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            mockValidator
                .Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(false);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication();

            //Act

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplicationOutDemo()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator =
                new Mock<IFrequentFlyerNumberValidator>();

            bool isValid = true;
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication
            { 
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };

            //Act
            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            //Arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(x => x.LicenseKey).Returns("EXPIRED"); // HARD-CODED VALUE
            mockValidator.Setup(x => x.LicenseKey).Returns(GetLicenseKeyExpiryString); // VALUE RETURNED BY FUNCTION BELOW

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 42 };

            //Act

            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //Assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLicenseKeyExpiryString()
        {
            // E.g. read from vendor-supplied constants file
            return "EXPIRED";
        }
    }
}
