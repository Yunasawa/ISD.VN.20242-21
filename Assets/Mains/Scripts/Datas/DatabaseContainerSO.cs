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
        public SerializableDictionary<UID, Product.Data> Products = new();

        public SerializableDictionary<UID, Texture2D> Images = new();
    }
}