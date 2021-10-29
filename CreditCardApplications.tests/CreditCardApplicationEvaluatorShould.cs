using System;
using Xunit;

namespace CreditCardApplications.tests
{
    public class CreditCardApplicationEvaluatorShould
    {
        [Fact]
        public void AcceptHighIncomeApplications()
        {
            var sut = new CreditCardApplicationEvaluator();
        }
    }
}
