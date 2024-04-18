using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using System.Xml.Linq;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codefllix.Catalog.UnitTests.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTests
    {

        private readonly CategoryTestFixture _categoryTestFixture;

        public CategoryTests(CategoryTestFixture categoryTestFixture) 
            => _categoryTestFixture = categoryTestFixture;

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var dateTimeAfter = DateTime.Now.AddSeconds(1);

            //Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

            (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
            (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
            category.IsActive.Should().BeTrue();
        }


        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            //Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
            var dateTimeAfter = DateTime.Now.AddSeconds(1);

            //Assert
            Assert.NotNull(category);
            Assert.Equal(validCategory.Name, category.Name);
            Assert.Equal(validCategory.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt >= dateTimeBefore);
            Assert.True(category.CreatedAt <= dateTimeAfter);
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

            action.Should()
                .Throw<EntityValidationExceprion>()
                .WithMessage("Description should not be null");
        }


        [Theory(DisplayName = "ExceptionWhenInstantiateWithNameIsLessThan3Char")]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesWithLessThan3Chars), parameters: 10)]
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
            var validCategory = _categoryTestFixture.GetValidCategory();

            var dateTimeBefore = DateTime.Now;
            //Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
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
            var category = _categoryTestFixture.GetValidCategory();
            var newValues = new { Name = "new name", Description = "new description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateNameOnly))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateNameOnly()
        {
            var category= _categoryTestFixture.GetValidCategory();

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
            var category = _categoryTestFixture.GetValidCategory();

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
            var category = _categoryTestFixture.GetValidCategory();

            Action action = () => category.Update(invalidName!);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = "ExceptionWhenUpdateWithNameIsLongerThan255Chars")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenUpdateWithNameIsLongerThan255Chars()
        {

            var category = _categoryTestFixture.GetValidCategory();

            var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

            Action action = () => category.Update(invalidName);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Name size should be less or equal to 255 chars", exception.Message);
        }

        [Fact(DisplayName = "ExceptionWhenUpdateWithDescriptionLongerThan10000Chars")]
        [Trait("Domain", "Category - Aggregates")]
        public void ExceptionWhenUpdateWithDescriptionLongerThan10000Chars()
        {
            var category = new DomainEntity.Category("Name", "description");

            var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

            while(invalidDescription.Length <= 10000)
                invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

            Action action = () => category.Update("Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationExceprion>(action);
            Assert.Equal("Description size should be less or equal to 10_000 chars", exception.Message);
        }

        public static IEnumerable<object[]> GetNamesWithLessThan3Chars(int testQuantity) 
        {
            var fixture = new CategoryTestFixture();

            for (int i = 0; i < testQuantity; i++)
            {
                var isOdd = i % 2 == 0;
                yield return new object[] { fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
            }

        }

    }
}
