namespace FA.LVIS.Tower.Services
{
    public class DomainUserValidator : IDomainUserValidator
    {
        public virtual bool IsDomainUserName(string userName)
        {
            return userName.Contains("\\");
        }
    }
}
