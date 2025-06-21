using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    using PP = Product.Property;

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

            return product.Description.Replace("#", "\r\n");
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
    
        public static string[] GetAllProductGenreString(this Product.Type type)
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

        public static string[] GetProductGenresString(this Product.Type type, ushort genreValue)
        {
            var genreType = type.GetGenreType();
            if (genreType == null)
            {
                return Array.Empty<string>();
            }

            var genres = Enum.GetValues(genreType)
                .Cast<Enum>()
                .Where(flag =>
                {
                    var flagValue = Convert.ToUInt16(flag);
                    return flagValue != 0 && (genreValue & flagValue) == flagValue;
                })
                .Select(flag => flag.ToString())
                .ToArray();

            return genres;
        }

        public static float GetAverageCharge(this DeliveryType type)
        {
            return type switch
            {
                DeliveryType.Normal => UnityEngine.Random.Range(5f, 10f),
                DeliveryType.Fast => UnityEngine.Random.Range(10f, 20f),
                DeliveryType.Rush => UnityEngine.Random.Range(20f, 30f),
                _ => 0
            };
        }

        public static string GetOrderCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return "JAMOS" + new string(Enumerable.Range(0, 15).Select(_ => chars[UnityEngine.Random.Range(0, chars.Length)]).ToArray());
        }
    
        public static Type GetGenreType(this Product.Type type)
        {
            return type switch
            {
                Product.Type.Book => typeof(BookGenre),
                Product.Type.CD => typeof(MusicGenre),
                Product.Type.DVD => typeof(MovieGenre),
                Product.Type.LP => typeof(MusicGenre),
                _ => null
            };
        }
    
        public static PP[] GetProductProperties(this Product.Type type)
        {
            return type switch
            {
                Product.Type.Book => new PP[] { PP.Language, PP.NumberOfPage },
                Product.Type.CD => new PP[] { PP.Album, PP.Duration },
                Product.Type.DVD => new PP[] { PP.Studio },
                _ => new PP[0]
            };
        }
    }
}