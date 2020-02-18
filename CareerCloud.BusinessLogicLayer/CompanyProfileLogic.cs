using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyProfileLogic : BaseLogic<CompanyProfilePoco>
    {
        public CompanyProfileLogic(IDataRepository<CompanyProfilePoco> repository) : base(repository)
        {
        }
        protected override void Verify(CompanyProfilePoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach(CompanyProfilePoco poco in pocos)
            {
                if (string.IsNullOrEmpty(poco.ContactPhone))
                {
                    exceptions.Add(new ValidationException(601, "Must not be empty"));
                }
                else if(!Regex.IsMatch(poco.ContactPhone, @"/\d{ 3}-\d{ 3}-\d{ 4}/"))
                {
                    exceptions.Add(new ValidationException(601, "Must not be empty"));
                }

                if (string.IsNullOrEmpty(poco.CompanyWebsite))
                {
                    exceptions.Add(new ValidationException(601, "Must not be empty"));
                }
                else if (!Regex.IsMatch(poco.CompanyWebsite, @"/[a-z][\w\.]+@\w+\.(com|ca|biz)/", RegexOptions.IgnoreCase))
                {
                    exceptions.Add(new ValidationException(600, "must end with the following extensions – ca, com or biz"));
                }  
            }
            if(exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Add(CompanyProfilePoco[] pocos)
        {
            Verify(pocos);
            base.Add(pocos);
        }
        public override void Update(CompanyProfilePoco[] pocos)
        {
            Verify(pocos);
            base.Update(pocos);
        }
    }
}
