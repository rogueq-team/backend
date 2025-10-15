using System.Text;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Services;

public static class DealService
{
    static List<Deal> Deals = new List<Deal>();
    static int DealId = 0;

    public static List<Deal> GetAll() => Deals;

    public static Deal? FindByDealId(int DealId)
    {
        return Deals.Find(Deal => Deal.DealId == DealId);
    }

    public static List<Deal> FindByApplicationId(int ApplicationID)
    {
        return Deals.FindAll(Deal => Deal.ApplicationId == ApplicationID);
    }

    public static List<Deal> FindByAdvertiserId(int AdvertiserId)
    {
        return Deals.FindAll(Deal => Deal.AdvertiserId == AdvertiserId);
    }

    public static List<Deal> FindByPlatformId(int PlatformId)
    {
        return Deals.FindAll(Deal => Deal.PlatformId == PlatformId);
    }
    public static void SetDescription(Deal deal, string description)
    {
        deal.Description = description;
    }

    public static void SetStatus(Deal deal, string status)
    {
        deal.Status = status;
    }

    public static void Add(Deal deal)
    {
        DealId++;
        Deals.Add(deal);
    }

    public static void Delete(int DealId)
    {
        Deal? deal = FindByDealId(DealId);
        if (!(deal is null))
        {
            Deals.Remove(deal);
        }
    }
    
    public static void Update(Deal deal)
    {

        Deal? replaceDeal = FindByDealId(deal.DealId);
        if (!(replaceDeal is null))
        {
            int index = Deals.IndexOf(replaceDeal);
            Deals[index] = deal;
            Deals[index].DealId = replaceDeal.DealId;
        }
    }
    
}