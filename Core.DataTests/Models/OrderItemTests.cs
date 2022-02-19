using Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Core.DataTests.Models
{
    [TestClass]
    public class OrderItemTests
    {
        [DataTestMethod]
        [DataRow(100, 1, 100)]
        [DataRow(100, 2, 150)]
        [DataRow(100, 3, 225)]
        [DataRow(100, 4, 300)]
        [DataRow(100, 5, 375)]
        [DataRow(100, 6, 300)]
        [DataRow(100, 7, 350)]
        public void GetAmount_WhenAmountRequested_CalculatesAmountAndAppliesDiscounts(
            double unitPrice,
            int units,
            double expectedAmount)
        {
            // Arrange
            var orderItem = new OrderItem(Guid.NewGuid(), "Data Driven Tested Product", unitPrice, units);

            // Act
            var actualAmount = orderItem.Amount;

            // Assert
            actualAmount.Should().Be(expectedAmount);
        }
    }
}
