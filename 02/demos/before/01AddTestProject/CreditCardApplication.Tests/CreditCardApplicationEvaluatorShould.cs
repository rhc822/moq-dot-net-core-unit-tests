using System;
using Xunit;

namespace CreditCardApplication.Tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            var sut = new CreditCardApplicationEvaluator();

            var application = new CreditCardApplication { GrossAnnualINcome = 100_000 };
        }
    }
}
