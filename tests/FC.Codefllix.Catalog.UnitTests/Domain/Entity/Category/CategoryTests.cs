﻿using FC.Codeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codefllix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTests
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var dateTimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);    
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > dateTimeBefore);
            Assert.True(category.CreatedAt < dateTimeAfter);
            Assert.True(category.IsActive);
        }


        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            //Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var dateTimeAfter = DateTime.Now;

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > dateTimeBefore);
            Assert.True(category.CreatedAt < dateTimeAfter);
            Assert.Equal(category.IsActive, isActive);
        }

        [Theory(DisplayName = "ExceptionWhenInstantiateWithEmptyName")]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void ExceptionWhenInstantiateWithEmptyName(string? name)
        {
            Action action = 
                () => new DomainEntity.Category(name!, "Category description");
            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name should not be null or empty", exception.Message);
        }

        [Fact(DisplayName = "ExceptionWhenInstantiateWithNullDescription")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenInstantiateWithNullDescription()
        {
            Action action =
                () => new DomainEntity.Category("Category name", null);
            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Description should not be null", exception.Message);
        }
    }
}
