using System;
using System.Collections.Generic;
using System.Linq;
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

    [System.Serializable]
    public class MessageItem
    {
        public AccountType Type;
        public string Message;
    }

    [System.Serializable]
    public class MessageList
    {
        public List<MessageItem> Messages = new();
    }

    [System.Serializable]
    public class OrderItem
    {
        public DateTime OrderDate = DateTime.Now;
        public SerializableDictionary<UID, uint> OrderAmounts = new();

        public OrderItem Initialize()
        {
            OrderAmounts = Main.Runtime.OrderedAmounts;
            return this;
        }
    }

    [Serializable]
    public class RuntimeData
    {
        public UID AccountID;
        public List<(SearchingSuggestionType Type, string Value)> SearchingHistory = new();
        public SerializableDictionary<UID, LikedFeedback> LikedFeedbacks = new();
        public List<string> FavoriteGenres = new();
        public List<UID> CartedProducts = new();
        public List<UID> ProductCollection = new();
        public SerializableDictionary<UID, MessageList> Messages = new();
        public SerializableDictionary<string, OrderItem> Orders = new();
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
        public SerializableDictionary<UID, uint> OrderedAmounts = new();
        public ushort Discount = 0;

        public void Reset()
        {
            Data = new();
        }
    }
}