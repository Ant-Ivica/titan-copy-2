using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class IdentityUser : DataContractBase, IUser
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// User ID
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        /// <value>
        /// The username of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// </summary>
        /// <value>
        ///   <c>true</c> if [email confirmed]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// The user password for Active Directory authentication [Note: Passing password in plain text is not secure, HTTPS must be used]
        /// </summary>
        /// <value>
        /// The active directory password.
        /// </value>
        public virtual string ActiveDirectoryPassword { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        /// <value>
        /// The security stamp.
        /// </value>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>
        /// <value>
        /// <c>true</c> if [phone number confirmed]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user
        /// </summary>
        /// <value>
        ///   <c>true</c> if [two factor enabled]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        /// <value>
        /// The lockout end date UTC.
        /// </value>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// </summary>
        /// <value>
        ///   <c>true</c> if [lockout enabled]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// </summary>
        /// <value>
        /// The access failed count.
        /// </value>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Is the password expired for this user
        /// </summary>
        /// <value>
        ///   <c>true</c> if [password expired]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool PasswordExpired { get; set; }
    }
}
