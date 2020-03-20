﻿using System;
using System.Collections.Generic;

namespace GildedRose
{
    public class GildedRose
    {
        private readonly IList<Item> _items;
        
        public GildedRose(IList<Item> items)
        {
            _items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in _items)
            {
                item.SellIn = GetUpdatedSellInValue(item);
                item.Quality = GetUpdatedItemQuality(item);
            }
        }

        private int GetAgedBrieQualityChange(Item item)
        {
            return item.SellIn < 0 ? 2 : 1;
        }
        
        private int GetBackStagePassQualityChange(Item item)
        {
            if (item.SellIn < 0)
            {
                // Reduce quality to zero if the event has already happened.
                return -1 * item.Quality;
            }
            
            if (item.SellIn >= 10)
            {
                return 1;
            }

            return item.SellIn >= 5 ? 2 : 3;
        }

        private int GetConjuredItemQualityChange(Item item)
        {
            return 2 * GetDefaultQualityChange(item);
        }
        
        private int GetDefaultQualityChange(Item item)
        {
            return item.SellIn < 0 ? -2 : -1;
        }

        private int GetChangeInQuality(Item item)
        {
            return item.Name switch
            {
                "Aged Brie" => GetAgedBrieQualityChange(item),
                "Backstage passes to a TAFKAL80ETC concert" => GetBackStagePassQualityChange(item),
                "Conjured Mana Cake" => GetConjuredItemQualityChange(item),
                _ => GetDefaultQualityChange(item)
            };
        }

        private int GetUpdatedItemQuality(Item item)
        {
            if (item.Name == "Sulfuras, Hand of Ragnaros")
            {
                // Quality of Sulfuras, Hand of Ragnaros should never change.
                return item.Quality;
            }
            
            var changeInQuality = GetChangeInQuality(item);
            
            // Other products cannot have negative quality or one above 50.
            return Math.Max(0, Math.Min(50, item.Quality + changeInQuality));
        }


        private int GetUpdatedSellInValue(Item item)
        {
            if (item.Name == "Sulfuras, Hand of Ragnaros")
            {
                return item.SellIn;
            }

            return item.SellIn - 1;
        }
    }
}