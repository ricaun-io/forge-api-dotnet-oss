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
    /// ObjectFullDetailsDeltas
    /// </summary>
    [DataContract]
    public partial class ObjectFullDetailsDeltas :  IEquatable<ObjectFullDetailsDeltas>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFullDetailsDeltas" /> class.
        /// </summary>
        /// <param name="Position">Position.</param>
        /// <param name="Sha1">Sha1.</param>
        public ObjectFullDetailsDeltas(int? Position = null, string Sha1 = null)
        {
            this.Position = Position;
            this.Sha1 = Sha1;
        }
        
        /// <summary>
        /// Gets or Sets Position
        /// </summary>
        [DataMember(Name="position", EmitDefaultValue=false)]
        public int? Position { get; set; }
        /// <summary>
        /// Gets or Sets Sha1
        /// </summary>
        [DataMember(Name="sha1", EmitDefaultValue=false)]
        public string Sha1 { get; set; }
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ObjectFullDetailsDeltas {\n");
            sb.Append("  Position: ").Append(Position).Append("\n");
            sb.Append("  Sha1: ").Append(Sha1).Append("\n");
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
            return this.Equals(obj as ObjectFullDetailsDeltas);
        }

        /// <summary>
        /// Returns true if ObjectFullDetailsDeltas instances are equal
        /// </summary>
        /// <param name="other">Instance of ObjectFullDetailsDeltas to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ObjectFullDetailsDeltas other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Position == other.Position ||
                    this.Position != null &&
                    this.Position.Equals(other.Position)
                ) && 
                (
                    this.Sha1 == other.Sha1 ||
                    this.Sha1 != null &&
                    this.Sha1.Equals(other.Sha1)
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
                if (this.Position != null)
                    hash = hash * 59 + this.Position.GetHashCode();
                if (this.Sha1 != null)
                    hash = hash * 59 + this.Sha1.GetHashCode();
                return hash;
            }
        }
    }

}

