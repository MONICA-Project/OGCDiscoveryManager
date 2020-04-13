/*
 * MONICA COP API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 0.7.0
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
    public partial class PhoneConnectedToWearable : IEquatable<PhoneConnectedToWearable>
    { 
        /// <summary>
        /// Wearable physical Id.
        /// </summary>
        /// <value>Wearable physical Id.</value>
        [DataMember(Name="wearablePhysicalId")]
        public int? WearablePhysicalId { get; set; }

        /// <summary>
        /// Phone id for notification
        /// </summary>
        /// <value>Phone id for notification</value>
        [DataMember(Name="phoneId")]
        public string PhoneId { get; set; }

        /// <summary>
        /// Optional phone number
        /// </summary>
        /// <value>Optional phone number</value>
        [DataMember(Name="phoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PhoneConnectedToWearable {\n");
            sb.Append("  WearablePhysicalId: ").Append(WearablePhysicalId).Append("\n");
            sb.Append("  PhoneId: ").Append(PhoneId).Append("\n");
            sb.Append("  PhoneNumber: ").Append(PhoneNumber).Append("\n");
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
            return obj.GetType() == GetType() && Equals((PhoneConnectedToWearable)obj);
        }

        /// <summary>
        /// Returns true if PhoneConnectedToWearable instances are equal
        /// </summary>
        /// <param name="other">Instance of PhoneConnectedToWearable to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PhoneConnectedToWearable other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    WearablePhysicalId == other.WearablePhysicalId ||
                    WearablePhysicalId != null &&
                    WearablePhysicalId.Equals(other.WearablePhysicalId)
                ) && 
                (
                    PhoneId == other.PhoneId ||
                    PhoneId != null &&
                    PhoneId.Equals(other.PhoneId)
                ) && 
                (
                    PhoneNumber == other.PhoneNumber ||
                    PhoneNumber != null &&
                    PhoneNumber.Equals(other.PhoneNumber)
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
                    if (WearablePhysicalId != null)
                    hashCode = hashCode * 59 + WearablePhysicalId.GetHashCode();
                    if (PhoneId != null)
                    hashCode = hashCode * 59 + PhoneId.GetHashCode();
                    if (PhoneNumber != null)
                    hashCode = hashCode * 59 + PhoneNumber.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(PhoneConnectedToWearable left, PhoneConnectedToWearable right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhoneConnectedToWearable left, PhoneConnectedToWearable right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}