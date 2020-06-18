using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelMotel.Business.Models;

namespace HotelMotel.Business.IFacade
{
    public interface IHmLogin
    {
        LoginResult LogResult(string userid, string password);
    }
}
