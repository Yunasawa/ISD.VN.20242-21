using System;
using System.Collections.Generic;
using UnityEngine;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    [System.Serializable]
    public class LikedFeedback
    {
        public List<UID> Feedbacks = new();
    }

    [System.Serializable]
    public class BookedRoom
    {
        public List<UID> Rooms = new();
    }

    [Serializable]
    public class RuntimeData
    {
        public UID AccountID;
        public List<(SearchingSuggestionType Type, string Value)> SearchingHistory = new();
        public SerializableDictionary<UID, LikedFeedback> LikedFeedbacks = new();
        public List<string> FavoriteGenres = new();
    }

    [CreateAssetMenu(fileName = "RuntimePropertiesSO", menuName = "YNL - Checkotel/RuntimePropertiesSO")]
    public class RuntimePropertiesSO : ScriptableObject
    {
        public RuntimeData Data = new();

        public bool IsSearchTimeApplied = false;
        public Product.Type SearchingProductType = Product.Type.None;
        public string SearchingInput = string.Empty;
        public UID SelectedProduct = 0;
        public string SearchingGenre = string.Empty;

        public void Reset()
        {
            Data = new();
        }
    }
}