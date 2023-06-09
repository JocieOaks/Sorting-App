﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Sorting_App.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

#nullable disable

namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="Element"/> class is the model for the members of an <see cref="ElementList"/>.
    /// </summary>
    public class Element
    {
        /// <value>The database ID for the <see cref="Element"/>.</value>
        public int ID { get; set; }

        /// <value>The image associated with the <see cref="Element"/> as an array of bytes.</value>
        public byte[] Image { get; set; }

        /// <value>The <see cref="ElementList"/> the <see cref="Element"/> is in.</value>
        [ValidateNever]
        public ElementList List { get; set; }

        /// <value>The name of the <see cref="Element"/>.</value>
        public string Name { get; set; } = "";
        /// <value>The <see cref="List{T}"/> of <see cref="ElementTag"/>s the <see cref="Element"/> has.</value>
        public List<ElementTag> Tags { get; set; }

        /// <value>Get <see cref="Tags"/> as a string to be put in the input field when editing this <see cref="Element"/>.</value>
        public string TagsString
        {
            get
            {
                if (Tags != null && Tags.Count > 0)
                {
                    StringBuilder tags = new();
                    foreach (ElementTag tag in Tags.SkipLast(1))
                    {
                        tags.Append(tag.Tag);
                        tags.Append(", ");
                    }
                    tags.Append(Tags.Last().Tag);
                    return tags.ToString();
                }
                return "";
            }
        }
    }
}
