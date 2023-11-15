using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LostAndFound.API.Extensions
{
    public class FirestoreProvider
    {
        private readonly FirestoreDb _fireStoreDb = null!;

        public FirestoreProvider(FirestoreDb fireStoreDb)
        {
            _fireStoreDb = fireStoreDb;
        }

        public async Task UpdateUserDisplayName(string uid, string newDisplayName)
        {
            var ct = new CancellationToken();
            var document = _fireStoreDb.Collection("users").Document(uid);
            await document.UpdateAsync("displayName", newDisplayName, cancellationToken: ct);
        }

        public async Task UpdateUserPhotoUrl(string uid, string newUrl)
        {
            var ct = new CancellationToken();
            var document = _fireStoreDb.Collection("users").Document(uid);
            await document.UpdateAsync("photoUrl", newUrl, cancellationToken: ct);
        }

        public async Task CreateNewUser(string displayName, string email, string photoUrl, string uid)
        {
            var ct = new CancellationToken();
            //create new user
            var document = _fireStoreDb.Collection("users").Document(uid);
            await document.CreateAsync(
                new
                {
                    displayName = displayName,
                    email = email,
                    photoUrl = photoUrl,
                    uid = uid
                },
                cancellationToken: ct);

            //create their userChats
            await _fireStoreDb.Collection("userChats").Document("random uid").SetAsync(new Dictionary<string, object>(), null, ct);
        }

        public async Task<bool> GetUser(string id)
        {
            var cancellationToken = new CancellationToken();
            var document = await _fireStoreDb.Collection("users").Document(id).GetSnapshotAsync();
            if (document.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
