using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace logic_app_test.Infrastructure
{
    public class CertificateProvider
    {
        public bool ValidateCertificate(X509Certificate2 clientCertificate)
        {
            string[] allowedThumbprints =
            {
                Settings.JobThumbprint
            };
            if (allowedThumbprints.Contains(clientCertificate.Thumbprint))
            {
                return true;
            }

            return false;
        }
    }
}
