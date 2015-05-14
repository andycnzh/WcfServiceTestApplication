

namespace WcfServiceTestApplication
{
    using System;
    using System.IdentityModel.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RestServiceImpl" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RestServiceImpl.svc or RestServiceImpl.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestServiceImpl : IRestServiceImpl
    {
        public void DoWork()
        {
        }


        public string XMLData(string id)
        {
            string message = string.Empty;

            message += "You requested id " + id + ".";

            string certificateThumbprint = string.Empty;

            if (OperationContext.Current != null &&
                OperationContext.Current.ServiceSecurityContext != null &&
                OperationContext.Current.ServiceSecurityContext.AuthorizationContext != null)
            {
                System.IdentityModel.Policy.AuthorizationContext authContext = OperationContext.Current.ServiceSecurityContext.AuthorizationContext;
                if (authContext.ClaimSets != null)
                {
                    if (authContext.ClaimSets.Count > 0)
                    {
                        X509Certificate2 cert = ((X509CertificateClaimSet)authContext.ClaimSets[0]).X509Certificate;

                        // verify the certrificate date
                        DateTime now = DateTime.Now.ToLocalTime();
                        if (now > cert.NotBefore && now < cert.NotAfter)
                        {
                            certificateThumbprint = cert.Thumbprint;
                        }
                        else
                        {
                            message += " 4. Cert expired time check return false.";
                        }
                    }
                    else
                    {
                        message += " 3. AuthorizationContext ClaimSets count is 0.";
                    }
                }
                else
                {
                    message += " 2. AuthorizationContext ClaimSets is null.";
                }
            }
            else
            {
                message += " 1. OperationContext check return false.";
            }

            message += " Certificate thumbprint is " + certificateThumbprint;

            return message;
        }

        public string JSONData(string id)
        {
            return "You requested id " + id;
        }
    }
}
