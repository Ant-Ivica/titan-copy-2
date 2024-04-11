using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    /// <summary>
    /// Represents user's claims
    /// </summary>
    public class UserClaims
    {
        /// <summary>
        /// User ID
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Claims for the user
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public List<Claim> Claims { get; set; }
    }
}
