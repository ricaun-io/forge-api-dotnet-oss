/* 
 * Forge SDK
 *
 * The Forge Platform contains an expanding collection of web service components that can be used with Autodesk cloud-based products or your own technologies. Take advantage of Autodesk’s expertise in design and engineering.
 *

 * Contact: forge.help@autodesk.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Autodesk.Forge.Model
{
    /// <summary>
    /// Bucket
    /// </summary>
    [DataContract]
    public partial class Bucket :  IEquatable<Bucket>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bucket" /> class.
        /// </summary>
        /// <param name="BucketKey">The key of the Bucket.</param>
        /// <param name="BucketOwner">The owner of the Bucket.</param>
        /// <param name="CreatedDate">Counts the number of views for the asset.</param>
        /// <param name="Permissions">Permissions.</param>
        /// <param name="PolicyKey">The policy key.</param>
        public Bucket(string BucketKey = null, string BucketOwner = null, string CreatedDate = null, List<Permission> Permissions = null, string PolicyKey = null)
        {
            this.BucketKey = BucketKey;
            this.BucketOwner = BucketOwner;
            this.CreatedDate = CreatedDate;
            this.Permissions = Permissions;
            this.PolicyKey = PolicyKey;
        }
        
        /// <summary>
        /// The key of the Bucket
        /// </summary>
        /// <value>The key of the Bucket</value>
        [DataMember(Name="bucketKey", EmitDefaultValue=false)]
        public string BucketKey { get; set; }
        /// <summary>
        /// The owner of the Bucket
        /// </summary>
        /// <value>The owner of the Bucket</value>
        [DataMember(Name="bucketOwner", EmitDefaultValue=false)]
        public string BucketOwner { get; set; }
        /// <summary>
        /// Counts the number of views for the asset
        /// </summary>
        /// <value>Counts the number of views for the asset</value>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public string CreatedDate { get; set; }
        /// <summary>
        /// Gets or Sets Permissions
        /// </summary>
        [DataMember(Name="permissions", EmitDefaultValue=false)]
        public List<Permission> Permissions { get; set; }
        /// <summary>
        /// The policy key
        /// </summary>
        /// <value>The policy key</value>
        [DataMember(Name="policyKey", EmitDefaultValue=false)]
        public string PolicyKey { get; set; }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Bucket {\n");
            sb.Append("  BucketKey: ").Append(BucketKey).Append("\n");
            sb.Append("  BucketOwner: ").Append(BucketOwner).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  Permissions: ").Append(Permissions).Append("\n");
            sb.Append("  PolicyKey: ").Append(PolicyKey).Append("\n");
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
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as Bucket);
        }

        /// <summary>
        /// Returns true if Bucket instances are equal
        /// </summary>
        /// <param name="other">Instance of Bucket to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Bucket other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.BucketKey == other.BucketKey ||
                    this.BucketKey != null &&
                    this.BucketKey.Equals(other.BucketKey)
                ) && 
                (
                    this.BucketOwner == other.BucketOwner ||
                    this.BucketOwner != null &&
                    this.BucketOwner.Equals(other.BucketOwner)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.Permissions == other.Permissions ||
                    this.Permissions != null &&
                    this.Permissions.SequenceEqual(other.Permissions)
                ) && 
                (
                    this.PolicyKey == other.PolicyKey ||
                    this.PolicyKey != null &&
                    this.PolicyKey.Equals(other.PolicyKey)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.BucketKey != null)
                    hash = hash * 59 + this.BucketKey.GetHashCode();
                if (this.BucketOwner != null)
                    hash = hash * 59 + this.BucketOwner.GetHashCode();
                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();
                if (this.Permissions != null)
                    hash = hash * 59 + this.Permissions.GetHashCode();
                if (this.PolicyKey != null)
                    hash = hash * 59 + this.PolicyKey.GetHashCode();
                return hash;
            }
        }
    }

}
