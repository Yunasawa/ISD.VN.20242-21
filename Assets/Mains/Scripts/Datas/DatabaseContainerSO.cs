using System.Collections.Generic;
using UnityEngine;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    [CreateAssetMenu(fileName = "DatabaseContainerSO", menuName = "YNL - Checkotel/DatabaseContainerSO")]
    public class DatabaseContainerSO : ScriptableObject
    {
        public SerializableDictionary<UID, Account> Accounts = new();
        public SerializableDictionary<UID, ReviewFeedback> Feedbacks = new();
        public SerializableDictionary<UID, Book.Data> Books = new();
        public SerializableDictionary<UID, CD.Data> CDs = new();
        public SerializableDictionary<UID, DVD.Data> DVDs = new();
        public SerializableDictionary<UID, LP.Data> LPs = new();
    }
}