using System;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Gather model
    /// </summary>
    public class Gather: BaseModel
    {
        /// <summary>
        /// Gather state
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Reason of gathe
        /// </summary>
        public string Reason { get; set; }
        
        /// <summary>
        /// Created time
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        
        /// <summary>
        /// Completed time
        /// </summary>
        public DateTime? CompletedTime { get; set; }
        
        /// <summary>
        /// Url to related call
        /// </summary>
        public string Call { get; set; }
        
        /// <summary>
        /// Digits
        /// </summary>
        public string Digits { get; set; }
    }
  
}