/*
 * MONICA COP API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 0.3.0
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
    public partial class Zone : IEquatable<Zone>
    { 
        /// <summary>
        /// Zone id
        /// </summary>
        /// <value>Zone id</value>
        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Zone name
        /// </summary>
        /// <value>Zone name</value>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Zone description
        /// </summary>
        /// <value>Zone description</value>
        [DataMember(Name="description")]
        public string Description { get; set; }

        /// <summary>
        /// Zone metadata
        /// </summary>
        /// <value>Zone metadata</value>
        [DataMember(Name="metadata")]
        public string Metadata { get; set; }

        /// <summary>
        /// Zone type (e.g. BAR, STAGE, COMMANDCENTER, et c)
        /// </summary>
        /// <value>Zone type (e.g. BAR, STAGE, COMMANDCENTER, et c)</value>
        [DataMember(Name="type")]
        public string Type { get; set; }

        /// <summary>
        /// Optional capacity for Zones that have it defined
        /// </summary>
        /// <value>Optional capacity for Zones that have it defined</value>
        [DataMember(Name="capacity")]
        public int? Capacity { get; set; }

        /// <summary>
        /// Optional people count for Zones that have it defined
        /// </summary>
        /// <value>Optional people count for Zones that have it defined</value>
        [DataMember(Name="peoplecount")]
        public int? Peoplecount { get; set; }

        /// <summary>
        /// The bounding polygon
        /// </summary>
        /// <value>The bounding polygon</value>
        [DataMember(Name="BoundingPolygon")]
        public List<List<decimal?>> BoundingPolygon { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Zone {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Capacity: ").Append(Capacity).Append("\n");
            sb.Append("  Peoplecount: ").Append(Peoplecount).Append("\n");
            sb.Append("  BoundingPolygon: ").Append(BoundingPolygon).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Zone)obj);
        }

        /// <summary>
        /// Returns true if Zone instances are equal
        /// </summary>
        /// <param name="other">Instance of Zone to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Zone other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    Metadata == other.Metadata ||
                    Metadata != null &&
                    Metadata.Equals(other.Metadata)
                ) && 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                ) && 
                (
                    Capacity == other.Capacity ||
                    Capacity != null &&
                    Capacity.Equals(other.Capacity)
                ) && 
                (
                    Peoplecount == other.Peoplecount ||
                    Peoplecount != null &&
                    Peoplecount.Equals(other.Peoplecount)
                ) && 
                (
                    BoundingPolygon == other.BoundingPolygon ||
                    BoundingPolygon != null &&
                    BoundingPolygon.SequenceEqual(other.BoundingPolygon)
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
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (Metadata != null)
                    hashCode = hashCode * 59 + Metadata.GetHashCode();
                    if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                    if (Capacity != null)
                    hashCode = hashCode * 59 + Capacity.GetHashCode();
                    if (Peoplecount != null)
                    hashCode = hashCode * 59 + Peoplecount.GetHashCode();
                    if (BoundingPolygon != null)
                    hashCode = hashCode * 59 + BoundingPolygon.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Zone left, Zone right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Zone left, Zone right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
