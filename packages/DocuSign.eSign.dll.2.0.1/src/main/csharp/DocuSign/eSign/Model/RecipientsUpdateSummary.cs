using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DocuSign.eSign.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class RecipientsUpdateSummary :  IEquatable<RecipientsUpdateSummary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipientsUpdateSummary" /> class.
        /// </summary>
        public RecipientsUpdateSummary()
        {
            
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [DataMember(Name="recipientUpdateResults", EmitDefaultValue=false)]
        public List<RecipientUpdateResponse> RecipientUpdateResults { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RecipientsUpdateSummary {\n");
            sb.Append("  RecipientUpdateResults: ").Append(RecipientUpdateResults).Append("\n");
            
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
            return this.Equals(obj as RecipientsUpdateSummary);
        }

        /// <summary>
        /// Returns true if RecipientsUpdateSummary instances are equal
        /// </summary>
        /// <param name="other">Instance of RecipientsUpdateSummary to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RecipientsUpdateSummary other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.RecipientUpdateResults == other.RecipientUpdateResults ||
                    this.RecipientUpdateResults != null &&
                    this.RecipientUpdateResults.SequenceEqual(other.RecipientUpdateResults)
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
                
                if (this.RecipientUpdateResults != null)
                    hash = hash * 59 + this.RecipientUpdateResults.GetHashCode();
                
                return hash;
            }
        }

    }
}
