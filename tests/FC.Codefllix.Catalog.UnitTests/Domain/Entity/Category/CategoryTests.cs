using FC.Codeflix.Catalog.Domain.Exceptions;
using System.Xml.Linq;
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


        [Theory(DisplayName = "ExceptionWhenInstantiateWithNameIsLessThan3Char")]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("ab")]
        [InlineData("a")]
        public void ExceptionWhenInstantiateWithNameIsLessThan3Char(string invalidName)
        {
            Action action =
                () => new DomainEntity.Category(invalidName, "Category description");
            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact (DisplayName = "ExceptionWhenInstantiateWithNameIsLongerThan255Chars")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenInstantiateWithNameIsLongerThan255Chars()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action =
                () => new DomainEntity.Category(invalidName, "Category description");
            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name size should be less or equal to 255 chars", exception.Message);
        }

        [Fact(DisplayName = "ExceptionWhenInstantiateWithDescriptionLongerThan10000Chars")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenInstantiateWithDescriptionLongerThan10000Chars()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
            Action action =
                () => new DomainEntity.Category("Name", invalidDescription);
            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Description size should be less or equal to 10_000 chars", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            //Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, false);
            category.Activate();

            //Assert
            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            //Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            //Assert
            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("name", "descripion");
            var newValues = new { Name = "new name", Description = "new description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateNameOnly))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateNameOnly()
        {
            var category = new DomainEntity.Category("name", "descripion");

            var currentDescription = category.Description;
            var newName = "new name";

            category.Update(newName);

            Assert.Equal(newName, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = "ExceptionWhenUpdateWithEmptyName")]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("    ")]
        public void ExceptionWhenUpdateWithEmptyName(string name)
        {
            var category = new DomainEntity.Category("name", "descripion");

            Action action = () => category.Update(name!);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name should not be null or empty", exception.Message);
        }

        [Theory(DisplayName = "ExceptionWhenUpdateWithNameIsLessThan3Char")]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("ab")]
        [InlineData("a")]
        public void ExceptionWhenUpdateWithNameIsLessThan3Char(string invalidName)
        {
            var category = new DomainEntity.Category("name", "Category description");

            Action action = () => category.Update(invalidName!);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = "ExceptionWhenUpdateWithNameIsLongerThan255Chars")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenUpdateWithNameIsLongerThan255Chars()
        {

            var category = new DomainEntity.Category("name", "Category description");

            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => category.Update(invalidName);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name size should be less or equal to 255 chars", exception.Message);
        }

        [Fact(DisplayName = "ExceptionWhenUpdateWithDescriptionLongerThan10000Chars")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenUpdateWithDescriptionLongerThan10000Chars()
        {
            var category = new DomainEntity.Category("Name", "description");

            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

            Action action = () => category.Update(invalidDescription);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Description size should be less or equal to 10_000 chars", exception.Message);
        }

    }
}
