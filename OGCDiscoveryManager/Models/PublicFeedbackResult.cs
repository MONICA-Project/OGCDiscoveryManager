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
    public partial class PublicFeedbackResult : IEquatable<PublicFeedbackResult>
    { 
        /// <summary>
        /// number of replies
        /// </summary>
        /// <value>number of replies</value>
        [DataMember(Name="count")]
        public decimal? Count { get; set; }

        /// <summary>
        /// Feedback Type
        /// </summary>
        /// <value>Feedback Type</value>
        [DataMember(Name="feedbackType")]
        public string FeedbackType { get; set; }

        /// <summary>
        /// Feedback mean value
        /// </summary>
        /// <value>Feedback mean value</value>
        [DataMember(Name="feedback_meanvalue")]
        public decimal? FeedbackMeanvalue { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PublicFeedbackResult {\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  FeedbackType: ").Append(FeedbackType).Append("\n");
            sb.Append("  FeedbackMeanvalue: ").Append(FeedbackMeanvalue).Append("\n");
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
            return obj.GetType() == GetType() && Equals((PublicFeedbackResult)obj);
        }

        /// <summary>
        /// Returns true if PublicFeedbackResult instances are equal
        /// </summary>
        /// <param name="other">Instance of PublicFeedbackResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PublicFeedbackResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Count == other.Count ||
                    Count != null &&
                    Count.Equals(other.Count)
                ) && 
                (
                    FeedbackType == other.FeedbackType ||
                    FeedbackType != null &&
                    FeedbackType.Equals(other.FeedbackType)
                ) && 
                (
                    FeedbackMeanvalue == other.FeedbackMeanvalue ||
                    FeedbackMeanvalue != null &&
                    FeedbackMeanvalue.Equals(other.FeedbackMeanvalue)
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
                    if (Count != null)
                    hashCode = hashCode * 59 + Count.GetHashCode();
                    if (FeedbackType != null)
                    hashCode = hashCode * 59 + FeedbackType.GetHashCode();
                    if (FeedbackMeanvalue != null)
                    hashCode = hashCode * 59 + FeedbackMeanvalue.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(PublicFeedbackResult left, PublicFeedbackResult right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PublicFeedbackResult left, PublicFeedbackResult right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
