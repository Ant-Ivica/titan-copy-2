namespace FA.LVIS.Tower.Services
{
    public interface IDomainUserValidator
    {
        /// <summary>
        /// Determines whether [is domain user name] [the specified user name].
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>True if user is a domain user</returns>
        bool IsDomainUserName(string userName);
    }
}
