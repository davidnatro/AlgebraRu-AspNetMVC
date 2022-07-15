using mednik.Data.Base;
using mednik.Models;
using Microsoft.AspNetCore.Mvc;

namespace mednik.Controllers;

public class ServicesController : Controller
{
    private EntityBaseRepository<Services> _servicesEntity;

    public ServicesController(EntityBaseRepository<Services> servicesEntity)
    {
        _servicesEntity = servicesEntity;
    }
}