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
    /// Buckets
    /// </summary>
    [DataContract]
    public partial class Buckets :  IEquatable<Buckets>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Buckets" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected Buckets() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Buckets" /> class.
        /// </summary>
        /// <param name="Items">Array of items representing the buckets (required).</param>
        /// <param name="Next">Next possible request (required).</param>
        public Buckets(List<BucketsItems> Items = null, string Next = null)
        {
            // to ensure "Items" is required (not null)
            if (Items == null)
            {
                throw new InvalidDataException("Items is a required property for Buckets and cannot be null");
            }
            else
            {
                this.Items = Items;
            }
            // to ensure "Next" is required (not null)
            if (Next == null)
            {
                throw new InvalidDataException("Next is a required property for Buckets and cannot be null");
            }
            else
            {
                this.Next = Next;
            }
        }
        
        /// <summary>
        /// Array of items representing the buckets
        /// </summary>
        /// <value>Array of items representing the buckets</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<BucketsItems> Items { get; set; }
        /// <summary>
        /// Next possible request
        /// </summary>
        /// <value>Next possible request</value>
        [DataMember(Name="next", EmitDefaultValue=false)]
        public string Next { get; set; }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Buckets {\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Next: ").Append(Next).Append("\n");
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
            return this.Equals(obj as Buckets);
        }

        /// <summary>
        /// Returns true if Buckets instances are equal
        /// </summary>
        /// <param name="other">Instance of Buckets to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Buckets other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.Next == other.Next ||
                    this.Next != null &&
                    this.Next.Equals(other.Next)
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
                if (this.Items != null)
                    hash = hash * 59 + this.Items.GetHashCode();
                if (this.Next != null)
                    hash = hash * 59 + this.Next.GetHashCode();
                return hash;
            }
        }
    }

}
