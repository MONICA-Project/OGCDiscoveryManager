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
    public partial class Wearable : IEquatable<Wearable>
    { 
        /// <summary>
        /// Person id
        /// </summary>
        /// <value>Person id</value>
        [DataMember(Name="personId")]
        public int? PersonId { get; set; }

        /// <summary>
        /// Thing ID
        /// </summary>
        /// <value>Thing ID</value>
        [DataMember(Name="thingId")]
        public int? ThingId { get; set; }

        /// <summary>
        /// time of wearable connection (e.g. &#39;2016-06-17T15:28:34Z&#39; (RFC 3339, ISO 8601))
        /// </summary>
        /// <value>time of wearable connection (e.g. &#39;2016-06-17T15:28:34Z&#39; (RFC 3339, ISO 8601))</value>
        [DataMember(Name="timestamp")]
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Wearable {\n");
            sb.Append("  PersonId: ").Append(PersonId).Append("\n");
            sb.Append("  ThingId: ").Append(ThingId).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Wearable)obj);
        }

        /// <summary>
        /// Returns true if Wearable instances are equal
        /// </summary>
        /// <param name="other">Instance of Wearable to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Wearable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    PersonId == other.PersonId ||
                    PersonId != null &&
                    PersonId.Equals(other.PersonId)
                ) && 
                (
                    ThingId == other.ThingId ||
                    ThingId != null &&
                    ThingId.Equals(other.ThingId)
                ) && 
                (
                    Timestamp == other.Timestamp ||
                    Timestamp != null &&
                    Timestamp.Equals(other.Timestamp)
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
                    if (PersonId != null)
                    hashCode = hashCode * 59 + PersonId.GetHashCode();
                    if (ThingId != null)
                    hashCode = hashCode * 59 + ThingId.GetHashCode();
                    if (Timestamp != null)
                    hashCode = hashCode * 59 + Timestamp.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Wearable left, Wearable right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Wearable left, Wearable right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
