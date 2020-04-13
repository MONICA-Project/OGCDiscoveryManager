/*
 * MONICA COP API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 0.1.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PublicFeedback : IEquatable<PublicFeedback>
    { 
        /// <summary>
        /// Phone id
        /// </summary>
        /// <value>Phone id</value>
        [DataMember(Name="phoneid")]
        public string Phoneid { get; set; }

        /// <summary>
        /// Feedback Type
        /// </summary>
        /// <value>Feedback Type</value>
        [DataMember(Name="feedbackType")]
        public string FeedbackType { get; set; }

        /// <summary>
        /// Feedback value
        /// </summary>
        /// <value>Feedback value</value>
        [DataMember(Name="feedback_value")]
        public decimal? FeedbackValue { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PublicFeedback {\n");
            sb.Append("  Phoneid: ").Append(Phoneid).Append("\n");
            sb.Append("  FeedbackType: ").Append(FeedbackType).Append("\n");
            sb.Append("  FeedbackValue: ").Append(FeedbackValue).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((PublicFeedback)obj);
        }

        /// <summary>
        /// Returns true if PublicFeedback instances are equal
        /// </summary>
        /// <param name="other">Instance of PublicFeedback to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PublicFeedback other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Phoneid == other.Phoneid ||
                    Phoneid != null &&
                    Phoneid.Equals(other.Phoneid)
                ) && 
                (
                    FeedbackType == other.FeedbackType ||
                    FeedbackType != null &&
                    FeedbackType.Equals(other.FeedbackType)
                ) && 
                (
                    FeedbackValue == other.FeedbackValue ||
                    FeedbackValue != null &&
                    FeedbackValue.Equals(other.FeedbackValue)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Phoneid != null)
                    hashCode = hashCode * 59 + Phoneid.GetHashCode();
                    if (FeedbackType != null)
                    hashCode = hashCode * 59 + FeedbackType.GetHashCode();
                    if (FeedbackValue != null)
                    hashCode = hashCode * 59 + FeedbackValue.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(PublicFeedback left, PublicFeedback right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PublicFeedback left, PublicFeedback right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
