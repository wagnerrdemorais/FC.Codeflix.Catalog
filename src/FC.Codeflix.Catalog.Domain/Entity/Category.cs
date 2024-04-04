using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Entity
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Category(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = true;
            CreatedAt = DateTime.Now;
            Validate();
        }

        public Category(string name, string description, bool isActive = true)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
            Validate();
        }

        public void Activate()
        {
            this.IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            this.IsActive = false;
            Validate();
        }

        public void Update(string name, string? description = null) { 
            this.Name = name;
            this.Description = description ?? Description;
            Validate();
        }

        private void Validate()
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                throw new EntityValidationExceprion($"{nameof(Name)} should not be null or empty");
            }
            if (Name.Length < 3)
            {
                throw new EntityValidationExceprion($"{nameof(Name)} should be at least 3 characters long");
            }
            if (Name.Length > 255)
            {
                throw new EntityValidationExceprion($"{nameof(Name)} size should be less or equal to 255 chars");
            }
            if (String.IsNullOrWhiteSpace(Description))
            {
                throw new EntityValidationExceprion($"{nameof(Description)} should not be null");
            }
            if (Description.Length > 10000)
            {
                throw new EntityValidationExceprion($"{nameof(Description)} size should be less or equal to 10_000 chars");
            }

        }
    }

}
