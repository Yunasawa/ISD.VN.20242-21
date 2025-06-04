using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public static partial class Function
    {
        private static SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        public static UID[] GetNewProductsList()
        {
            return _products.Select(p => p.Key).ToArray();
        }

        public static string GetImageURL(this UID id)
        {
            var type = _products[id].Type;
            var url = DataManager.ProductImageURL.Replace("{Type}", type.ToString()).Replace("{ID}", id.ToString());
            return url;
        }

        public static string GetStreamURL(this UID id)
        {
            var type = _products[id].Type;
            var url = DataManager.ProductStreamURL
                .Replace("{Type}", type.ToString())
                .Replace("{ID}", id.ToString())
                .Replace("{Format}", id.GetStreamFormat());
            return url;
        }

        public static string GetDurationText(this UID id)
        {
            var product = Main.Database.Products[id];

            return product.Type switch
            {
                Product.Type.Book => $"{product.Properties[Product.Property.NumberOfPage]} pages",
                Product.Type.CD => $"{product.Properties[Product.Property.Duration]}",
                _ => throw new Exception("Invalid Product Property for duration text")
            };
        }

        public static string GetDescriptionText(this UID id)
        {
            var product = Main.Database.Products[id];

            return product.Description.Replace("#", "\r\n\r\n");
        }
    
        public static string GetStreamFormat(this UID id)
        {
            var product = Main.Database.Products[id];

            return product.Type switch
            {
                Product.Type.Book => "txt",
                Product.Type.CD => "mp3",
                Product.Type.DVD => "mp4",
                Product.Type.LP => "mp3",
                _ => ""
            };
        }

        public static List<UID> GetSortedItemList(this List<UID> items, SortType sortType)
        {
            return sortType switch
            {
                SortType.ByTitleAToZ => items.OrderBy(id => _products[id].Title).ToList(),
                SortType.ByTitleZToA => items.OrderByDescending(id => _products[id].Title).ToList(),
                SortType.NewestReleaseDate => items.OrderByDescending(id => _products[id].PublicationDate).ToList(),
                SortType.OldestReleaseDate => items.OrderBy(id => _products[id].PublicationDate).ToList(),
                SortType.MostPopular => items.OrderByDescending(id => _products[id].SoldAmount).ToList(),
                SortType.LeastPopular => items.OrderBy(id => _products[id].SoldAmount).ToList(),
                SortType.HighestRating => items.OrderByDescending(id => _products[id].Review.AverageTotalRating).ToList(),
                SortType.LowestRating => items.OrderBy(id => _products[id].Review.AverageTotalRating).ToList(),
                SortType.HighestPrice => items.OrderByDescending(id => _products[id].Price).ToList(),
                SortType.LowestPrice => items.OrderBy(id => _products[id].Price).ToList(),
                SortType.LongestDuration => items,
                SortType.ShortestDuration => items,
                _ => null
            };
        }
    
        public static string[] GetProductGenreList(this Product.Type type)
        {
            return type switch
            {
                Product.Type.None => Array.Empty<string>(),
                Product.Type.Book => Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>().Select(i => i.ToString()).ToArray(),
                Product.Type.CD => Enum.GetValues(typeof(MusicGenre)).Cast<MusicGenre>().Select(i => i.ToString()).ToArray(),
                Product.Type.DVD => Enum.GetValues(typeof(MovieGenre)).Cast<MovieGenre>().Select(i => i.ToString()).ToArray(),
                Product.Type.LP => Enum.GetValues(typeof(MusicGenre)).Cast<MusicGenre>().Select(i => i.ToString()).ToArray(),
                _ => null
            };
        }
    }
}