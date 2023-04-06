﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Sorting_App.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="Element"/> class is the model for the members of an <see cref="ElementList"/>.
    /// </summary>
    public class Element
    {
        /// <value>The database ID for the <see cref="Element"/>.</value>
        public int ID { get; set; }

        /// <value>The name of the <see cref="Element"/>.</value>
        public string Name { get; set; } = "";

        /// <value>The <see cref="ElementList"/> the <see cref="Element"/> is in.</value>
        [ValidateNever]
        public ElementList List { get; set; }

        /// <value>The image associated with the <see cref="Element"/> as an array of bytes.</value>
        public byte[]? Image { get; set; }

        /// <value>The <see cref="List{T}"/> of <see cref="ElementTag"/>s the <see cref="Element"/> has.</value>
        public List<ElementTag>? Tags { get; set; }
    }

    public class ListEntityBinder : IModelBinder
    {
        private readonly SortingContext _context;

        public ListEntityBinder(SortingContext context)
        {
            _context = context;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }
            bindingContext.Result = ModelBindingResult.Success(value);
            return Task.CompletedTask;
        }
    }
}
