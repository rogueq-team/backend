using System.Text;
using System.Threading.Tasks;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class DealService
{
    private readonly AppDbContext _db;

    public DealService(AppDbContext context)
    {
        _db = context;
    }
}