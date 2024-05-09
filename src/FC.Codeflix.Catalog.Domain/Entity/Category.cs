﻿using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Domain.Validation;

namespace FC.Codeflix.Catalog.Domain.Entity
{
    public class Category : AggregateRoot
    {
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

        public Category(string name, string description, bool isActive = true) : base()
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
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            DomainValidation.MinLength(Name, 3, nameof(Name));
            DomainValidation.MaxLength(Name, 255, nameof(Name));
            DomainValidation.NotNull(Description, nameof(Description));
            DomainValidation.MaxLength(Description, 10000, nameof(Description));
        }
    }

}
