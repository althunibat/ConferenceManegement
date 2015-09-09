using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConferenceManagementSystem.Common.Helpers;
using ConferenceManagementSystem.Conference.Model.Properties;
using MassTransit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConferenceManagementSystem.Conference.Model
{
    public class EditableConferenceInfo {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Location { get; set; }

        public string Tagline { get; set; }
        public string TwitterSearch { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(ResourceType = typeof (Resources), Name = "StartDate")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(ResourceType = typeof (Resources), Name = "EndDate")]
        public DateTime EndDate { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "IsPublished")]
        public bool IsPublished { get; set; }
    }

    public class ConferenceInfo : EditableConferenceInfo {

        [BsonConstructor]
        public ConferenceInfo()
        {
            Id = NewId.NextGuid();
            AccessCode = HandleGenerator.Generate(6);
        }

        [BsonId]
        public Guid Id { get; set; }

        [StringLength(6,MinimumLength = 6)]
        public string AccessCode { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Owner")]
        [Required(AllowEmptyStrings = false)]
        public string OwnerName { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "OwnerEmail")]
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InvalidEmail")]
        public string OwnerEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^\w+$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InvalidSlug")]
        public string Slug { get; set; }

        public bool WasEverPublished { get; set; }
    }
}
