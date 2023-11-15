using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LostAndFound.API.Extensions.FirestoreService
{
    public class FirebaseSetting
    {
        [JsonPropertyName("type")]
        public string Type => "service_account";
        [JsonPropertyName("project_id")]
        public string ProjectId => "lostandfound-test-b0e12";
        [JsonPropertyName("private_key_id")]
        public string PrivateKeyId => "641398cc438ab2ac92794acc214cd6124032c696";
        [JsonPropertyName("private_key")]
        public string PrivateKey => "-----BEGIN PRIVATE KEY-----\nMIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQCe0M19HtJLIwn0\nfR0i6isKPmRyEYdQ+ol3ClSzaMGn/RCdaeq87I7PXeRrOwZ/WDsGTST4pN2t/s9Z\nOl67THZt0V6+FHZ5P8AXhX952swBU6CFodnRULycI77z0w0eHiqjDvtDDnq8+mTK\nYOw4us43UnD27pU0yEdFu+t+w5IhgIQ/zCh+DnnGoWC5+szfwmqfGkXzMo3W3IDL\nxrJx12gT2jMkzhq5kWCd6rLMzQHAXBWhShwDg/0ktB8n2pnZW6MWA6z96U1rgQxg\nS+dS/gCfdN4HiMOKnyUOzMzZaVm/25DRhiHKb27j8S5vMX1rKFj1i0O+AucaS9S4\nTbHI2zDfAgMBAAECggEAM7RmGjSlIWo0bQEosbvMLGcYu5xiUiZnB7b40XorBj6Z\nIK6hikV3zyJR4elTGbWHbetCvKKsO2AIcJPU0KS5r9IoxriRw7LSHrZLRMhLf6kS\nyz7g08k1xp3KpsYQ8LvAhNq1SeUWZ50boFnCvktvJMq11Wbic886iQT/zALu+fHC\nXvK4FiAsUyhiDym+KcHoyz8YGelo93/w64oeXWQLix5FqY7XQOTELIHnq+1Y3nVZ\neQdBiycGr+Llc92bssACpsBAXyVuDo3e9C3+3aPwO94UCY7efj2XJ96b68JCKKTJ\n3rOaGOqyTV2rCGBkY0Y4cxxkduxEwfV4bLnAAsc9NQKBgQDVCFmR1CylFCdBiR05\nmcscJR+14qSOH69098mqURiZKSrryxSU4dPbsZKJHDRb0NsPAybXwuOGyLNYFkAh\n8us5txLtACcfJ6Epwru8h0EX+ginS8L+cfV7KTSjewMh5n4SQJiUNsTg2eznJTGd\nZPHyXvKwi84lpcSqVmTdf+2KdQKBgQC+2QhU9pVqL10vfvPKxWRq8L0MueEhSBj5\nYYyGjZOGhgWXLTqxISazaDN4W7DPHaP6PK3yeUN42Z4NXPm4sTZmwhOuakLDYRK1\npsvgMf6+mEHFcEyCYQFIKnR5tFQQ2yXHzzL0AWFjFzus62BfpC4EbUapiHPViyFB\nC3RIHYcbgwKBgQC/2UEwvy9livy/XEfhc8sikZw5JN6kz4wnfm8y3s2UQSrSJpVm\nbuloVEZvC+NrPG/K2T/F11EoNN6uWXtwF2AXHfQBU73npLzS6vg/FF9exGG+p8/3\nbZFxVO5+u21avkSTE3FVKO2swRVEJI7F+/6YE0HkFEOWS+8Mp7k2cNrvcQKBgQC1\nWr4NJukjJ3EQwj3e8SaUbMHpRvWFT+LTj/wenIiU6+SCHSvJyGvjTmCivdbNaig5\nkdHmOY+BVqJXpoNzG4tLqJ9VPYrF6QgDcEYKfNfLvBm8ChPaTV4PerCGOnMsNWO4\ndM/BiVJG/HvrvRwupnBIKqNcVNtmEDkAe2b7ZHvQCwKBgQCOAW04VIc9L4VjghB0\nvplerU+qksnHrhXkYjJe9AmrPILZSRtgKO0Xee2ZiZ09HMRdQeKYE5W0WpTYfQ+D\ntGVBp++h5TGtHbylbZAMc9dkJ2J2NM5BBmOcF4o2Ou2LMdVUdEHUF44W/3kRVyYr\nPTv/OgNQsBVu+WadeP7IrELCwg==\n-----END PRIVATE KEY-----\n";
        [JsonPropertyName("client_email")]
        public string ClientEmail => "firebase-adminsdk-8j53l@lostandfound-test-b0e12.iam.gserviceaccount.com";
        [JsonPropertyName("client_id")]
        public string ClientId => "108248402349865269172";
        [JsonPropertyName("auth_uri")]
        public string AuthUri => "https://accounts.google.com/o/oauth2/auth";
        [JsonPropertyName("token_uri")]
        public string TokenUri => "https://oauth2.googleapis.com/token";
        [JsonPropertyName("auth_provider_x509_cert_url")]
        public string AuthProviderCertUrl => "https://www.googleapis.com/oauth2/v1/certs";
        [JsonPropertyName("client_x509_cert_url")]
        public string ClientCertUrl => "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-8j53l%40lostandfound-test-b0e12.iam.gserviceaccount.com";
        [JsonPropertyName("universe_domain")]
        public string UniverseDomain => "googleapis.com";
    }
}
