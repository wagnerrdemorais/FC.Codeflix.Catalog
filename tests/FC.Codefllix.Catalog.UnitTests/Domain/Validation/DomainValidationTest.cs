
using Bogus;
using Xunit;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using System;

namespace FC.Codefllix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOK))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOK()
        {
            var value = Faker.Commerce.ProductName();
            Action action =
                () => DomainValidation.NotNull(value, "Value");
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            Action action =
                () => DomainValidation.NotNull(value, "FieldName");
            action.Should().Throw<EntityValidationException>().WithMessage("FieldName Should Not Be Null");
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrow))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void NotNullOrEmptyThrow(string? target)
        {
            Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");
            action.Should().Throw<EntityValidationException>()
                .WithMessage("fieldName should not be null or empty");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOK))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOK()
        {
            var target = Faker.Commerce.ProductName();

            Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(ThrowWhenLessThanMinLength))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
        public void ThrowWhenLessThanMinLength(string target, int minLength)
        {
            Action action = () => DomainValidation.MinLength(target, minLength, "fieldName");

            action.Should().Throw<EntityValidationException>()
            .WithMessage($"fieldName should not be less than {minLength}");
        }

        [Theory(DisplayName = nameof(MinLengthOK))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
        public void MinLengthOK(string target, int minLength)
        {
            Action action = () => DomainValidation.MinLength(target, minLength, "fieldName");

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int tetsNumber)
        {
            var Faker = new Faker();
            for (int i = 0; i <= tetsNumber; i++)
            {
                var example = Faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(1, 20);
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int tetsNumber)
        {
            var Faker = new Faker();
            for (int i = 0; i <= tetsNumber; i++)
            {
                var example = Faker.Commerce.ProductName();
                var minLength = example.Length - (new Random()).Next(1, 5);
                yield return new object[] { example, minLength };
            }
        }

        [Theory(DisplayName = nameof(maxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters:10)]
        public void maxLengthThrowWhenGreater(string target, int maxLength)
        {
            Action action = () => DomainValidation.MaxLength(target, maxLength, "fieldName");

            action.Should().Throw<EntityValidationException>()
            .WithMessage($"fieldName should not be greater than {maxLength}");
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests)
        {
            yield return new object[] { "123456", 5 };

            var faker = new Faker();
            for(int i =0; i < (numberOfTests -1); i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - (new Random()).Next(1, 5);
                yield return new object[] {example, maxLength };
            }
        }

        [Theory(DisplayName = nameof(maxLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
        public void maxLengthOk(string target, int maxLength)
        {
            Action action = () => DomainValidation.MaxLength(target, maxLength, "fieldName");

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests)
        {
            yield return new object[] { "12345", 5 };

            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length + (new Random()).Next(0, 5);
                yield return new object[] { example, maxLength };
            }
        }

    }
}
